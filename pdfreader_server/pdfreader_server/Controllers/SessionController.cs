using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;



namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {





        private readonly IConfiguration _configuration;
        public SessionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        [HttpGet]
        public JsonResult getFields(int userid)
        {
            string query = @"
            select userFields from dbo.PdfFiles where userId = '" + userid + @"'";
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



            //List<List<string>> result = new List<List<string>>();
            //for(int i=0;i< table.Rows.Count; i++)
            //{
            //    string res = table.Rows[i].ToString(); 
            //    result[0].Add(res);
            //}



            query = @"
            select userFieldExcel from dbo.ExcelFiles where userId = '" + userid + @"' ";
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



            //for(int i = 0; i < table.Rows.Count; i++)
            //{
            //    result[1].Add(table.Rows[i].ToString());
            //}



            //return result;



            return new JsonResult(table);
        }
    }
}