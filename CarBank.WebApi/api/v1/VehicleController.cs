using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarBank.WebApi.api.v1
{

    [Authorize(Policy = "ApiUser")]
    [Produces("application/json")]
    [Route("api/v1/[controller]/[action]")]

    public class VehicleController : Controller
    {
        public VehicleController()
        {

        }
        
        [HttpPost]
        [ActionName("search")]
        public async Task<string> Search()
        {
            await Task.Delay(1);
            return "Toyota Corrola";
        }
    }
}