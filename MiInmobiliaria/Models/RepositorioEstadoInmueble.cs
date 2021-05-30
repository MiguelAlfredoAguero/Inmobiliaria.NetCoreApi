using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioEstadoInmueble : RepositorioBase
    {
        public RepositorioEstadoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public List<EstadoInmueble> Listar()
        {
            var res = new List<EstadoInmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(EstadoInmueble.Id)}, {nameof(EstadoInmueble.Nombre)} FROM {nameof(EstadoInmueble)}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var t = new EstadoInmueble
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                        res.Add(t);
                    }

                    con.Close();
                }
            }
            return res;
        }



        // TUTORIAL
        // https://www.youtube.com/watch?v=tiG71g9YnMw

        /// <summary>
        /// Retorna una List<SelectListItem> para poder llenar un DropDownList
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListarSelectListItem()
        {
            List<EstadoInmueble> lst = Listar();

            List<SelectListItem> items = lst.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.Id.ToString()
                };
            });

            return items;
        }

        public EstadoInmueble Obtener(int id)
        {
            EstadoInmueble usoInmueble = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(EstadoInmueble.Id)}, {nameof(EstadoInmueble.Nombre)} " +
                    $"FROM {nameof(EstadoInmueble)} " +
                    $"WHERE id={@id}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        usoInmueble = new EstadoInmueble
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }

                    con.Close();
                }
            }

            return usoInmueble;
        }

        public int Create(EstadoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(EstadoInmueble)} ({nameof(EstadoInmueble.Nombre)}) " +
                    $"VALUES (@nombre);" +
                    $"SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Editar(EstadoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(EstadoInmueble)} SET {nameof(EstadoInmueble.Nombre)} = @nombre " +
                    $"WHERE {nameof(EstadoInmueble.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public int Delete(int id)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM {nameof(EstadoInmueble)} WHERE {nameof(EstadoInmueble.Id)} = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return res;
        }

    }
}
