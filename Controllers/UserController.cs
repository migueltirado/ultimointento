using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_users.Models;
using Microsoft.AspNetCore.Authorization;
using System.Dynamic;

namespace MVC_users.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserrolContext _context;

        public UserController(UserrolContext context)
        {
            _context = context;
        }
        
        // GET: Usuario
        public async Task <IActionResult> Index()
        { 
            return View( await _context.Usuariosr.FromSql("select usr.id, usr.userid,usr.nombre,usr.pass,rol.nombrerol from usuarios as usr inner join usuario_rol as u_rol on usr.id = u_rol.idusuario inner join roles as rol on rol.idroles = u_rol.idrol;").ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarioss
                .SingleOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }
            return View(usuarios);
        }

        // GET: Usuario/Create
        public IActionResult Create(Roles roles)
        {
          
         if(roles.idroles == 0)
            {
                ModelState.AddModelError("","Selecciona un rol");
            }
            
            int Sel = roles.idroles;
            ViewBag.SelectedValue = roles.idroles;
            
            List<Roles> rol = new List<Models.Roles>();
            rol = (from Roles in _context.Roless
            select Roles).ToList();

            rol.Insert(0,new Roles{idroles = 0, nombrerol = "Selecciona "});
           ViewBag.ListofRoles = rol;
            return View();
        }
 
    
        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Save(string userid, string nombreuser,string passuser,int idr)
        {                
            using(var r = new UserrolContext())
            { 
                Console.WriteLine(userid);
                Console.WriteLine(nombreuser);
                Console.WriteLine(passuser);
                Console.WriteLine(idr);

                r.Add(new Usuarios 
                {  Nombre = nombreuser, 
                Userid = userid, 
                Pass = passuser 
                });

                r.SaveChanges ();
                await r.SaveChangesAsync();

                var usuario = await  r.Usuarioss.SingleAsync (u => u.Userid == userid);
                r.Add (new UsuarioRol { 
                    idusuario = usuario.Id, 
                    idrol = idr 
                    });

                r.SaveChanges ();
                await r.SaveChangesAsync();
              return RedirectToAction("Index");
            }
        } 

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(Roles roles, int? id)
        {
            if(roles.idroles == 0)
            {
                ModelState.AddModelError("","Selecciona un rol");
            }
            
            int Sel = roles.idroles;
            ViewBag.SelectedValue = roles.idroles;
            
            List<Roles> rol = new List<Models.Roles>();
            rol = (from Roles in _context.Roless
            select Roles).ToList();

            rol.Insert(0,new Roles{idroles = 0, nombrerol = "Selecciona "});
            ViewBag.ListofRoles = rol;
          
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarioss.SingleOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }
            return View(usuarios);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(int id,string userid, string nombreuser,string passuser,int idr)
        { 
            using(var r = new UserrolContext())
            {  
                var r2  = r.Usuarioss.Single(n => n.Id == id);
                r2.Userid = userid;
                r2.Nombre = nombreuser;
                r2.Pass = passuser;

                r.SaveChanges ();
                await r.SaveChangesAsync();

                var usuario = await  r.Usuarioss.SingleAsync (u => u.Userid == userid);
                var roll  = r.UsuarioRoles.Single(n => n.idur == id);
           
                roll.idusuario = usuario.Id;
                roll.idrol = idr;                  

                r.SaveChanges ();
                await r.SaveChangesAsync();

                return RedirectToAction("Index");
                } 
        } 
        
        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarioss
                .SingleOrDefaultAsync(m => m.Id == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuarios = await _context.Usuarioss.SingleOrDefaultAsync(m => m.Id == id);
            _context.Usuarioss.Remove(usuarios);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 
        private bool EmployeesExists(int id)
        {
            return _context.Usuarioss.Any(e => e.Id ==id);
        }
    }
}
