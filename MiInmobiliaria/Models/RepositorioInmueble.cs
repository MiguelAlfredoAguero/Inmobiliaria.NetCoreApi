using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {
        }

        public int Create(Inmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO inmueble ( direccion, ambientes, precio, " +
                    $"UsoInmuebleId, TipoInmuebleId, disponible, PropietarioId, AgenciaId, avatar ) " +
                    $"VALUES ( @direccion, @ambientes, @precio, @UsoInmuebleId, @TipoInmuebleId, @disponible, " +
                    $"@PropietarioId, @AgenciaId, @avatar); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@direccion", e.Direccion);
                    cmd.Parameters.AddWithValue("@ambientes", e.Ambientes);
                    cmd.Parameters.AddWithValue("@precio", e.Precio);
                    cmd.Parameters.AddWithValue("@UsoInmuebleId", e.UsoInmuebleId);
                    cmd.Parameters.AddWithValue("@TipoInmuebleId", e.TipoInmuebleId);
                    cmd.Parameters.AddWithValue("@disponible", e.Disponible);
                    cmd.Parameters.AddWithValue("@PropietarioId", e.PropietarioId);
                    cmd.Parameters.AddWithValue("@AgenciaId", e.AgenciaId);
                    cmd.Parameters.AddWithValue("@avatar", e.Avatar);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }
                
        public int Edit(Inmueble e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmueble SET " +
                        $"direccion = @direccion, " +
                        $"ambientes = @ambientes, " +
                        $"precio = @precio, " +
                        $"UsoInmuebleId = @UsoInmuebleId, " +
                        $"TipoInmuebleId = @TipoInmuebleId, " +
                        $"disponible = @disponible, " +
                        $"PropietarioId = @PropietarioId, " +
                        $"AgenciaId = @AgenciaId, " +
                        $"avatar =  @avatar " +
                    $"WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@direccion", e.Direccion);
                    cmd.Parameters.AddWithValue("@ambientes", e.Ambientes);
                    cmd.Parameters.AddWithValue("@precio", e.Precio);
                    cmd.Parameters.AddWithValue("@UsoInmuebleId", e.UsoInmuebleId);
                    cmd.Parameters.AddWithValue("@TipoInmuebleId", e.TipoInmuebleId);
                    cmd.Parameters.AddWithValue("@disponible", e.Disponible);
                    cmd.Parameters.AddWithValue("@PropietarioId", e.PropietarioId);
                    cmd.Parameters.AddWithValue("@AgenciaId", e.AgenciaId);
                    cmd.Parameters.AddWithValue("@avatar", e.Avatar);
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
                string sql = $"DELETE FROM Inmueble WHERE Id = @id";
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

        public IList<Inmueble> getAll()
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }
        public IList<Inmueble> getAll(Propietario p)
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles " +
                    $"WHERE (PropietarioId = @PropietarioId OR @PropietarioId = 0 ) ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@PropietarioId", p.Id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> getAll(Agencia a)
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles WHERE (AgenciaId = @AgenciaId OR @AgenciaId = 0 ) ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@AgenciaId", a.Id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> getAllDisponibles()
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles WHERE disponible = 1 ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public Inmueble getById(int id)
        {
            Inmueble e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }


        public IList<Inmueble> getDesocupados(DateTime desde, DateTime hasta)
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles " +
                    $"WHERE disponible = 1 AND Id IN " +
                    $"( SELECT InmuebleId FROM Contrato " +
                    $"      WHERE desde > @hasta OR  hasta < @desde ) " +
                    $"UNION " +
                    $"SELECT* FROM vInmuebles " +
                    $"  WHERE disponible = 1 AND Id NOT IN " +
                    $"      (SELECT InmuebleId FROM Contrato)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@desde", desde);
                    cmd.Parameters.AddWithValue("@hasta", hasta);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }
        public Inmueble getDesocupado(DateTime desde, DateTime hasta, int InmuebleId)
        {
            Inmueble e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles " +
                    $"WHERE Id = @InmuebleId AND disponible = 1 AND Id IN " +
                    $"( SELECT InmuebleId FROM Contrato " +
                    $"      WHERE (desde > @hasta OR  hasta < @desde) AND InmuebleId = @InmuebleId )";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@InmuebleId", InmuebleId);
                    cmd.Parameters.AddWithValue("@desde", desde);
                    cmd.Parameters.AddWithValue("@hasta", hasta);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }


        public IList<Inmueble> Busqueda(int UsoInmuebleId, int TipoInmuebleId, int ambientes, DateTime desde, DateTime hasta, decimal minimo, decimal maximo)
        {
            var res = new List<Inmueble>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vInmuebles " +
                    $"WHERE disponible = 1 " +
                    $"  AND ( UsoInmuebleId = @UsoInmuebleId OR @UsoInmuebleId = 0 ) " +
                    $"  AND ( TipoInmuebleId = @TipoInmuebleId OR @TipoInmuebleId = 0 ) " +
                    $"  AND ( ambientes = @ambientes OR @ambientes = 0 ) " +
                    $"  AND precio BETWEEN @minimo AND @maximo " +
                    $"  AND Id IN " +
                    $"      ( SELECT InmuebleId FROM Contrato " +
                    $"              WHERE desde > @hasta OR  hasta < @desde ) " +
                    $"UNION " +
                    $"SELECT* FROM vInmuebles " +
                    $"WHERE disponible = 1 " +
                    $"  AND ( UsoInmuebleId = @UsoInmuebleId OR @UsoInmuebleId = 0 ) " +
                    $"  AND ( TipoInmuebleId = @TipoInmuebleId OR @TipoInmuebleId = 0 ) " +
                    $"  AND ( ambientes = @ambientes OR @ambientes = 0 ) " +
                    $"  AND precio BETWEEN @minimo AND @maximo " +
                    $"  AND Id NOT IN " +
                    $"      (SELECT InmuebleId FROM Contrato)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UsoInmuebleId", UsoInmuebleId);
                    cmd.Parameters.AddWithValue("@TipoInmuebleId", TipoInmuebleId);
                    cmd.Parameters.AddWithValue("@ambientes", ambientes);
                    cmd.Parameters.AddWithValue("@desde", desde);
                    cmd.Parameters.AddWithValue("@hasta", hasta);
                    cmd.Parameters.AddWithValue("@minimo", minimo);
                    cmd.Parameters.AddWithValue("@maximo", maximo);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Inmueble e = null;
                    while (reader.Read())
                    {
                        e = new Inmueble()
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Precio = reader.GetDecimal(3),
                            UsoInmuebleId = reader.GetInt32(4),
                            UsoInmueble = new UsoInmueble
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(5)
                            },
                            TipoInmuebleId = reader.GetInt32(6),
                            TipoInmueble = new TipoInmueble
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                            },
                            Disponible = reader.GetBoolean(8),
                            PropietarioId = reader.GetInt32(9),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(12),
                                        Nombre = reader.GetString(13)
                                    }
                                },
                                Activo = reader.GetBoolean(14)
                            },
                            AgenciaId = reader.GetInt32(15),
                            Agencia = new Agencia
                            {
                                Id = reader.GetInt32(15),
                                Persona = new Persona
                                {
                                    Apellido = reader.GetString(16),
                                    Nombre = reader.GetString(17),
                                    TipoPersona = new TipoPersona
                                    {
                                        Id = reader.GetInt32(18),
                                        Nombre = reader.GetString(19)
                                    }
                                },
                                Activo = reader.GetBoolean(20)
                            },
                            Avatar = reader.GetString(21)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }

    }
}
