using Api_Coppel.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api_Coppel.Repository
{
    public class DenunciaRepository
    {
        ConexionSql conexionSql= new ConexionSql();
        public List<Denuncia> obtenerInformesDenuncias()
        {
            string cadenasql = conexionSql.GetConnectionString();
            List<Denuncia> list = new List<Denuncia>();

            try
            {
                using(var conexion= new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("ObtenerInformesDenuncias",conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using(var reader= cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            list.Add(new Denuncia {
                                folio = Convert.ToInt32(reader["Folio"]),
                                empresaName =Convert.ToString( reader["NombreEmpresa"]),
                                paisName= Convert.ToString(reader["Pais"]),
                                estadoName= Convert.ToString(reader["Estado"]),
                                centro= Convert.ToInt32(reader["NumCentro"]),
                                fecha= Convert.ToString(reader["Fecha"]),
                                estatusInfo= Convert.ToString(reader["InfoEstatus"])
                                });
                        }
                    }

                }
                return list;
            }catch(Exception ex)
            {
                throw ex;
            }
          
        }
    public string ValidarLoginDenunciante(Denuncia denuncia) {
            string cadenasql = conexionSql.GetConnectionString();
            string result = "";

            try
            {
                using(var conexion= new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("ValidarLoginDenunciante",conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folio", denuncia.folio);
                    cmd.Parameters.AddWithValue("@contrasenia", denuncia.contrasenia);

                    SqlParameter outputParameter = new SqlParameter("@result", SqlDbType.NVarChar, -1);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);
                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["@result"].Value.ToString();
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
    public List<Denuncia> MostrarDenunciaPorfolioDenunciante(Denuncia denuncia)
        {
            List<Denuncia> list = new List<Denuncia>();
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using(var conexion= new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("MostrarDenunciaPorfolioDenunciante", conexion); 
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folio", denuncia.folio);

                    using(var reader= cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(
                                new Denuncia
                                {
                                    folio = Convert.ToInt32(reader["Folio"]),
                                    estatusInfo = Convert.ToString(reader["Estatus"]),
                                    detalle = Convert.ToString(reader["detalle"]),
                                    estadoName= Convert.ToString(reader["estado"]),
                                    empresaName= Convert.ToString(reader["empresa"])
                                }
                                );;
                            

                        }
                    }

                }
                return list;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
    public List<Comentario> HistorialComentarios(Comentario comentario)
    {
            List<Comentario> comentarios = new List<Comentario>();
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using (var conexion = new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("MostrarHistorialComentarios", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folio",comentario.folio);
                    using (var reader= cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comentarios.Add(new Comentario
                            {
                                comentario =Convert.ToString( reader["Comentario"]),
                                fecha= Convert.ToString(reader["Fecha"])

                            }) ;
                        }
                    }

                }
                return comentarios;
            }catch(Exception ex)
            {
                throw ex;
            }

        }
    public string AgregarDenuncia(Denuncia denuncia)
        {
            string result = "";
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using(var conexion =new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("AgregarInformeDenuncias", conexion);
                    cmd.CommandType= System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@empresaid",denuncia.empresaid);
                    cmd.Parameters.AddWithValue("@estadoid", denuncia.estadoid);
                    cmd.Parameters.AddWithValue("@detalle", denuncia.detalle);
                    cmd.Parameters.AddWithValue("@fecha",denuncia.fecha);
                    cmd.Parameters.AddWithValue("@centro", denuncia.centro);
                    cmd.Parameters.AddWithValue("@contrasenia", denuncia.contrasenia);


                    SqlParameter outputParameter = new SqlParameter("@folio", SqlDbType.NVarChar, -1);
                    outputParameter.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParameter);
                    if (!string.IsNullOrEmpty(denuncia.nombre) && !string.IsNullOrEmpty(denuncia.correo) && !string.IsNullOrEmpty(denuncia.telefono))
                    {
                        cmd.Parameters.AddWithValue("@nombre", denuncia.nombre);
                        cmd.Parameters.AddWithValue("@correo", denuncia.correo);
                        cmd.Parameters.AddWithValue("@telefono", denuncia.telefono);
                    }
                    cmd.ExecuteNonQuery();

                    result = cmd.Parameters["@folio"].Value.ToString();

                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        public List<Denuncia> obtenerDenunciaAdmin(int folio)
        {
            List<Denuncia> list = new List<Denuncia>();
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using (var conexion = new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("verDetalleDenuncia", conexion);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folio", folio);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(
                                new Denuncia
                                {
                                    folio = Convert.ToInt32(reader["Folio"]),
                                    empresaName = Convert.ToString(reader["Info"]),
                                    estatusInfo = Convert.ToString(reader["Estatus"]),
                                    detalle = Convert.ToString(reader["Detalle"]),
                                    paisName= Convert.ToString(reader["Pais"]),
                                    centro= Convert.ToInt32(reader["NumC"]),
                                    estadoName = Convert.ToString(reader["Estado"])
                                    
                                }
                                ); ;


                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool agregarComentario(Comentario comentario)
        {
            string cadenasql = conexionSql.GetConnectionString();
            using (var conexion = new SqlConnection(cadenasql))
            {
                conexion.Open();
                var cmd = new SqlCommand("agregarComentario", conexion); 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@folio", comentario.folio);
                cmd.Parameters.AddWithValue("@comentario", comentario.comentario);
                cmd.ExecuteNonQuery();

                return true;
            }
        }

        public bool actualizarEstatus(Comentario comentario)
        {
            string cadenasql = conexionSql.GetConnectionString();
            try
            {
                using (var conexion = new SqlConnection(cadenasql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("CambiarEstatus", conexion); 
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@folio", comentario.folio);
                    cmd.Parameters.AddWithValue("@comentario", comentario.comentario);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ActualizarEstatusYAgregarComentario(Denuncia denuncia)
        {
            string cadenasql = conexionSql.GetConnectionString();
            using (SqlConnection connection = new SqlConnection(cadenasql))
            {

                using (SqlCommand command = new SqlCommand("ActualizarEstatusYAgregarComentario", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    command.Parameters.AddWithValue("@folio", denuncia.folio);
                    command.Parameters.AddWithValue("@nuevoEstatusID", denuncia.estatusid);
                    command.Parameters.AddWithValue("@nuevoComentario",denuncia.comentario);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }
        }
    }
        
    
}
