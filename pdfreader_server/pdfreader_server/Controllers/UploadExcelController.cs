//filePath = Path.Combine(@"C:\Users\clakshmisaicharan\Desktop\Project\Pdf_extractor-\pdfreader_server\pdfreader_server\Excel\", file.FileName);





using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;





namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadExcelController : ControllerBase
    {
        string fileName = "";
        string filePath = "";
        static string Fpath;



        private readonly IConfiguration _configuration;
        public UploadExcelController(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            //public static string fileName;



            try
            {



                if (file == null || file.Length == 0)





                {





                    return BadRequest("No file selected");





                }







                filePath = Path.Combine(@"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Excel\", file.FileName);



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
            catch (Exception ex)
            {



                return BadRequest($"An error occurred while uploading the file: {ex.Message}");
            }



        }
        [HttpGet]
        public List<string> GetCols(int userid, string filenameexcel)
        {
            //string filePath = @"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Excel\file.xlsx";
            //filePath += HttpContext.Session.GetString("FilePath");
            //filePath += UploadController.fileName;







            Console.WriteLine();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("UTF-8");





            //string[] cols = new string[10];
            //int k = 0;





            List<string> cols2 = new List<string>();
            try
            {
                if (!filenameexcel.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new List<string>();
                    throw new ArgumentException("The specified file is not a valid PDF.");
                }





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
                string selectquery = @"
                    select * from dbo.ExcelFiles where userId = '" + userid + @"'";
                DataTable selecttable = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("LoginAppCon");
                SqlDataReader selectmyReader;
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(selectquery, myCon))
                    {
                        selectmyReader = myCommand.ExecuteReader();
                        selecttable.Load(selectmyReader); ;



                        selectmyReader.Close();
                        myCon.Close();
                    }
                }
                if (selecttable.Rows.Count != 0)
                {
                    string deleteQuery = @"delete from dbo.ExcelFiles where userId= '" + userid + @"'";
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(deleteQuery, myCon))
                        {
                            selectmyReader = myCommand.ExecuteReader();
                            selecttable.Load(selectmyReader); ;



                            selectmyReader.Close();
                            myCon.Close();
                        }
                    }
                }
                for (int i = 0; i < cols2.Count; i++)
                {
                    string query = @"
                    insert into dbo.ExcelFiles values 
                       (
                    '" + userid + @"'
                    ,'" + filenameexcel + @"'
                    ,'" + cols2[i] + @"'

                    )
                    ";
                    DataTable table = new DataTable();
                    //string sqlDataSource = _configuration.GetConnectionString("LoginAppCon");
                    SqlDataReader myReader;
                    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        {
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader); ;



                            myReader.Close();
                            myCon.Close();
                        }
                    }



                }





                return cols2;
            }



            catch (ArgumentException ex)
            {





                //return new List<string>();
                // or
                throw;
            }





            catch (Exception ex)
            {
                // Handle the exception here or log it for further investigation.
                // You can return a generic error response to the client.
                throw; // Optionally rethrow the exception if you want it to propagate further.
            }
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