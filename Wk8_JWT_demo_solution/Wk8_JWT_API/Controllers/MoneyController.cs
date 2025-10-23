using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wk8_JWT_API.Controllers
{
    /// <summary>
    /// Provides endpoints related to money operations for the authenticated user.
    /// </summary>
    /// <remarks>
    /// All endpoints in this controller require an authenticated user. Requests without valid authentication
    /// will be denied (401 Unauthorized).
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MoneyController : ControllerBase
    {

        /// <summary>
        /// Retrieves the current money amount for the authenticated user.
        /// </summary>
        /// <returns>
        /// A 200 OK response containing a JSON object with a single property named "money" representing
        /// the current amount (integer).
        /// </returns>
        /// <response code="200">Returns the current money amount as JSON ({ "money": 1000 }).</response>
        /// <response code="401">Unauthorized - the caller is not authenticated.</response>
        /// 

        [HttpGet(Name = "GetMoney")]
        public IActionResult Get()
        {
            var money = 1000;
            return Ok(new { money });
        }
    }
}
