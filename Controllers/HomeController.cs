using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MVC_users.Controllers
{    
    public class HomeController : Controller
    {
        public IActionResult Index(string message = "")
        {
            ViewBag.Message =message;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Login(string user, string password)
        {
            if(!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password)){

            UserrolContext db = new UserrolContext();

            var usr = db.Usuarioss.FirstOrDefault(e => e.Userid == user  && e.Pass == password);
                if(usr != null){
                    var claims = new List<Claim>  
                    {  
                        new Claim(ClaimTypes.Name, usr.Userid)  
                    };  
                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");  
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);  

                    await HttpContext.SignInAsync(principal);  
                return RedirectToAction("Index","User");
                }else{
                    return RedirectToAction("Index",new{message ="Verifica los datos"});
                }
            }else{
                return RedirectToAction("Index",new{message ="Llena los campos"});
            }           
        }
     
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();  
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
