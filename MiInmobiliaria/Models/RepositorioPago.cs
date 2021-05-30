using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        private readonly RepositorioContrato repositorioContrato;
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
            this.repositorioContrato = new RepositorioContrato(configuration);
        }
        public int Create(Pago e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO pago (numero, fecha, importe, ContratoId) " +
                    $"VALUES (@numero, @fecha, @importe, @ContratoId); " +
                    $"SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@numero", e.Numero);
                    cmd.Parameters.AddWithValue("@fecha", e.Fecha);
                    cmd.Parameters.AddWithValue("@importe", e.Importe);
                    cmd.Parameters.AddWithValue("@ContratoId", e.ContratoId);
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
                string sql = $"DELETE FROM Pago WHERE Id = @id";

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

        public int Edit(Pago e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Pago " +
                        $"SET numero = @numero, fecha = @fecha, importe = @importe " +
                    $"WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@numero", e.Numero);
                    cmd.Parameters.AddWithValue("@fecha", e.Fecha);
                    cmd.Parameters.AddWithValue("@importe", e.Importe);
                    cmd.Parameters.AddWithValue("@Id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }

        public IList<Pago> getAll()
        {
            var res = new List<Pago>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPagos ORDER BY numero DESC";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Pago e = null;

                    while (reader.Read())
                    {
                        e = new Pago
                        {
                            Id = reader.GetInt32(0),
                            Numero = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(4),
                                Desde = reader.GetDateTime(5),
                                Hasta = reader.GetDateTime(6),
                                CantidadMeses = repositorioContrato.cantidadMeses(reader.GetDateTime(5), reader.GetDateTime(6)),
                                Precio = reader.GetDecimal(7),
                                InquilinoId = reader.GetInt32(8),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(8),
                                    PersonaId = reader.GetInt32(9),
                                    Persona = new Persona()
                                    {
                                        Id = reader.GetInt32(9),
                                        Apellido = reader.GetString(10),
                                        Nombre = reader.GetString(11)
                                    }
                                },
                                InmuebleId = reader.GetInt32(12),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(12),
                                    Direccion = reader.GetString(13),
                                    Ambientes = reader.GetInt32(14)
                                }
                            }
                        };
                        res.Add(e);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public IList<Pago> getAll(int ContratoId)
        {
            var res = new List<Pago>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPagos WHERE ContratoId = @ContratoId ORDER BY numero DESC";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {

                    cmd.Parameters.AddWithValue("@ContratoId", ContratoId);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Pago e = null;

                    while (reader.Read())
                    {
                        e = new Pago
                        {
                            Id = reader.GetInt32(0),
                            Numero = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(4),
                                Desde = reader.GetDateTime(5),
                                Hasta = reader.GetDateTime(6),
                                CantidadMeses = repositorioContrato.cantidadMeses(reader.GetDateTime(5), reader.GetDateTime(6)),
                                Precio = reader.GetDecimal(7),
                                InquilinoId = reader.GetInt32(8),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(8),
                                    PersonaId = reader.GetInt32(9),
                                    Persona = new Persona()
                                    {
                                        Id = reader.GetInt32(9),
                                        Apellido = reader.GetString(10),
                                        Nombre = reader.GetString(11)
                                    }
                                },
                                InmuebleId = reader.GetInt32(12),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(12),
                                    Direccion = reader.GetString(13),
                                    Ambientes = reader.GetInt32(14)
                                }
                            }
                        };
                        res.Add(e);
                    }

                    con.Close();
                }
            }
            return res;
        }

        public Pago getById(int id)
        {
            Pago e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPagos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        e = new Pago
                        {
                            Id = reader.GetInt32(0),
                            Numero = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(4),
                                Desde = reader.GetDateTime(5),
                                Hasta = reader.GetDateTime(6),
                                CantidadMeses = repositorioContrato.cantidadMeses(reader.GetDateTime(5), reader.GetDateTime(6)),
                                Precio = reader.GetDecimal(7),
                                InquilinoId = reader.GetInt32(8),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(8),
                                    PersonaId = reader.GetInt32(9),
                                    Persona = new Persona()
                                    {
                                        Id = reader.GetInt32(9),
                                        Apellido = reader.GetString(10),
                                        Nombre = reader.GetString(11)
                                    }
                                },
                                InmuebleId = reader.GetInt32(12),
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(12),
                                    Direccion = reader.GetString(13)
                                }
                            }
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }


        public int numeroUltimoPago(int ContratoId)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT IsNull(MAX(numero),0) FROM Pago WHERE ContratoId = @ContratoId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ContratoId", ContratoId);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }
        public int cantidadPagosHechos(int ContratoId)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT COUNT(*) FROM Pago WHERE ContratoId = @ContratoId";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@ContratoId", ContratoId);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }
        public int numeroTotalPagos(Contrato e)
        {
            int res = -1;
            TimeSpan difFechas = e.Hasta - e.Desde;
            res = (int)( difFechas.Days / 30 < 1 ? 1 : difFechas.Days / 30 );
            return res;
        }
    }
}
