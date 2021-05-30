using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Create(Propietario e)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Propietario ( PersonaId, Activo ) " +
                    $"VALUES ( @PersonaId, @activo); " +
                    $"SELECT SCOPE_IDENTITY()";
                using(SqlCommand cmd = new SqlCommand(sql, con))
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

        public int Edit(Propietario e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Propietario SET Activo = @activo WHERE Id = @id";
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

        public int Delete(int id)
        {
            int res = -1;
            using(SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Propietario WHERE Id = @id";

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

        public IList<Propietario> getAll()
        {
            var res = new List<Propietario>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPropietarios ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Propietario e = null;

                    while (reader.Read())
                    {
                        e = new Propietario
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
                                Rol = reader.GetInt32(11),
                                Avatar = reader.GetString(12)
                            },
                            Activo = reader.GetBoolean(13)
                        };
                        res.Add(e);
                    }

                    con.Close();
                }
            }
            return res;
        }
        public IList<Propietario> getAll(int id)
        {
            var res = new List<Propietario>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPropietarios WHERE Id = @id ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Propietario e = null;

                    while (reader.Read())
                    {
                        e = new Propietario
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
                                Rol = reader.GetInt32(11),
                                Avatar = reader.GetString(12)
                            },
                            Activo = reader.GetBoolean(13)
                        };
                        res.Add(e);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public Propietario getById(int id)
        {
            Propietario e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPropietarios WHERE id = @id ORDER BY apellido, nombre ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        e = new Propietario
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
                                TipoPersonaId = reader.GetInt32(6),
                                TipoPersona = new TipoPersona
                                {
                                    Id = reader.GetInt32(6),
                                    Nombre = reader.GetString(7)
                                },
                                Telefono = reader.GetString(8),
                                Email = reader.GetString(9),
                                Password = reader.GetString(10),
                                Rol = reader.GetInt32(11),
                                Avatar = reader.GetString(12)
                            },
                            Activo = reader.GetBoolean(13)
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }
    }
}
