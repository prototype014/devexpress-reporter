using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DevExpress.XtraReports.Web.Extensions;
using DevExpressASPNETCoreReporting.DevExpressOverrides;
using DevExpress.XtraReports.Web.WebDocumentViewer.Native;
using DevExpress.XtraReports.Web.QueryBuilder.Native;
using DevExpress.XtraReports.Web.ReportDesigner.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner.Native;
using DevExpress.XtraReports.Web.WebDocumentViewer.Native.Services;
using DevExpress.DataAccess.Web;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.Web.QueryBuilder;
using System.IO;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using DevExpressASPNETCoreReporting.Data;
using Microsoft.EntityFrameworkCore;

namespace DevExpressASPNETCoreReporting {
    public class Startup {
        AppBuilderServiceRegistrator serviceRegistrator;
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services            
            services.AddMvc()            
               .ConfigureApplicationPartManager(manager => {
                   var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                   manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                   manager.FeatureProviders.Add(new ReferencesMetadataReferenceFeatureProvider());
               });
              this.serviceRegistrator = new AppBuilderServiceRegistrator(services);
            WebDocumentViewerBootstrapper.RegisterStandardServices(serviceRegistrator);
            QueryBuilderBootstrapper.RegisterStandardServices(serviceRegistrator);
            ReportDesignerBootstrapper.RegisterStandardServices(serviceRegistrator,
                () => serviceRegistrator.GetService<IReportManagementService>(),
                () => serviceRegistrator.GetService<IStoragesCleaner>(),
                () => serviceRegistrator.GetService<IConnectionProviderFactory>(),
                () => serviceRegistrator.GetService<IReportSqlDataSourceWizardService>(),
                () => serviceRegistrator.GetService<ISqlDataSourceConnectionParametersPatcher>()
            );
            services.AddTransient<ISqlDataSourceConnectionParametersPatcher, BlankSqlDataSourceConnectionParametersPatcher>();
            services.AddTransient<IWebDocumentViewerUriProvider, ASPNETCoreUriProvider>();
            services.AddTransient<IWebDocumentViewerReportResolver, ASPNETCoreReportResolver>();
            services.AddDbContext<ReportContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ManageConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            string baseDir = env.ContentRootPath;
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(baseDir, "App_Data"));

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            serviceRegistrator.UseGeneratedServices(app.ApplicationServices);
            DefaultWebDocumentViewerContainer.Current = DefaultQueryBuilderContainer.Current = DefaultReportDesignerContainer.Current = app.ApplicationServices;

            ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension());
        }
    }
}
