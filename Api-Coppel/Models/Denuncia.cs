namespace Api_Coppel.Models
{
    public class Denuncia
    {
        public Denuncia()
        {
        }
        public int folio { get; set; }
        public int empresaid { get;  set; }
        public string empresaName { get; set; }
        public int estadoid { get; set; }
        public string estadoName { get; set; }
        public string paisName { get; set; }
        public string detalle { get; set; }
        public string fecha { get; set; }
        public int centro { get; set; }
        public string contrasenia { get; set; }
        public int estatusid { get; set; }
        public string estatusInfo { get; set; }
        public int contactoid { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string comentario { get; set; }

    }
}
