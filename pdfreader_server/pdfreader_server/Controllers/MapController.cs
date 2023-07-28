using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdfreader_server.Models;
using ExcelDataReader;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;





namespace pdfreader_server.Controllers



{



    [Route("api/[controller]")]



    [ApiController]



    public class MapController : ControllerBase



    {



        private readonly IConfiguration _configuration;
        public MapController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        Dictionary<string, string> fieldPairs = new Dictionary<string, string>();
        string filePath = @"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Excel\";
        string existingPdfPath = @"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Pdf\";



        [HttpPost("ReceiveMapData")]
        public void ReceiveMapData(UserFileObj userFileObj)



        {



            Console.WriteLine(userFileObj.pdfName);
            existingPdfPath += userFileObj.pdfName;
            filePath += userFileObj.excelName;





            List<List<string>> excelData = ExcelRowExtract();



            int k = 0;





            foreach (var excel in excelData)



            {



                using (var pdfReader = new PdfReader(existingPdfPath))



                {
                    string f = userFileObj.pdfName + k.ToString() + ".pdf";
                    string outputPdfPath = Path.Combine(@"C:\Users\aajalali\Desktop\Automation of Pdf extraction and writing\pdfreader_server\pdfreader_server\Pdf\", f);





                    using (var pdfWriter = new PdfWriter(outputPdfPath))



                    {



                        using (var pdfDocument = new PdfDocument(pdfReader, pdfWriter))



                        {



                            var form = PdfAcroForm.GetAcroForm(pdfDocument, true);





                            var fields = form.GetAllFormFields();





                            foreach (KeyValuePair<string, PdfFormField> fieldName in fields)



                            {



                                var field = form.GetField(fieldName.Key);



                                //string fieldname = fieldName.Key;
                                if (userFileObj.myMap.ContainsKey(fieldName.Key))
                                {
                                    string value = excel[userFileObj.myMap[fieldName.Key]];



                                    if (field != null)



                                    {



                                        field.SetValue(value);



                                        //string val = myMap["field1"];



                                    }
                                }



                            }



                            pdfDocument.Close();



                        }



                    }



                }



                k++;





            }



            string query = @"
            select * from dbo.files where userId = '" + userFileObj.userId + @"'";



            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("LoginAppCon");
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



            if (table.Rows.Count != 0)
            {



                query = @"delete from dbo.files where userId = '" + userFileObj.userId + @"' ";



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





            query = @"
                    insert into dbo.files values 
                       (
                    '" + userFileObj.userId + @"'
                    ,'" + userFileObj.excelName + @"'
                    ,'" + userFileObj.pdfName + @"'

                    )
                    ";



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







        private List<List<string>> ExcelRowExtract()



        {



            using (var stream = System.IO.File.OpenRead(filePath))



            {



                using (var reader = ExcelReaderFactory.CreateOpenXmlReader(stream))



                {





                    DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()



                    {



                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()



                        {



                            UseHeaderRow = true // Assuming the first row contains headers



                        }



                    });







                    int totalRows = result.Tables[0].Rows.Count;



                    //Console.WriteLine(totalRows);





                    var allRowsData = new List<List<string>>();







                    //int desiredRow = 1; // The desired row to extract values (e.g., row 2)



                    int rowCount = 0;







                    while (reader.Read())



                    {



                        // Skip the header row as it was already included in the data set



                        if (rowCount > 0)



                        {



                            var rowData = new List<string>();





                            for (int column = 0; column < reader.FieldCount; column++)



                            {



                                var cellValue = reader.GetValue(column)?.ToString();



                                rowData.Add(cellValue);



                                Console.WriteLine("Row " + rowCount + ", Column " + column + ": " + cellValue);



                            }





                            allRowsData.Add(rowData);



                        }





                        rowCount++;



                    }



                    //Console.WriteLine(allRowsData.Count);





                    return allRowsData;



                }





            }







        }





    }



}

