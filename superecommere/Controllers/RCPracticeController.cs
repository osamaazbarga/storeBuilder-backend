using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace superecommere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RCPracticeController : ControllerBase
    {
        [HttpGet("public")]   
        public IActionResult Public()
        {
            return Ok("public");
        }

        #region Roles

        [HttpGet("admin-role")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminRole()
        {
            return Ok("admin role");
        }

        [HttpGet("manager-role")]
        [Authorize(Roles = "Manager")]
        public IActionResult ManagerRole()
        {
            return Ok("manager role");
        }

        //[HttpGet("customer-role")]
        //[Authorize(Roles = "Customer")]
        //public IActionResult VipCustomerRole()
        //{
        //    return Ok("vip customer role");
        //}

        [HttpGet("admin-or-manager-role")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult AdminOrManagerRole()
        {
            return Ok("admin or manager role");
        }

        [HttpGet("admin-or-customer-role")]
        [Authorize(Roles = "Admin,Customer")]
        public IActionResult AdminOrCustomerRole()
        {
            return Ok("admin or customer role");
        }
        #endregion

        #region Policy

        [HttpGet("admin-policy")]
        [Authorize(policy: "AdminPolicy")]
        public IActionResult AdminPolicy()
        {
            return Ok("admin policy");
        }

        [HttpGet("manager-policy")]
        [Authorize(policy: "ManagerPolicy")]
        public IActionResult ManagerPolicy()
        {
            return Ok("manager policy");
        }

        [HttpGet("customer-policy")]
        [Authorize(policy: "CustomerPolicy")]
        public IActionResult CustomerPolicy()
        {
            return Ok("customer policy");
        }

        [HttpGet("admin-or-manager-policy")]
        [Authorize(policy: "AdminOrMangerPolicy")]
        public IActionResult AdminOrManagerPolicy()
        {
            return Ok("admin or manager policy");
        }

        [HttpGet("admin-and-manager-policy")]
        [Authorize(policy: "AdminAndMangerPolicy")]
        public IActionResult AdminAndManagerPolicy()
        {
            return Ok("admin and manager policy");
        }

        [HttpGet("all-role-policy")]
        [Authorize(policy: "AllRolesPolicy")]
        public IActionResult AllRolesPolicy()
        {
            return Ok("all roles policy");
        }

        #endregion


        #region Claim Policy

        [HttpGet("admin-email-policy")]
        [Authorize(policy: "AdminEmailPolicy")]
        public IActionResult AdminEmailPolicy()
        {
            return Ok("admin email policy");
        }

        [HttpGet("osama3-surname-policy")]
        [Authorize(policy: "osama3Policy")]
        public IActionResult OsamaSurnamePolicy()
        {
            return Ok("osama surname customer policy");
        }

        [HttpGet("manager-email-and-osama2-surname-policy")]
        [Authorize(policy: "ManagerEmailAndOsama2SurnamePolicy")]
        public IActionResult ManagerEmailAndOsama2SurnamePolicy()
        {
            return Ok("manager and osama2 surname policy");
        }

        [HttpGet("vip-policy")]
        [Authorize(policy: "VIPPolicy")]
        public IActionResult VIPPolicy()
        {
            return Ok("Vip policy");
        }

        #endregion
    }
}
