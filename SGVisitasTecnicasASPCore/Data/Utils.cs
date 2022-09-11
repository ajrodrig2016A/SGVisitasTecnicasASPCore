using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace SGVisitasTecnicasASPCore.Data
{
    public class Utils
    {
        public const decimal ZERO_DEC = (decimal)0.00;
        internal static bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        internal static bool VerificaIdentificacion(string identificacion)
        {
            bool estado = false;
            char[] valced = new char[13];
            int provincia;
            if (identificacion.Length >= 10)
            {
                valced = identificacion.Trim().ToCharArray();
                provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
                if (provincia > 0 && provincia < 25)
                {
                    if (int.Parse(valced[2].ToString()) < 6)
                    {
                        estado = VerificaCedula(valced);
                    }
                    else if (int.Parse(valced[2].ToString()) == 6)
                    {
                        estado = VerificaSectorPublico(valced);
                    }
                    else if (int.Parse(valced[2].ToString()) == 9)
                    {

                        estado = VerificaPersonaJuridica(valced);
                    }
                }
            }
            return estado;
        }

        public static bool VerificaCedula(char[] validarCedula)
        {
            int aux = 0, par = 0, impar = 0, verifi;
            for (int i = 0; i < 9; i += 2)
            {
                aux = 2 * int.Parse(validarCedula[i].ToString());
                if (aux > 9)
                    aux -= 9;
                par += aux;
            }
            for (int i = 1; i < 9; i += 2)
            {
                impar += int.Parse(validarCedula[i].ToString());
            }

            aux = par + impar;
            if (aux % 10 != 0)
            {
                verifi = 10 - (aux % 10);
            }
            else
                verifi = 0;
            if (verifi == int.Parse(validarCedula[9].ToString()))
                return true;
            else
                return false;
        }

        public static bool VerificaPersonaJuridica(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[9] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
                for (int i = 0; i < 9; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }
                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[9].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool VerificaSectorPublico(char[] validarCedula)
        {
            int aux = 0, prod, veri;
            veri = int.Parse(validarCedula[9].ToString()) + int.Parse(validarCedula[10].ToString()) + int.Parse(validarCedula[11].ToString()) + int.Parse(validarCedula[12].ToString());
            if (veri > 0)
            {
                int[] coeficiente = new int[8] { 3, 2, 7, 6, 5, 4, 3, 2 };

                for (int i = 0; i < 8; i++)
                {
                    prod = int.Parse(validarCedula[i].ToString()) * coeficiente[i];
                    aux += prod;
                }

                if (aux % 11 == 0)
                {
                    veri = 0;
                }
                else if (aux % 11 == 1)
                {
                    return false;
                }
                else
                {
                    aux = aux % 11;
                    veri = 11 - aux;
                }

                if (veri == int.Parse(validarCedula[8].ToString()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string ConvertStringtoMD5(string strword)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(strword);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string GetSHA256(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public static bool SendEmailRecuperarClave(string recipient, string sender, string subject, string htmlFilePath, string user, string urlRecovery)
        {
            try
            {
                MailMessage message = null;

                using (StreamReader reader = File.OpenText(htmlFilePath)) // Path to your 
                {                                                         // HTML file
                    MailAddress addressFrom = new MailAddress(recipient, "SAIMEC Admin.");
                    MailAddress addressTo = new MailAddress(sender);
                    MailAddress addressBCC = new MailAddress("ventas.saimec@gmail.com");
                    message = new MailMessage(addressFrom, addressTo);
                    message.Bcc.Add(addressBCC);
                    message.Subject = subject;
                    string bodyEmailRecuperarClave = reader.ReadToEnd();  // Load the content from your file...
                                                                          //...
                    message.Body = bodyEmailRecuperarClave.Replace("(HORA Y FECHA)", DateTime.Now.ToString("F")).Replace("(NOMBRE USUARIO)", user).Replace("(LINK DE  RESETEO DE CONTRASE&Ntilde;A)", urlRecovery);
                    message.IsBodyHtml = true;
                }

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("rodandrews90210@gmail.com", "oztzvowdqdiegemp")
                };
                // code in brackets above needed if authentication required
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool SendEmailSolicitarInformacion(string sender, string recipient, string subject, string htmlFilePath, string user, string datosCliente)
        {
            try
            {
                MailAddress to = new MailAddress(recipient);
                MailAddress from = new MailAddress(sender);

                MailMessage message = null;

                using (StreamReader reader = File.OpenText(htmlFilePath)) // Path to your 
                {                                                         // HTML file
                    MailAddress addressFrom = new MailAddress(sender, "SAIMEC Admin.");
                    MailAddress addressTo = new MailAddress(recipient);
                    MailAddress addressBCC = new MailAddress("ventas.saimec@gmail.com");
                    message = new MailMessage(addressFrom, addressTo);
                    message.Bcc.Add(addressBCC);
                    message.Subject = subject;
                    string bodyEmailSolicitarInformacion = reader.ReadToEnd();  // Load the content from your file...
                                                                          //...
                    message.Body = bodyEmailSolicitarInformacion.Replace("(HORA Y FECHA)", DateTime.Now.ToString("F")).Replace("(NOMBRE USUARIO)", user).Replace("(DATOS DEL CLIENTE)", datosCliente);
                    message.IsBodyHtml = true;
                }

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("rodandrews90210@gmail.com", "oztzvowdqdiegemp")                    
                };
                // code in brackets above needed if authentication required
                client.Send(message);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public bool SendEmail(string sender, string recipient, string subject, string bodyText)
        {
            try
            {
                MailAddress to = new MailAddress(recipient);
                MailAddress from = new MailAddress(sender);

                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;
                message.Body = bodyText;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("c5caf274a330ff", "d4489958dcdc18"),
                    EnableSsl = true
                };
                // code in brackets above needed if authentication required
                client.Send(message);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

    }
}
