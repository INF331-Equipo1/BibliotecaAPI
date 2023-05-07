using API.DataBase;
using API.Models.Usuario;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models.UtilidadGeneral;
using API.BLL;
using API.DAL;
using Microsoft.Extensions.WebEncoders.Testing;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolUsuarioController : ControllerBase
    {
        private SitioDB db;
        public RolUsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        
        [HttpGet]
        [Route("Listar_PermisosByRol")]
        public Rol Listar_PermisosByRol(int id_rol)
        {
            RolDB rol = db.Rol.Find(id_rol);
            List<PermisoAcceso> permisos = new List<PermisoAcceso>();
            List<PermisoAccesoDB> list = db.PermisosAcceso.ToList();
            List<ItemMenuDB> items = db.ItemsMenu.ToList();
            Rol r = new Rol();
            foreach (ItemMenuDB item in items)
            {
                PermisoAcceso pa = new PermisoAcceso();
                pa.id_itemPadre = item.id_itemPadre;
                pa.id_itemMenu = item.id_itemMenu;
                pa.nombre = item.titulo;
                pa.permiso = list.Where(i => i.id_itemMenu == item.id_itemMenu && i.id_rol == id_rol).ToList().Count == 1;
                permisos.Add(pa);
            }
            r.rolUsuario = rol;
            r.permisos = permisos;
            return r;
        }
        
    }
}
