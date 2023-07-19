using Api_Coppel.Models;
using Api_Coppel.Repository;

namespace Api_Coppel.Bussines
{
    public class UsuarioBussines
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        public string validarUsuarioAdmin(Usuario user)
        {
            return usuarioRepository.ValidarLoginAdmin(user);
        }
        public bool salirAdmin(Usuario user)
        {
            return usuarioRepository.cerrarSecion(user);

        }
    
    }
}
