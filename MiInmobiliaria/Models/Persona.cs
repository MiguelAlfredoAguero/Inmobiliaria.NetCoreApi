using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
	public enum enRoles
	{
		SuperAdministrador = 1,
		Administrador = 2,
		Empleado = 3,
		Agencia = 4,
		Propietario = 5,
		Inquilino = 6,
		Garante = 7,
	}

	public class Persona
	{
		// TUTORIAL
		// https://www.youtube.com/watch?v=3mu2K5vXcxc

		[Key, Display(Name = "Código")]
		public int Id { get; set; }
		public string Apellido { get; set; }
		public string Nombre { get; set; }

		[DataType(DataType.Date)]
		[Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
		[Display(Name = "Fecha Nac.")]
		public DateTime FechaNac { get; set; }
		[Display(Name = "DNI/CUIL")]
		public string Dni { get; set; }
		public int TipoPersonaId { get; set; }
		[ForeignKey("TipoPersonaId")]
        public TipoPersona TipoPersona { get; set; }
        public string Telefono { get; set; }
		public string Avatar { get; set; }
		[Display(Name = "Avatar")]
		[NotMapped]
		public IFormFile AvatarFile { get; set; }
		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[Required, DataType(DataType.Password), Display(Name = "Clave")]
		public string Password { get; set; }
		public int Rol { get; set; }
		public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";
		
		
		
		public static IDictionary<int, string> ObtenerRoles()
		{
			SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
			Type tipoEnumRol = typeof(enRoles);
			foreach (var valor in Enum.GetValues(tipoEnumRol))
			{
				roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
			}
			return roles;
		}
	}
}
