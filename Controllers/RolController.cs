
using API.DataBase;
using API.Models;
using API.Models.Biblioteca;
using API.Models.Usuario;
using API.Models.UtilidadGeneral;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private SitioDB db;
        public RolController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public Respuesta Crear(RolDB rol)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Rol.Add(rol);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol ingresado exitosamente";
                    respuesta.item = rol.id_rol;
                    respuesta.status = true;
                }
                else
                {
                    respuesta.codigo = 400;
                    respuesta.mensaje = "Modelo Inválido";
                    respuesta.status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<Respuesta> Get(int id_Rol)
        {
            Respuesta respuesta = new Respuesta();
            RolDB? rol = await db.Rol.FindAsync(id_Rol);
            if (rol == null)
            {
                rol = new RolDB();
                respuesta.codigo = 200;
                respuesta.mensaje = "Rol no encontrado";
                respuesta.item = rol;
                respuesta.status = true;
            }
            else
            {
                respuesta.codigo = 200;
                respuesta.mensaje = "Rol encontrado exitosamente";
                respuesta.item = rol;
                respuesta.status = true;
            }
            return respuesta;

        }
        [HttpGet]
        [Route("List")]
        public Respuesta Listar() // Solo 3 niveles de profundidad
        {
            Respuesta respuesta = new Respuesta();
            List<RolDB> list = db.Rol.ToList();
            respuesta.status = true;
            respuesta.item = list;
            respuesta.mensaje = "Lista obtenida con éxito";
            respuesta.codigo = 200;
            return respuesta;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(RolDB rol)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB? Original = db.Rol.Find(rol.id_rol);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.nombre = rol.nombre;
                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Rol editado exitosamente";
                        respuesta.status = true;
                    }
                    else
                    {
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Error al editar el producto";
                        respuesta.status = false;
                        return respuesta;
                    }

                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;

        }
        [HttpDelete]
        [Route("Delete")]
        public Respuesta Eliminar(int id_Rol)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB? Eliminado = db.Rol.Find(id_Rol);
                if (Eliminado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Rol eliminado exitosamente";
                    respuesta.status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.codigo = 500;
                respuesta.mensaje = "Error: " + ex.Message;
                respuesta.status = false;
            }
            return respuesta;
        }
    }
}