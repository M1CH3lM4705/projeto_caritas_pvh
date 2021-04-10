using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjetoBetaAutenticacao.Infraestrutura;
using ProjetoBetaAutenticacao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ProjetoBetaAutenticacao.Utilidades
{
    public class Utils
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private static CaritasContext context = new CaritasContext();


        public static void ChangeUserName(string userNameOld, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(userNameOld);

            if (userASP == null)
            {
                return;
            }

            userASP.UserName = newUserName;
           
            userManager.Update(userASP);
        }

        public static void ChangeUserParoquia(string userName, string newParoquia)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(userName);

            if (userASP == null || userASP.Paroquia == newParoquia)
            {
                return;
            }

            userASP.Paroquia = newParoquia;

            userManager.Update(userASP);
        }

        public static bool UserAtivo(string userName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(userName);
            if (userASP == null)
            {
                return true;
            }
            return userASP.Ativo;
        }

        public static bool ValidarSenhaAtual(string id, string senha)
        {
            
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindById(id);

            return VerifyHashedPassword(userASP.PasswordHash, senha);
             
        }
        public static void ChangePassword(string username, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(username);

            if(userASP == null || userASP.PasswordHash == HashPassword(password))
            {
                return;
            }
            userASP.PasswordHash = HashPassword(password);
            userContext.Users.Attach(userASP);
            userContext.Entry(userASP).Property(x => x.PasswordHash).IsModified = true;
            userContext.SaveChanges();
        }
        public static void AtivarDesativarUser(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(username);
            if (userASP.Ativo == true)
            {
                userASP.Ativo = false;
            }
            else
            {
                userASP.Ativo = true;
            }
            userContext.Users.Attach(userASP);
            userContext.Entry(userASP).Property(x => x.Ativo).IsModified = true;
            userContext.SaveChanges();
        }

        public static void ExcluirUserManager(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(username);

            if (userASP == null)
            {
                return;
            }
            userContext.Users.Remove(userASP);
            userContext.SaveChanges();
        }

        public static void ChangeEmailUserASP(string oldEmail, string newEmail)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(oldEmail);

            if (userASP == null)
            {
                return;
            }

            userASP.UserName = newEmail;
            userASP.Email = newEmail;
            userManager.Update(userASP);
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
        public static void RemoverRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));
            var role =  roleManager.FindByName(roleName);
            if (role == null)
            {
                return;
                
            }
            roleManager.Delete(role);
        }
        public static void CheckSuperUser(string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var username = WebConfigurationManager.AppSettings["AdminUser"];
            var password = WebConfigurationManager.AppSettings["AdminPassWord"];
            var userASP = userManager.FindByName(username);
            if (userASP == null)
            {
                CreateUserASP(username, role, password);
                return;
            }
        }

        public static void CreateUserASP(string username)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                UserName = username,
            };

            userManager.Create(userASP, username);
        }

        public static void CreateUserASP(string username, string paroquia)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = new ApplicationUser
            {
                UserName = username,
                Paroquia = paroquia,
                Ativo = true,
                Email = ($"{username}@{username}.com")
        };

            userManager.Create(userASP, username);
            //userManager.AddToRole(userASP.Id, roleName);
        }

        public static void AddRoleToUser(string username, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(username);
            if (userASP == null)
            {
                return;
            }

            userManager.AddToRole(userASP.Id, roleName);
            
        }

        public static void UpdateRoleToUser(string username, string oldRoleName, string newRoleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByName(username);
            if(userASP == null)
            {
                return;
            }

            userManager.RemoveFromRole(userASP.Id, oldRoleName);
            userManager.AddToRole(userASP.Id, newRoleName);
        }

        public static void CreateUserASP(string username, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            var userASP = userManager.FindByName(username);
            if (userASP == null)
            {
                userASP = new ApplicationUser
                {
                    UserName = username,
                    Ativo = true
                };

                userManager.Create(userASP, password);
                userManager.AddToRole(userASP.Id, roleName);
            }
        }

        public static async Task SendMail(string to, string subject, string body)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"]
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        internal static void AddRoleToUser(object userName, string v)
        {
            throw new NotImplementedException();
        }

        public static async Task SendMail(List<string> mails, string subject, string body)
        {
            var message = new MailMessage();

            foreach (var to in mails)
            {
                message.To.Add(new MailAddress(to));
            }

            message.From = new MailAddress(WebConfigurationManager.AppSettings["AdminUser"]);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = WebConfigurationManager.AppSettings["AdminUser"],
                    Password = WebConfigurationManager.AppSettings["AdminPassWord"]
                };

                smtp.Credentials = credential;
                smtp.Host = WebConfigurationManager.AppSettings["SMTPName"];
                smtp.Port = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
        }

        public static async Task PasswordRecovery(string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);
            if (userASP == null)
            {
                return;
            }

            var user = context.Voluntarios.Where(tp => tp.UserName == email).FirstOrDefault();
            if (user == null)
            {
                return;
            }

            var random = new Random();
            var newPassword = string.Format("{0}{1}{2:04}*",
                user.Nome.Trim().ToUpper().Substring(0, 1),
                user.SobreNome.Trim().ToLower(),
                random.Next(10000));

            userManager.RemovePassword(userASP.Id);
            userManager.AddPassword(userASP.Id, newPassword);

            var subject = "Notes Password Recovery";
            var body = string.Format(@"
                <h1>Taxes Password Recovery</h1>
                <p>Yor new password is: <strong>{0}</strong></p>
                <p>Please change it for one, that you remember easyly",
                newPassword);

            await SendMail(email, subject, body);
        }

        /*public static string UploadPhoto(HttpPostedFileBase file)
        {
            // Upload image
            string path = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Fotos"), pic);
                file.SaveAs(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }
            }

            return pic;
        }*/

        //Metodo para criptografar senhas
        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        //Metodo para verificação de Hash
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;
            for (int i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        public void Dispose()
        {
            userContext.Dispose();
            context.Dispose();
        }
    }
}