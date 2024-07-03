using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using superecommere.Data;
using superecommere.Models.Domain;
using superecommere.Models.DTO;
using superecommere.Repositories.Interface;
using superecommere.Services;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace superecommere.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _superEcommereDbContext;
        private readonly ITblUserRepository tblUserRepository;
        private readonly JWTService _jwtService;
        private readonly SignInManager<TblUser> _signInManager;
        private readonly UserManager<TblUser> _userManager;
        private readonly EmailService _emailService; 
        private readonly IConfiguration _config;
        private readonly HttpClient _facebookHttpClient;





        public UsersController(JWTService jwtService,
            SignInManager<TblUser> signInManager,
            UserManager<TblUser> userManager,
            ApplicationDbContext superEcommereDbContext,
            ITblUserRepository tblUserRepository,
            EmailService emailService,
            IConfiguration config,
            HttpClient facebookHttpClient)
        {

            _superEcommereDbContext = superEcommereDbContext;
            this.tblUserRepository = tblUserRepository;
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _config = config;
            _facebookHttpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com")
            };
        }

        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult<TblUserDto>> RefreshUserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            if(await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized("You have been locked out");
            }
            return await CreateApplicationUserDto(user);
            //if (user == null)
            //{
            //    return Unauthorized("Invilad Email or password");
            //}
            //if (user.EmailConfirmed == false)
            //    return Unauthorized("Please confirm your email");
            //var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            //if (!result.Succeeded)
            //    return Unauthorized("Invilad Email or password");
            //return CreateApplicationUserDto(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TblUserDto>> Login(LoginDto model)
        {
            var user =await _userManager.FindByNameAsync(model.UserName);
            if(user == null)
            {
                return Unauthorized("Invilad Email or password");
            }
            if(user.EmailConfirmed==false)
                return Unauthorized("Please confirm your email");
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if (result.IsLockedOut)
            {
                return Unauthorized(string.Format("Your account has been locked. you should wait until {0} (UTC time) to be able to login",user.LockoutEnd));
            }
            if (!result.Succeeded)
            {
                //user has input an invailed password
                if (!user.UserName.Equals(SD.AdminUserName))
                {
                    await _userManager.AccessFailedAsync(user);
                }
                if (user.AccessFailedCount >= SD.MaximumLoginAttempts)
                {
                    //lock the user for one day
                    await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(1));
                    return Unauthorized(string.Format("Your account has been locked. you should wait until {0} (UTC time) to be able to login", user.LockoutEnd));
                }
                return Unauthorized("Invilad Email or password");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);
            return await CreateApplicationUserDto(user);
        }
        [HttpPost("login-with-third-party")]
        public async Task<ActionResult<TblUserDto>> LoginWithThirdParty(LoginWithExternalDto model)
        {

            if (model.Provider.Equals(SD.Facebook))
            {
                try
                {
                    if (!FacebookValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                    {
                        return Unauthorized("unable to login with facebook");
                    }
                }
                catch (Exception)
                {
                    return Unauthorized("unable to login with facebook");
                }
            }
            else if (model.Provider.Equals(SD.Google))
            {
                try
                {
                    if (!GoogleValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                    {
                        return Unauthorized("unable to login with google");
                    }
                }
                catch (Exception)
                {
                    return Unauthorized("unable to login with google");

                }
            }
            else
            {
                return BadRequest("Invalid providor");
            }
            var user =await _userManager.Users.FirstOrDefaultAsync(x=>x.UserName == model.UserId&& x.Provider==model.Provider);
            if(user==null)
                return Unauthorized("Unable to find your account");
            return await CreateApplicationUserDto(user);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if(await CheckEmailExistAsync(model.Email))
            {
                return BadRequest($"An existing account is using {model.Email},email address. please try with another email address");
            }
            var userToAdd = new TblUser
            {
                Email = model.Email.ToLower(),
                UserName = model.Email.ToLower(),
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                PhoneNumber=model.Phone.ToLower(),
                Merchant= model.Merchant.ToLower(),


            };

            var result =await _userManager.CreateAsync(userToAdd,model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(userToAdd, SD.CustomerRole);
            try
            {
                if(await SendConfirmEmailAsync(userToAdd))
                {
                    return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created,please confirm your email address" }));
                }
                return BadRequest("Failed to send email.Please contact admin");


            }
            catch(Exception ex) { 
                return BadRequest("Failed to send email.Please contact admin");
            }
            

        }

        [HttpPost("registerWithThirdParty")]
        public async Task<ActionResult<TblUserDto>> RegisterWithThirdParty(RegisterWithExternalDto model)
        {
            if (model.Provider.Equals(SD.Facebook))
            {
                try
                {
                    if (!FacebookValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                    {
                        return Unauthorized("unable to register with facebook");
                    }
                }
                catch (Exception)
                {
                    return Unauthorized("unable to register with facebook");
                }

            }
            else if (model.Provider.Equals(SD.Google))
            {
                try
                {
                    if (!GoogleValidatedAsync(model.AccessToken, model.UserId).GetAwaiter().GetResult())
                    {
                        return Unauthorized("unable to register with foogle");
                    }
                }
                catch (Exception)
                {
                    return Unauthorized("unable to register with google");
                }

            }
            else
            {
                return BadRequest("Invalid providor");
            }


            var user = await _userManager.FindByNameAsync(model.UserId);
            if (user != null)
                return BadRequest(string.Format("You have an account already. please login with your {0}", model.Provider));
            var userToAdd = new TblUser
            {
                //Email = model.Email.ToLower(),
                UserName = model.UserId,
                FirstName = model.FirstName.ToLower(),
                LastName = model.LastName.ToLower(),
                Provider=model.Provider,

            };

            var result = await _userManager.CreateAsync(userToAdd);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(userToAdd, SD.CustomerRole);
            return await CreateApplicationUserDto(userToAdd);
            try
            {
                if (await SendConfirmEmailAsync(userToAdd))
                {
                    return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created,please confirm your email address" }));
                }
                return BadRequest("Failed to send email.Please contact admin");


            }
            catch (Exception ex)
            {
                return BadRequest("Failed to send email.Please contact admin");
            }

            //if (await CheckEmailExistAsync(model.Email))
            //{
            //    return BadRequest($"An existing account is using {model.Email},email address. please try with another email address");
            //}
            //var userToAdd = new TblUser
            //{
            //    Email = model.Email.ToLower(),
            //    UserName = model.Email.ToLower(),
            //    FirstName = model.FirstName.ToLower(),
            //    LastName = model.LastName.ToLower(),

            //};

            //var result = await _userManager.CreateAsync(userToAdd, model.Password);
            //if (!result.Succeeded)
            //{
            //    return BadRequest(result.Errors);
            //}
            //try
            //{
            //    if (await SendConfirmEmailAsync(userToAdd))
            //    {
            //        return Ok(new JsonResult(new { title = "Account Created", message = "Your account has been created,please confirm your email address" }));
            //    }
            //    return BadRequest("Failed to send email.Please contact admin");


            //}
            //catch (Exception ex)
            //{
            //    return BadRequest("Failed to send email.Please contact admin");
            //}


        }


        [HttpPut("confirmEmail")]
        public async Task<IActionResult> confirmEmail(ConfirmEmailDto model)
        {
            var user= await _userManager.FindByEmailAsync(model.Email);
            if(user==null) return Unauthorized("This email address has not been registered yet");
            if (user.EmailConfirmed==true) return BadRequest("this email was confirmed berfore. Please login to your account.");
            try
            {
                var decodedTokenBytes= WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken=Encoding.UTF8.GetString(decodedTokenBytes);    
                var result = await _userManager.ConfirmEmailAsync(user,decodedToken);
                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Email confirmed", message = "Your email address is confirmed,you can login now." }));
                }
                return BadRequest("Invalid token. Please try again.");

            }
            catch(Exception)
            {
                return BadRequest("Invalid token. Please try again.");
            }

        }


        [HttpPost("resendEmailConfirmationLink/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");
            var user=await _userManager.FindByEmailAsync(email);
            if(user==null) return Unauthorized("This email address has not been registered yet");
            if (user.EmailConfirmed == true) return BadRequest("this email was confirmed before. Please login to your account.");
            try
            {
                if (await SendConfirmEmailAsync(user)) return Ok(new JsonResult(new { title = "Confirmation link sent", message = "Please confirm your email address" }));
                return BadRequest("Failed to send email.Please contact admin.");
            }
             
            catch (Exception)
            {
                return BadRequest("Failed to send email.Please contact admin.");
            }


        }



        [HttpPost("forgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassowrd(string email)
        {
            if (string.IsNullOrEmpty(email)) return BadRequest("Invalid email");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized("This email address has not been registered yet");
            if (user.EmailConfirmed == false) return BadRequest("Please confirm your email address first.");
            try
            {
                if(await SendForgotPassword(user))
                {
                    return Ok(new JsonResult(new { title = "Fogot password sent to email", message = "Please check your email address" }));
                }
                return BadRequest("Failed to send email.Please contact admin.");
            }
            catch(Exception) {
                return BadRequest("Failed to send email.Please contact admin.");
            }
        }

        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("This email address has not been registered yet");
            if (user.EmailConfirmed == false) return BadRequest("Please confirm your email address first.");
            try
            {
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(model.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
                var result = await _userManager.ResetPasswordAsync(user, decodedToken,model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new JsonResult(new { title = "Password reset success", message = "Your password has been reset." }));
                }
                return BadRequest("Invalid token. Please try again.");
            }
            catch (Exception)
            {
                return BadRequest("Invalid token. Please try again.");
            }

        }

        private async Task<bool> SendConfirmEmailAsync(TblUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token=WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JwtConfig:ClientUrl"]}/{_config["Email:ConfirmEmailPath"]}?token={token}&email={user.Email}";
            var body = $"<p>Hello :{user.FirstName}{user.LastName}</p>" +
                "<p>Please confirm your email address by clicking on the follwing link.</p>" +
                $"<p><a href=\"{url}\">Click here</a></p>" +
                $"<br>Thank you,{_config["Email:ApplicationName"]}";

            //var body = $" \r\n    <title>Welcome Email from TCP</title>  \r\n</head>  \r\n  \r\n<body>  \r\n    <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">  \r\n        <tr>  \r\n            <td align=\"center\" valign=\"top\" bgcolor=\"#ffe77b\" style=\"background-color:#ffe77b;\">  \r\n                <br>  \r\n                <br>  \r\n                <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">  \r\n                    <tr>  \r\n                        <td height=\"70\" align=\"left\" valign=\"middle\"></td>  \r\n                    </tr>  \r\n                    <tr>  \r\n                        <td align=\"left\" valign=\"top\"><img src=\"http://localhost:2131/Templates/EmailTemplate/images/top.png\" width=\"600\" height=\"13\" style=\"display:block;\"></td>  \r\n                    </tr>  \r\n                    <tr>  \r\n                        <td align=\"left\" valign=\"top\" bgcolor=\"#564319\" style=\"background-color:#564319; font-family:Arial, Helvetica, sans-serif; padding:10px;\">  \r\n                            <div style=\"font-size:36px; color:#ffffff;\">  \r\n                                <b>{{0}}</b>  \r\n                            </div>  \r\n                            <div style=\"font-size:13px; color:#a29881;\">  \r\n                                <b>{{1}} : ASP.NET Core Demp App</b>  \r\n                            </div>  \r\n                        </td>  \r\n                    </tr>  \r\n                    <tr>  \r\n                        <td align=\"left\" valign=\"top\" bgcolor=\"#ffffff\" style=\"background-color:#ffffff;\">  \r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">  \r\n                                <tr>  \r\n                                    <td align=\"center\" valign=\"middle\" style=\"padding:10px; color:#564319; font-size:28px; font-family:Georgia, 'Times New Roman', Times, serif;\">  \r\n                                        Congratulations! <small>You are registered.</small>  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">  \r\n                                <tr>  \r\n                                    <td width=\"40%\" align=\"center\" valign=\"middle\" style=\"padding:10px;\">  \r\n                                        <img src=\"http://localhost:2131/Templates/EmailTemplate/images/Weak_Password.gif\" width=\"169\" height=\"187\" style=\"display:block\">  \r\n                                    </td>  \r\n                                    <td align=\"left\" valign=\"middle\" style=\"color:#525252; font-family:Arial, Helvetica, sans-serif; padding:10px;\">  \r\n                                        <div style=\"font-size:16px;\">  \r\n                                            Dear {{2}},  \r\n                                        </div>  \r\n                                        <div style=\"font-size:12px;\">  \r\n                                            Thank you for showing your interest  in  our website.  \r\n                                            All you need to do is click the button below (it only takes a few seconds).  \r\n                                            You won’t be asked to log in to your account – we're simply verifying ownership of this email address.  \r\n                                            <hr>  \r\n                                            <center>  \r\n  \r\n                                                <button type=\"button\" title=\"Confirm Account Registration\" style=\"background: #1b97f1\">  \r\n                                                    <a href=\"{{6}}\" style=\"font-size:22px; padding: 10px; color: #ffffff\">  \r\n                                                        Confirm Email Now  \r\n                                                    </a>  \r\n                                                </button>  \r\n  \r\n                                            </center>  \r\n                                        </div>  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">  \r\n                                <tr>  \r\n                                    <td align=\"center\" valign=\"middle\" style=\"padding:5px;\">  \r\n                                        <img src=\"http://localhost:2131/Templates/EmailTemplate/images/divider.gif\" width=\"566\" height=\"30\">  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                            <table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-bottom:15px;\">  \r\n                                <tr>  \r\n                                    <td align=\"left\" valign=\"middle\" style=\"padding:15px; font-family:Arial, Helvetica, sans-serif;\">  \r\n                                        <div style=\"font-size:20px; color:#564319;\">  \r\n                                            <b>Please keep your credentials confidential for future use. </b>  \r\n                                        </div>  \r\n                                        <div style=\"font-size:16px; color:#525252;\">  \r\n                                            <b>Email         :</b> {{2}}  \r\n                                            <br />  \r\n                                            <b>Username :</b> {{3}}  \r\n                                            <br />  \r\n                                            <b>Password :</b> {{4}}  \r\n                                        </div>  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                            <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom:10px;\">  \r\n                                <tr>  \r\n                                    <td align=\"left\" valign=\"middle\" style=\"padding:15px; background-color:#564319; font-family:Arial, Helvetica, sans-serif;\">  \r\n                                        <div style=\"font-size:20px; color:#fff;\">  \r\n                                            <b>Update your password now.</b>  \r\n                                        </div>  \r\n                                        <div style=\"font-size:13px; color:#ffe77b;\">  \r\n                                            Weak passwords get stolen and lead to hacked accounts. Celebrate World Password Day with a new, strong password.  \r\n                                            <br>  \r\n                                            <br>  \r\n                                            <a href=\"#\" style=\"color:#ffe77b; text-decoration:underline;\">CLICK HERE</a> TO CHANGE PASSOWORD  \r\n                                        </div>  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">  \r\n                                <tr>  \r\n                                    <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"padding:10px;\">  \r\n                                        <table width=\"75%\" border=\"0\" cellspacing=\"0\" cellpadding=\"4\">  \r\n                                            <tr>  \r\n                                                <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:14px; color:#000000;\">  \r\n                                                    <b>Follow Us On</b>  \r\n                                                </td>  \r\n                                            </tr>  \r\n                                            <tr>  \r\n                                                <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:12px; color:#000000;\">  \r\n                                                    <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">  \r\n                                                        <tr>  \r\n                                                            <td width=\"33%\" align=\"left\" valign=\"middle\">  \r\n                                                                <a href=\"https://twitter.com\" title=\"Facebook\">  \r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/tweet48.png\" width=\"48\" height=\"48\">  \r\n                                                                </a>  \r\n                                                            </td>  \r\n                                                            <td width=\"34%\" align=\"left\" valign=\"middle\">  \r\n                                                                <a href=\"https://linkedin.com\" title=\"Linkedin\">  \r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/in48.png\" width=\"48\" height=\"48\">  \r\n                                                                </a>  \r\n                                                            </td>  \r\n                                                            <td width=\"33%\" align=\"left\" valign=\"middle\">  \r\n                                                                <a href=\"https://facebook.com\" title=\"Facebook\">  \r\n                                                                    <img src=\"http://localhost:2131/Templates/EmailTemplate/images/face48.png\" width=\"48\" height=\"48\">  \r\n                                                                </a>  \r\n                                                            </td>  \r\n                                                        </tr>  \r\n                                                    </table>  \r\n                                                </td>  \r\n                                            </tr>  \r\n                                        </table>  \r\n                                    </td>  \r\n                                    <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\">  \r\n                                        <b>Hours:</b> Mon-Fri 9:30-5:30, Sat. 9:30-3:00, Sun. Closed <br>  \r\n                                        <b>Customer Support:</b> <a href=\"mailto:name@yourcompanyname.com\" style=\"color:#564319; text-decoration:none;\">name@yourcompanyname.com</a><br>  \r\n                                        <br>  \r\n                                        <b>Company Address</b><br>  \r\n                                        Company URL: <a href=\"http://www.yourcompanyname.com\" target=\"_blank\" style=\"color:#564319; text-decoration:none;\">http://www.yourcompanyname.com</a>  \r\n                                    </td>  \r\n                                </tr>  \r\n                            </table>  \r\n                        </td>  \r\n                    </tr>  \r\n                    <tr>  \r\n                        <td align=\"left\" valign=\"top\"><img src=\"http://localhost:2131/Templates/EmailTemplate/images/bot.png\" width=\"600\" height=\"37\" style=\"display:block;\"></td>  \r\n                    </tr>  \r\n                </table>  \r\n                <br>  \r\n                <br>  \r\n            </td>  \r\n        </tr>  \r\n    </table>  \r\n</body>  \r\n";
            var emailSend = new EmailSendDto(user.Email, "Confirm your email", body);
            return _emailService.SendEmail(emailSend);

        }

        private async Task<bool> SendForgotPassword(TblUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var url = $"{_config["JwtConfig:ClientUrl"]}/{_config["Email:ResetPasswordPath"]}?token={token}&email={user.Email}";
            var body = $"<p>Hello :{user.FirstName}{user.LastName}</p>" +
               $"<p>In order to reset your password, please click on the following link.</p>" +
               $"<p><a href=\"{url}\">Click here</a></p>" +
               $"<br>Thank you,{_config["Email:ApplicationName"]}";
            var emailSend = new EmailSendDto(user.Email, "Reset your password", body);
            return _emailService.SendEmail(emailSend);
        }

        private async Task<bool> FacebookValidatedAsync(string accessToken,string userId)
        {
            var facebookKeys = _config["Facebook:AppID"] + "|" + _config["Facebook:AppSecret"];
            var fbResult = await _facebookHttpClient.GetFromJsonAsync<FacebookResultDto>($"debug_token?input_token={accessToken}&access_token={facebookKeys}");

            if(fbResult == null ||fbResult.Data.Is_Valid==false || !fbResult.Data.User_Id.Equals(userId))
            {
                return false;
            }
            return true;
        }

        private async Task<bool> GoogleValidatedAsync(string accessToken, string userId)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken);
            if (!payload.Audience.Equals(_config["Google:ClientId"]))
            {
                return false;
            }
            if(!payload.Issuer.Equals("accounts.goolgle.com")&& !payload.Issuer.Equals("https://accounts.google.com"))
            {
                return false;
            }
            if (payload.ExpirationTimeSeconds == null)
            {
                return false;
            }
            DateTime now = DateTime.Now.ToUniversalTime();
            DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
            if (now > expiration)
            {
                return false;
            }
            if (!payload.Subject.Equals(userId))
            {
                return false;
            }
            return true;
        }
        private async Task<TblUserDto> CreateApplicationUserDto(TblUser user)
        {
            return new TblUserDto
            {
                JWT = await _jwtService.CreateJWT(user),
                LastName = user.LastName,
                FirstName = user.FirstName,
                Id = user.Id
            };
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _userManager.Users.AnyAsync(u => u.Email == email.ToLower());
        }





        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var Users = await _superEcommereDbContext.Users.ToListAsync();
            return Ok(Users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] TblUser user)
        {
            user.Id = Guid.NewGuid().ToString();
            await tblUserRepository.CreateAsync(user);


            return Ok(user);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            var user = await _superEcommereDbContext.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> updateUser([FromRoute] Guid id, TblUser updateUser)
        {
            var user = await _superEcommereDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Id = updateUser.Id;
            //user.Username = updateUser.Username;
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            //user.Password = updateUser.Password;
            user.Email = updateUser.Email;
            user.Phone = updateUser.Phone;
            user.Address = updateUser.Address;
            user.City = updateUser.City;
            user.Region = updateUser.Region;
            user.PostalCode = updateUser.PostalCode;
            user.Country = updateUser.Country;
            user.PhoneNumber = updateUser.PhoneNumber;

            await _superEcommereDbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> deleteUser([FromRoute] Guid id)
        {
            var user = await _superEcommereDbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await tblUserRepository.RemoveAsync(user);
            return Ok();
        }


    }
}
