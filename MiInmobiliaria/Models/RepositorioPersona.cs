using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class RepositorioPersona : RepositorioBase, IRepositorioPersona
    {
        public RepositorioPersona(IConfiguration configuration) : base(configuration)
        {
        }

        /*
                ALTER TABLE Persona
                ADD CONSTRAINT fk_PersonaTipoPersonaId FOREIGN KEY(TipoPersonaId) REFERENCES TipoPersona(Id);

                ALTER TABLE Agencia
                ADD CONSTRAINT fk_AgenciaPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

                ALTER TABLE Propietario
                ADD CONSTRAINT fk_PropietarioPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

                ALTER TABLE Inquilino
                ADD CONSTRAINT fk_InquilinoPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

                ALTER TABLE Garante
                ADD CONSTRAINT fk_GarantePersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

                ALTER TABLE Usuario
                ADD CONSTRAINT fk_UsuarioPersonaId FOREIGN KEY(PersonaId) REFERENCES Persona(Id);

                ALTER TABLE Inmueble
                ADD CONSTRAINT fk_InmueblePropietarioId FOREIGN KEY(PropietarioId) REFERENCES Propietario(Id);

                ALTER TABLE Inmueble
                ADD CONSTRAINT fk_InmuebleAgenciaId FOREIGN KEY(AgenciaId) REFERENCES Agencia(Id);

                ALTER TABLE Inmueble
                ADD CONSTRAINT fk_InmuebleTipoInmuebleId FOREIGN KEY(TipoInmuebleId) REFERENCES TipoInmueble(Id);

                ALTER TABLE Inmueble
                ADD CONSTRAINT fk_InmuebleUsoInmuebleId FOREIGN KEY(UsoInmuebleId) REFERENCES UsoInmueble(Id);

                ALTER TABLE Contrato
                ADD CONSTRAINT fk_ContratoInmuebleId FOREIGN KEY(InmuebleId) REFERENCES Inmueble(Id);

                ALTER TABLE Contrato
                ADD CONSTRAINT fk_ContratoInquilinoId FOREIGN KEY(InquilinoId) REFERENCES Inquilino(Id);

                ALTER TABLE Contrato
                ADD CONSTRAINT fk_ContratoGaranteId FOREIGN KEY(GaranteId) REFERENCES Garante(Id);

                ALTER TABLE Pago
                ADD CONSTRAINT fk_PagoContratoId FOREIGN KEY(ContratoId) REFERENCES Contrato(Id);
        */

        //exec sp_fkeys 'TipoPersona';

        //ALTER TABLE Persona DROP CONSTRAINT fk_TipoPersonaId;


        public int Create(Persona e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Persona ( apellido, nombre, fechaNac, dni, " +
                    $"TipoPersonaId, telefono, email, password, rol, avatar) " +
                    $"VALUES ( @apellido, @nombre, @fechaNac, @dni, @TipoPersonaId, " +
                        $"@telefono, @email, @password, @rol, @avatar); " +
                    $"SELECT SCOPE_IDENTITY();";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@apellido", e.Apellido);
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@fechaNac", e.FechaNac);
                    cmd.Parameters.AddWithValue("@dni", e.Dni);
                    cmd.Parameters.AddWithValue("@TipoPersonaId", e.TipoPersonaId);
                    cmd.Parameters.AddWithValue("@telefono", e.Telefono);
                    cmd.Parameters.AddWithValue("@email", e.Email);
                    cmd.Parameters.AddWithValue("@password", e.Password);
                    cmd.Parameters.AddWithValue("@rol", e.Rol);
                    cmd.Parameters.AddWithValue("@avatar", e.Avatar);
                    con.Open();
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return res;
        }

        public int Edit(Persona e)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE persona SET " +
                        $"apellido = @apellido, nombre = @nombre, fechaNac = @fechaNac, dni = @dni, " +
                        $"TipoPersonaId = @TipoPersonaId, telefono = @telefono, email = @email, " +
                        $"password = @password, avatar =  @avatar, rol =  @rol " +
                    $"WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@apellido", e.Apellido);
                    cmd.Parameters.AddWithValue("@nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@fechaNac", e.FechaNac);
                    cmd.Parameters.AddWithValue("@dni", e.Dni);
                    cmd.Parameters.AddWithValue("@TipoPersonaId", e.TipoPersonaId);
                    cmd.Parameters.AddWithValue("@telefono", e.Telefono);
                    cmd.Parameters.AddWithValue("@email", e.Email);
                    cmd.Parameters.AddWithValue("@password", e.Password);
                    cmd.Parameters.AddWithValue("@avatar", e.Avatar);
                    cmd.Parameters.AddWithValue("@rol", e.Rol);
                    cmd.Parameters.AddWithValue("@id", e.Id);
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return res;
        }
        public int Edit(int id, string avatar)
        {
            int res = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Persona SET avatar = @avatar WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@avatar", avatar);
                    cmd.Parameters.AddWithValue("@id", id);
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
                string sql = $"DELETE FROM {nameof(Persona)} WHERE {nameof(Persona.Id)} = @id";
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
 
        public IList<Persona> getAll()
        {
            var res = new List<Persona>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPersonas ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    Persona persona = null;

                    while (reader.Read())
                    {
                        persona = new Persona
                        {
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            FechaNac = reader.GetDateTime(3),
                            Dni = reader.GetString(4),
                            TipoPersonaId = reader.GetInt32(5),
                            TipoPersona = new TipoPersona
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(6)
                            },
                            Telefono = reader.GetString(7),
                            Email = reader.GetString(8),
                            Password = reader.GetString(9),
                            Avatar = reader.GetString(10),
                            Rol = reader.GetInt32(11)
                        };
                        res.Add(persona);
                    }
                    con.Close();
                }
            }
            return res;
        }

        public Persona getById(int id)
        {
            Persona e = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPersonas WHERE Id = @id  ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        e = new Persona
                        {
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            FechaNac = reader.GetDateTime(3),
                            Dni = reader.GetString(4),
                            TipoPersonaId = reader.GetInt32(5),
                            TipoPersona = new TipoPersona
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(6)
                            },
                            Telefono = reader.GetString(7),
                            Email = reader.GetString(8),
                            Password = reader.GetString(9),
                            Avatar = reader.GetString(10),
                            Rol = reader.GetInt32(11)
                        };
                    }
                    con.Close();
                }
            }
            return e;
        }

        public Persona getByDniEmail(string dni, string email)
        {
            Persona persona = null;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM vPersonas WHERE dni = @dni OR email = @email  ORDER BY apellido, nombre";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@dni", dni);
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        persona = new Persona
                        {
                            Id = reader.GetInt32(0),
                            Apellido = reader.GetString(1),
                            Nombre = reader.GetString(2),
                            FechaNac = reader.GetDateTime(3),
                            Dni = reader.GetString(4),
                            TipoPersonaId = reader.GetInt32(5),
                            TipoPersona = new TipoPersona
                            {
                                Id = reader.GetInt32(5),
                                Nombre = reader.GetString(6)
                            },
                            Telefono = reader.GetString(7),
                            Email = reader.GetString(8),
                            Password = reader.GetString(9),
                            Avatar = reader.GetString(10),
                            Rol = reader.GetInt32(11)
                        };
                    }
                    con.Close();
                }
            }
            return persona;
        }

    }
}
