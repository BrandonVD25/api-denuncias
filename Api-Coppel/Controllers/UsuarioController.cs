using Api_Coppel.Bussines;
using Api_Coppel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Coppel.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        UsuarioBussines usuarioBussines = new UsuarioBussines();
        DenunciaBussines denunciaBussines = new DenunciaBussines();
        public UsuarioController()
        {

        }
        [HttpPost]
        [Route("validarLoginAdmin")]
        public IActionResult validarDeuncia(Usuario user)
        {
            try
            {
                string respuesta = usuarioBussines.validarUsuarioAdmin(user);
                return StatusCode(StatusCodes.Status200OK, new { response = respuesta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }

        }
        [HttpPost]
        [Route("salirAdmin")]
        public IActionResult salirAdmin(Usuario usuario)
        {
            try
            {
                bool respuesta = usuarioBussines.salirAdmin(usuario);
                return StatusCode(StatusCodes.Status200OK, new { response = "ok" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }
        }
      
    }
}
