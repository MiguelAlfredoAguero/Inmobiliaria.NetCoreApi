using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {
        }

        public int Create(Contrato e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO contrato (InmuebleId, InquilinoId, GaranteId, " +
                    $"desde, hasta, precio) " +
                    $"VALUES ( @InmuebleId, @InquilinoId, @GaranteId, @desde, @hasta, @precio); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@InmuebleId", e.InmuebleId);
                    cmd.Parameters.AddWithValue("@InquilinoId", e.InquilinoId);
                    cmd.Parameters.AddWithValue("@GaranteId", e.GaranteId);
                    cmd.Parameters.AddWithValue("@desde", e.Desde);
                    cmd.Parameters.AddWithValue("@hasta", e.Hasta);
                    cmd.Parameters.AddWithValue("@precio", e.Precio);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Renovar(Contrato e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO contrato (InmuebleId, InquilinoId, GaranteId, " +
                    $"desde, hasta, precio) " +
                    $"VALUES ( @InmuebleId, @InquilinoId, @GaranteId, @desde, @hasta, @precio); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@InmuebleId", e.InmuebleId);
                    cmd.Parameters.AddWithValue("@InquilinoId", e.InquilinoId);
                    cmd.Parameters.AddWithValue("@GaranteId", e.GaranteId);
                    cmd.Parameters.AddWithValue("@desde", e.Desde);
                    cmd.Parameters.AddWithValue("@hasta", e.Hasta);
                    cmd.Parameters.AddWithValue("@precio", e.Precio);
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
                string sql = $"DELETE FROM Contrato WHERE Id = @id";
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

        public int Edit(Contrato e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contrato " +
                        $"SET InmuebleId = @InmuebleId, InquilinoId = @InquilinoId, " +
                            $"GaranteId = @GaranteId, desde = @desde, hasta = @hasta, precio = @precio " +
                    $"WHERE Id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@InmuebleId", e.InmuebleId);
                    cmd.Parameters.AddWithValue("@InquilinoId", e.InquilinoId);
                    cmd.Parameters.AddWithValue("@GaranteId", e.GaranteId);
                    cmd.Parameters.AddWithValue("@desde", e.Desde);
                    cmd.Parameters.AddWithValue("@hasta", e.Hasta);
                    cmd.Parameters.AddWithValue("@precio", e.Precio);
                    cmd.Parameters.AddWithValue("@Id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public IList<Contrato> getAll()
        {
            var res = new List<Contrato>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vContratos";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Contrato e = null;
                    while (reader.Read())
                    {
                        e = new Contrato()
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(2),
                                Ambientes = reader.GetInt32(3),
                            },
                            InquilinoId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                PersonaId = reader.GetInt32(5),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(5),
                                    Apellido = reader.GetString(6),
                                    Nombre = reader.GetString(7)
                                }
                            },
                            GaranteId = reader.GetInt32(8),
                            Garante = new Garante
                            {
                                Id = reader.GetInt32(8),
                                PersonaId = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(9),
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11)
                                }
                            },
                            Desde = reader.GetDateTime(12),
                            Hasta = reader.GetDateTime(13),
                            CantidadMeses = cantidadMeses(reader.GetDateTime(12), reader.GetDateTime(13)),
                            Precio = reader.GetDecimal(14)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }
        public IList<Contrato> getAllVigentes()
        {
            var res = new List<Contrato>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vContratos WHERE hasta >= GETDATE() ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Contrato e = null;
                    while (reader.Read())
                    {
                        e = new Contrato()
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(2),
                                Ambientes = reader.GetInt32(3),
                            },
                            InquilinoId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                PersonaId = reader.GetInt32(5),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(5),
                                    Apellido = reader.GetString(6),
                                    Nombre = reader.GetString(7)
                                }
                            },
                            GaranteId = reader.GetInt32(8),
                            Garante = new Garante
                            {
                                Id = reader.GetInt32(8),
                                PersonaId = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(9),
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11)
                                }
                            },
                            Desde = reader.GetDateTime(12),
                            Hasta = reader.GetDateTime(13),
                            CantidadMeses = cantidadMeses(reader.GetDateTime(12), reader.GetDateTime(13)),
                            Precio = reader.GetDecimal(14)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }
        public IList<Contrato> getByInmueble(int inmuebleId)
        {
            var res = new List<Contrato>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vContratos WHERE InmuebleId = @inmuebleId ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@inmuebleId", inmuebleId);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Contrato e = null;
                    while (reader.Read())
                    {
                        e = new Contrato()
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(2),
                                Ambientes = reader.GetInt32(3),
                            },
                            InquilinoId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                PersonaId = reader.GetInt32(5),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(5),
                                    Apellido = reader.GetString(6),
                                    Nombre = reader.GetString(7)
                                }
                            },
                            GaranteId = reader.GetInt32(8),
                            Garante = new Garante
                            {
                                Id = reader.GetInt32(8),
                                PersonaId = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(9),
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11)
                                }
                            },
                            Desde = reader.GetDateTime(12),
                            Hasta = reader.GetDateTime(13),
                            CantidadMeses = cantidadMeses(reader.GetDateTime(12), reader.GetDateTime(13)),
                            Precio = reader.GetDecimal(14)
                        };
                        res.Add(e);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public Contrato getById(int id)
        {
            Contrato e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vContratos WHERE Id = @id ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        e = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString(2),
                                Ambientes = reader.GetInt32(3),
                            },
                            InquilinoId = reader.GetInt32(4),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                PersonaId = reader.GetInt32(5),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(5),
                                    Apellido = reader.GetString(6),
                                    Nombre = reader.GetString(7)
                                }
                            },
                            GaranteId = reader.GetInt32(8),
                            Garante = new Garante
                            {
                                Id = reader.GetInt32(8),
                                PersonaId = reader.GetInt32(9),
                                Persona = new Persona
                                {
                                    Id = reader.GetInt32(9),
                                    Apellido = reader.GetString(10),
                                    Nombre = reader.GetString(11)
                                }
                            },
                            Desde = reader.GetDateTime(12),
                            Hasta = reader.GetDateTime(13),
                            CantidadMeses = cantidadMeses(reader.GetDateTime(12), reader.GetDateTime(13)),
                            Precio = reader.GetDecimal(14)
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }

        public Decimal calcularMulta(Contrato e)
        {
            decimal importe = 0;

            return importe;
        }

        public int cantidadMeses(DateTime desde, DateTime hasta)
        {
            int res = -1;
            TimeSpan difFechas = hasta - desde;
            int dias = difFechas.Days;
            res = (dias / 30) < 1 ? 1 : (int)(dias / 30);
            return res;
        }

    }
}
