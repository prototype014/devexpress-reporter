namespace DevExpressASPNETCoreReporting.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Content { get; set; }
    }
}
