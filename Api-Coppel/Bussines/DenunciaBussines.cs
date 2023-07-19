using Api_Coppel.Models;
using Api_Coppel.Repository;

namespace Api_Coppel.Bussines
{
    public class DenunciaBussines
    {
        DenunciaRepository denunciaRepository = new DenunciaRepository();
        public DenunciaBussines() { }

        public List<Denuncia> obtenerInformesDenuncias()
        {
            return this.denunciaRepository.obtenerInformesDenuncias();
        }
        public string validarFolio(Denuncia denuncia)
        {
            return this.denunciaRepository.ValidarLoginDenunciante(denuncia);
        }
        public List<Denuncia> MostrarDenunciaPorfolioDenunciante(Denuncia denuncia)
        {
            return this.denunciaRepository.MostrarDenunciaPorfolioDenunciante(denuncia);
        }
        public List<Comentario> HistorialComentarios(Comentario comentario)
        {
            return this.denunciaRepository.HistorialComentarios(comentario);
        }
        public string agregarDenuncia(Denuncia denuncia)
        {
            return this.denunciaRepository.AgregarDenuncia(denuncia);
        }
        public List <Denuncia> denunciaAdmin(int folio)
        {
            return this.denunciaRepository.obtenerDenunciaAdmin(folio);

        }
        public bool actualizarEstatus(Denuncia denuncia)
        {
            return this.actualizarEstatus(denuncia);
        }
        public void ActualizarEstatusYAgregarComentario(Denuncia denuncia)
        {
           this.denunciaRepository.ActualizarEstatusYAgregarComentario(denuncia);
        }

    }
}
