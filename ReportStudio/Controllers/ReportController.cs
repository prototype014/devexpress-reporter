using Microsoft.AspNetCore.Mvc;
using ReportStudio.Data;
using System.Linq;

namespace ReportStudio.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportContext _context;

        public ReportController(ReportContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Reports.ToList());
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        public IActionResult Run(int id)
        {
            return View();
        }
    }
}