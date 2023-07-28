using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;



namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileNameController : ControllerBase
    {



        private readonly IConfiguration _configuration;
        public FileNameController(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        [HttpGet]
        public JsonResult getFieldNames(int userid)
        {
            string query = @"
            select * from dbo.Files where userId = '" + userid + @"'";
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
            return new JsonResult(table);
        }



    }
}