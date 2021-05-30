using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioUsoInmueble : RepositorioBase, IRepositorioUsoInmueble
    {
        public RepositorioUsoInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public IDictionary<int, string> getIDictionary()
        {
            SortedDictionary<int, string> dicc = new SortedDictionary<int, string>();
            foreach (var fila in getAll())
            {
                dicc.Add((int)fila.Id, (string)fila.Nombre);
            }
            return dicc;
        }


        public int Create(UsoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO {nameof(UsoInmueble)} ({nameof(UsoInmueble.Nombre)}) " +
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

        public int Edit(UsoInmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE {nameof(UsoInmueble)} SET {nameof(UsoInmueble.Nombre)} = @nombre " +
                    $"WHERE {nameof(UsoInmueble.Id)} = @id";
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
                string sql = $"DELETE FROM {nameof(UsoInmueble)} WHERE {nameof(UsoInmueble.Id)} = @id";
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

        public IList<UsoInmueble> getAll()
        {
            var res = new List<UsoInmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Nombre)} FROM {nameof(UsoInmueble)}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var t = new UsoInmueble
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

        public UsoInmueble getById(int id)
        {
            UsoInmueble usoInmueble = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Nombre)} " +
                    $"FROM {nameof(UsoInmueble)} " +
                    $"WHERE id={@id}";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        usoInmueble = new UsoInmueble
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
    }
}
