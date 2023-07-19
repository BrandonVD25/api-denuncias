using Api_Coppel.Bussines;
using Api_Coppel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api_Coppel.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DenunciaController : ControllerBase
    {
        DenunciaBussines denunciaBussines= new DenunciaBussines();
        public DenunciaController() { 
        }

        [HttpGet]
        [Route("listaDenuncias")]
        public IActionResult ObtenerInformesDenuncia () {
            try
            {
                List<Denuncia> list = denunciaBussines.obtenerInformesDenuncias();
                return StatusCode(StatusCodes.Status200OK,  list );

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }
        }
        [HttpPost]
        [Route("validarDenuncia")]
        public IActionResult validarDeuncia (Denuncia denuncia)
        {
            try
            {
                string respuesta = denunciaBussines.validarFolio(denuncia);
                return StatusCode(StatusCodes.Status200OK, new {response=respuesta});
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }

        }
        [HttpPost]
        [Route("obtenerDenunciaByFolio")]
        public IActionResult obtenerDenunciaByFolio(Denuncia denuncia)
        {
            try
            {
                List<Denuncia> list = denunciaBussines.MostrarDenunciaPorfolioDenunciante(denuncia);
                return StatusCode(StatusCodes.Status200OK, list);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..."  });
            }

        }
        [HttpPost]
        [Route("HistorialComentarios")]
        public IActionResult HistorialComentarios(Comentario comentario)
        {
            try
            {
                List<Comentario> list = denunciaBussines.HistorialComentarios(comentario);
                return StatusCode(StatusCodes.Status200OK, list);
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje="algo salio mal..." });
            }

        }
        [HttpPost]
        [Route("agregarDenuncia")]
        public IActionResult agregarDenuncia(Denuncia denuncia)
        {
            try
            {
                string respuesta = denunciaBussines.agregarDenuncia(denuncia);
                return StatusCode(StatusCodes.Status200OK,new {response= respuesta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }
        }
        [HttpPost]
        [Route("ActualizarEstatusYAgregarComentario")]
        public IActionResult agregarComentarioActulizar(Denuncia denuncia)
        {
            try
            {
                denunciaBussines.ActualizarEstatusYAgregarComentario(denuncia);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "algo salio mal..." });
            }
        }

    }
}
