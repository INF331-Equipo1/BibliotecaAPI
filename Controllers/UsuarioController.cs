﻿
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
    public class UsuarioController : ControllerBase
    {
        private SitioDB db;
        public UsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public Respuesta Crear(UsuarioDB usuario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario ingresado exitosamente";
                    respuesta.item = usuario.id_usuario;
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
        public async Task<Respuesta> Get(int id_usuario)
        {
            Respuesta respuesta = new Respuesta();
            UsuarioDB? usuario = await db.Usuarios.FindAsync(id_usuario);
            if (usuario == null)
            {
                usuario = new UsuarioDB();
                respuesta.codigo = 200;
                respuesta.mensaje = "Usuario no encontrado";
                respuesta.item = usuario;
                respuesta.status = true;
            }
            else
            {
                respuesta.codigo = 200;
                respuesta.mensaje = "Usuario encontrado exitosamente";
                respuesta.item = usuario;
                respuesta.status = true;
            }
            return respuesta;

        }
        [HttpGet]
        [Route("List")]
        public Respuesta Listar() // Solo 3 niveles de profundidad
        {
            Respuesta respuesta = new Respuesta();
            List<UsuarioDB> list = db.Usuarios.ToList();
            respuesta.status = true;
            respuesta.item = list;
            respuesta.mensaje = "Lista obtenida con éxito";
            respuesta.codigo = 200;
            return respuesta;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(UsuarioDB usuario)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                UsuarioDB? Original = db.Usuarios.Find(usuario.id_usuario);
                if (Original == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario no encontrado";
                    respuesta.status = false;
                    return respuesta;
                }
                else
                {
                    Original.nombre = usuario.nombre;
                    Original.correo = usuario.correo;
                    Original.password = usuario.password;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Usuario editado exitosamente";
                        respuesta.status = true;
                    }
                    else
                    {
                        respuesta.codigo = 200;
                        respuesta.mensaje = "Error al editar el usuario";
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
                UsuarioDB? Eliminado = db.Usuarios.Find(id_Rol);
                if (Eliminado == null)
                {
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario no encontrado";
                    respuesta.status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.codigo = 200;
                    respuesta.mensaje = "Usuario eliminado exitosamente";
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