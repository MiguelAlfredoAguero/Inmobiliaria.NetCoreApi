using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Utils
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public Utils(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }
        
        public string getPasswordHashed(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
            return hashed;
        }

        public string uploadFile(Persona e)
        {
            if (e.AvatarFile != null && e.Id > 0)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Path.GetFileName(u.AvatarFile.FileName); //este nombre se puede repetir
                string fileName = "avatar_" + e.Id + Path.GetExtension(e.AvatarFile.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                e.Avatar = Path.Combine("/Uploads", fileName);
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    e.AvatarFile.CopyTo(stream);
                }
            }
            return e.Avatar;
        }

        public string uploadFile(Inmueble e)
        {
            if (e.AvatarFile != null && e.Id > 0)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //Path.GetFileName(u.AvatarFile.FileName); //este nombre se puede repetir
                string fileName = "foto_" + e.Id + Path.GetExtension(e.AvatarFile.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                e.Avatar = Path.Combine("/Uploads", fileName);
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    e.AvatarFile.CopyTo(stream);
                }
            }
            return e.Avatar;
        }
    }
}
