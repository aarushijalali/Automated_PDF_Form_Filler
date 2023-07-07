using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdfreader_server.Models;

namespace pdfreader_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        Dictionary<string, string> fieldPairs = new Dictionary<string, string>();

        [HttpPost]
        public JsonResult Hi(InputObj inputObj)
        {
            
            string excelValue = inputObj.excel;
            string pdfValue = inputObj.pdf;

            //fieldPairs.Add(excelValue, pdfValue);
            fieldPairs[pdfValue] = excelValue;
            
            System.Diagnostics.Debug.Write(fieldPairs);
            return new JsonResult(fieldPairs);
            
        }
        [HttpGet]
        public Dictionary<string,string> Get()
        {
            return fieldPairs;
        }
    }
}
