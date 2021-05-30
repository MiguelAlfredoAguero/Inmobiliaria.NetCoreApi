using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioTipoPersona : RepositorioBase , IRepositorioTipoPersona
    {
        public RepositorioTipoPersona(IConfiguration configuration) : base(configuration)
        {
        }

        public IList<TipoPersona> getAll() {
            var res = new List<TipoPersona>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, nombre FROM TipoPersona";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        var t = new TipoPersona
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
        //public List<SelectListItem> ListarSelectListItem()
        //{
        //    List<TipoPersona> lst = Listar();

        //    List<SelectListItem> items = lst.ConvertAll(d =>
        //    {
        //        return new SelectListItem()
        //        {
        //            Text = d.Nombre.ToString(),
        //            Value = d.Id.ToString()
        //        };
        //    });

        //    return items;
        //}

        public TipoPersona getById(int id)
        {
            TipoPersona tipoPersona = null;

            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(TipoPersona.Id)}, {nameof(TipoPersona.Nombre)} " +
                    $"FROM {nameof(TipoPersona)} " +
                    $"WHERE id={@id}";
                using(SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        tipoPersona = new TipoPersona
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        };
                    }

                    con.Close();
                }
            }

            return tipoPersona;
        }

        public int Create(TipoPersona e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(TipoPersona)} ({nameof(TipoPersona.Nombre)}) " +
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

        public int Edit(TipoPersona e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(TipoPersona)} SET {nameof(TipoPersona.Nombre)} = @nombre " +
                    $"WHERE {nameof(TipoPersona.Id)} = @id";
                using(SqlCommand cmd = new SqlCommand(sql, con))
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
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM {nameof(TipoPersona)} WHERE {nameof(TipoPersona.Id)} = @id";
                using(SqlCommand cmd = new SqlCommand(sql, con))
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
