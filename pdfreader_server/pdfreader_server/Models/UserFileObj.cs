using Azure.Identity;



namespace pdfreader_server.Models
{
    public class UserFileObj
    {
        public Dictionary<string, int> myMap { get; set; }
        public string pdfName { get; set; }
        public string excelName { get; set; }
        public int userId { get; set; }
    }
}