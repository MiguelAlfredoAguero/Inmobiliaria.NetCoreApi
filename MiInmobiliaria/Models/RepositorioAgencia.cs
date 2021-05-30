using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioAgencia : RepositorioBase, IRepositorioAgencia
    {
        public RepositorioAgencia(IConfiguration configuration) : base(configuration)
        {
        }


        public int Create(Agencia e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO agencia ( PersonaId, activo ) " +
                    $"VALUES ( @PersonaId, @activo); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    cmd.Parameters.AddWithValue("@PersonaId", e.Persona.Id);
                    cmd.Parameters.AddWithValue("@activo", e.Activo);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
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
                string sql = $"DELETE FROM agencia WHERE id = @id";

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

        public int Edit(Agencia e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE agencia SET activo = @activo WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@activo", e.Activo);
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public IList<Agencia> getAll()
        {
            var res = new List<Agencia>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vAgencias ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Agencia agencia = null;

                    while (reader.Read())
                    {
                        agencia = new Agencia
                        {
                            Id = reader.GetInt32(0),
                            PersonaId = reader.GetInt32(1),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(2),
                                Nombre = reader.GetString(3),
                                FechaNac = reader.GetDateTime(4),
                                Dni = reader.GetString(5),
                                TipoPersona = new TipoPersona
                                {
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(7)
                                },
                                Telefono = reader.GetString(8),
                                Email = reader.GetString(9),
                                Password = reader.GetString(10),
                                Avatar = reader.GetString(11)
                            },
                            Activo = reader.GetBoolean(12)
                        };
                        res.Add(agencia);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public Agencia getById(int id)
        {
            Agencia agencia = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vAgencias WHERE id = @id ORDER BY apellido, nombre ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        agencia = new Agencia
                        {
                            Id = reader.GetInt32(0),
                            PersonaId = reader.GetInt32(1),
                            Persona = new Persona()
                            {
                                Id = reader.GetInt32(1),
                                Apellido = reader.GetString(2),
                                Nombre = reader.GetString(3),
                                FechaNac = reader.GetDateTime(4),
                                Dni = reader.GetString(5),
                                TipoPersona = new TipoPersona
                                {
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(7)
                                },
                                Telefono = reader.GetString(8),
                                Email = reader.GetString(9),
                                Password = reader.GetString(10),
                                Avatar = reader.GetString(11)
                            },
                            Activo = reader.GetBoolean(12)
                        };
                    }
                    con.Close();
                }
            }
            return agencia;
        }
    }
}
