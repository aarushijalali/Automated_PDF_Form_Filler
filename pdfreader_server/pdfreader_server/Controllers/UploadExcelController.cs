using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;

namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadExcelController : ControllerBase
    {
        string fileName="";
        string  filePath="";
        static string Fpath;

        
        [HttpPost]

        public async Task<IActionResult> Upload(IFormFile file)

        {
            //public static string fileName;

            if (file == null || file.Length == 0)

            {

                return BadRequest("No file selected");

            }


            filePath = Path.Combine(@"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Excel", file.FileName);
            Fpath = filePath;

            fileName = file.FileName;

            Console.WriteLine(fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))

            {

                await file.CopyToAsync(stream);

            }

            //HttpContext.Session.SetString("FilePath", file.FileName);

            //return RedirectToAction("GetCols", "ExtractExcelController", new {filePath = filePath});

            return Ok(new { message = "File uploaded successfully" });

        }
        [HttpGet]
        public List<string> GetCols()
        {
            //string filePath = @"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Excel\file.xlsx";
            //filePath += HttpContext.Session.GetString("FilePath");
            //filePath += UploadController.fileName;


            
            Console.WriteLine(fileName);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("UTF-8");

            //string[] cols = new string[10];
            //int k = 0;

            List<string> cols2 = new List<string>();

            // Read the Excel file using ExcelDataReader
            using (var stream = System.IO.File.Open(Fpath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                // Create an ExcelDataReader object for the Excel file stream
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Read the data from the first worksheet
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true // Treat the first row as column headers
                        }
                    });

                    // Access the first table (worksheet) in the DataSet
                    var dataTable = dataSet.Tables[0];

                    // Get the column names
                    var columnNames = GetColumnNames(dataTable);

                    // Display the column names
                    foreach (var columnName in columnNames)
                    {
                        cols2.Add(columnName);
                        //cols[k] = columnName;
                        //k++;
                    }

                    //foreach (string col in cols)
                    //{
                    //    Console.WriteLine("Element: " + col);
                    //}
                }
            }

            return cols2;
        }

        private string[] GetColumnNames(DataTable dataTable)
        {
            // Get the column names from the Columns collection
            var columnNames = new string[dataTable.Columns.Count];
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                columnNames[i] = dataTable.Columns[i].ColumnName;
            }

            return columnNames;
        }
    }
}
