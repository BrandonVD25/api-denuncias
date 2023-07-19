using Api_Coppel.Models;
using System.Data.SqlClient;
using System.Data;

namespace Api_Coppel.Repository
{
    public class UsuarioRepository
    {

        ConexionSql conexionSql = new ConexionSql();
        public string ValidarLoginAdmin(Usuario user)
        {
            string cadenasql = conexionSql.GetConnectionString();
            string result = "";

            try
            {
                using (var conexion = new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("ValidarLoginUsuario", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo", user.correo);
                    cmd.Parameters.AddWithValue("@contrasenia", user.contrasenia);

                    SqlParameter outputParameter = new SqlParameter("@respuesta", SqlDbType.NVarChar, -1);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);
                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["@respuesta"].Value.ToString();
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public bool cerrarSecion(Usuario user)
        {
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using(var conexion = new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("userSalir",conexion);
                    cmd.CommandType=System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@correo",user.correo);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }

        }

    }
}
