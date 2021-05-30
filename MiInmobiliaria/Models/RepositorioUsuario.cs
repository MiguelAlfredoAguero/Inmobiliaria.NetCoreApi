using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Create(Usuario e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO usuario ( PersonaId, activo ) " +
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
                string sql = $"DELETE FROM usuario WHERE id = @id";

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

        public int Edit(Usuario e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE usuario SET activo = @activo WHERE Id = @id";

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

        public IList<Usuario> getAll()
        {
            var res = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vUsuarios ORDER BY apellido, nombre";
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    Usuario e = null;

                    while (reader.Read())
                    {
                        e = new Usuario
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
                                Avatar = reader.GetString(12),
                            },
                            Activo = reader.GetBoolean(13)
                        };
                        res.Add(e);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public Usuario getById(int id)
        {
            Usuario e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vUsuarios WHERE Id=@id";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Usuario
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
                    }
                    connection.Close();
                }
            }
            return e;
        }

        public Usuario getByEmail(string email)
        {
            Usuario e = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vUsuarios WHERE email = @email";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@email", email);
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Usuario
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
                    }
                    connection.Close();
                }
            }
            return e;
        }
    }
}
