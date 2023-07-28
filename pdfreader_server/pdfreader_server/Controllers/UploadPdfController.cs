using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Configuration;
using pdfreader_server.Models;
using System.Data;
using System.Data.SqlClient;



namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPdfController : ControllerBase
    {
        string fileName = "";
        string filePath = "";
        static string Fpath;
        List<string> pdfFields = new List<string>();



        private readonly IConfiguration _configuration;
        public UploadPdfController(IConfiguration configuration)
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





                filePath = Path.Combine(@"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Pdf\", file.FileName);
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
        public List<string> getPdfFields(string fileName, int userid)
        {
            try
            {
                if (!fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return new List<string>();
                    throw new ArgumentException("The specified file is not a valid PDF.");
                }
                using (var pdfReader = new PdfReader(@"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Pdf\" + fileName))
                //change the path to variable filepath TODO
                {
                    //using (var pdfWriter = new PdfWriter(/*existingPdfPath + ".temp"*/outputPdf))
                    {
                        using (var pdfDocument = new PdfDocument(pdfReader))
                        {
                            var form = PdfAcroForm.GetAcroForm(pdfDocument, true);



                            var fields = form.GetAllFormFields();



                            foreach (KeyValuePair<string, PdfFormField> fieldName in fields)
                            {
                                var field = form.GetField(fieldName.Key);
                                if (field != null)
                                {
                                    pdfFields.Add(fieldName.Key);
                                }



                            }
                            pdfDocument.Close();
                        }
                    }
                }



                string selectquery = @"
                    select * from dbo.PdfFiles where userId = '" + userid + @"'";
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
                    string deleteQuery = @"delete from dbo.PdfFiles where userId= '" + userid + @"'";
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



                for (int i = 0; i < pdfFields.Count; i++)
                {
                    string query = @"
                    insert into dbo.PdfFiles values 
                       (
                    '" + userid + @"'
                    ,'" + fileName + @"'
                    ,'" + pdfFields[i] + @"'

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





                return pdfFields;
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
    }
}