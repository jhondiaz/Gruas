using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Services.Entitys.Entities;
using Services.DataAccess;
using System.Net.Mail;
using System.Net.Mime;

namespace CancelarSolicitudes
{
    public class CencelAuto
    {
        /// <summary>
        /// Contextos de Base de Datos, Configuraciones de Hora Regional
        /// </summary>
        Context context = new Context();
        static TimeZoneInfo horazone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        DateTime horacol = TimeZoneInfo.ConvertTime(DateTime.Now, horazone);

        /// <summary>
        /// IncioSinc
        /// Description: Se inicia la consola que procesa la Cancelación de Solicitudes de Grúas.
        /// </summary>
        /// <returns></returns>
        public void IncioSinc()
        {
            try
            {
                Console.WriteLine("Iniciando Cancelación Automatica...");
                Thread.Sleep(1000);
                Console.WriteLine("...Cancelando Solicitudes...");
                Console.WriteLine(InicioCancel());
                Thread.Sleep(1000);
                Console.WriteLine("...Iniciando Validación ANS...");
                Console.WriteLine(validarANS());
                Console.WriteLine("...Validando Envío Email...");
                Console.WriteLine(valmail());
                Thread.Sleep(6000);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error en la cancelación automatica " + ex.Message);
                Thread.Sleep(8000);
            }
        }

        /// <summary>
        /// InicioCancel
        /// Description: Método usado para iniciar el proceso de cancelación de solicitudes de grúas.
        /// </summary>
        /// <returns></returns>
        public dynamic InicioCancel()
        {
            try
            {

                var cont = 0;

                var query2 = context.SolicitudGruas.Where(t => t.Fecha_Cierre_Auto != null && (t.Estado == "SOLICITADA" || t.Estado == "APROBADA" || t.Estado == "REASIGNADA")).ToList();


                foreach (var item in query2)
                {
                    if (horacol >= item.Fecha_Cierre_Auto)
                    {
                        cont++;

                        SolicitudGruas ob = context.SolicitudGruas.Where(t => t.ID_solicitud == item.ID_solicitud).FirstOrDefault();

                        ob.Estado = "CANCELADA AUT";
                        ob.Fecha_Cierre_Auto = horacol;
                        context.SaveChanges();

                        Estados est = new Estados();
                        est.ID_solicitud = item.ID_solicitud;
                        est.Nombre = "CANCELADA AUT";
                        est.Fecha = horacol;
                        context.Estados.Add(est);
                        context.SaveChanges();

                        Console.WriteLine("Cancelaciones Satisfactorias " + cont);
                    }

                }
                return "Cacelaciones Automaticas Realizadas Satisfactoriamente.";
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw;
            }
        }

        /// <summary>
        /// ValidarANS
        /// Description: Cambia estado de las solicitudes según parámetros establecido por configuración.
        /// </summary>
        /// <returns></returns>
        public dynamic validarANS()
        {
            try
            {
                var cont = 0;

                var query2 = context.SolicitudGruas.Where(t => t.ValANS != true && t.Estado == "APROBADA").ToList();

                foreach (var item in query2)
                {
                    if (horacol >= item.ANSTime)
                    {
                        cont++;

                        SolicitudGruas ob = context.SolicitudGruas.Where(t => t.ID_solicitud == item.ID_solicitud).FirstOrDefault();

                        ob.ValANS = true;
                        context.SaveChanges();

                        Console.WriteLine(cont + " Actualización ANS Satisfactoria.");
                    }
                }
                return "Actualizacions ANS Satisfactorias.";
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

        /// <summary>
        /// valmail
        /// Description: Método para enviar los mails, para las solicitudes que cumplieron el tiempo ANS.
        /// </summary>
        /// <returns></returns>
        public dynamic valmail()
        {
            try
            {
                var anstime = context.ConfigCierreAutos.Where(t => t.Id == "3").FirstOrDefault();

                DateTime MyDateTime = DateTime.Parse(anstime.Horas);
                DateTime MyDateTimeAdd = DateTime.Parse(anstime.Horas);

                //var restatime = item.ANSTime.AddHours
                if (horacol > MyDateTime && horacol < MyDateTimeAdd.AddMinutes(5))
                {
                    var minuets = MyDateTime.AddHours(-24);
                    var filterdate = context.SolicitudGruas.Where(t => (t.Fecha_y_hora_solicitud_servicio > minuets && t.Fecha_y_hora_solicitud_servicio < MyDateTime) && t.ValANS == true).ToList();

                    var table = "<table border=1 style='margin-right:auto;margin-left:auto;'><tr><td>Id Solicitud</td><td>Entidad</td><td>Fecha Hora Solicitud</td><td>Codigo Infraccion</td><td>Estado</td></tr>";
                    var cuerpo = "";
                    foreach (var item in filterdate)
                    {
                        cuerpo += "<tr><td>" + item.ID_solicitud + "</td><td>" + item.Entidad + "</td><td>" + item.Fecha_y_hora_solicitud_servicio + "</td><td>" + item.Codigo_de_infraccion + "</td><td>" + item.Estado + "</td></tr>";
                    }
                    var total = table + cuerpo + "</table>";


                    var listcrr = context.ListaCorreosANS.ToList();

                    if (listcrr != null)
                    {
                        foreach (var item in listcrr)
                        {
                            var path = String.Format(@"{0}..\..\img\heater.jpg", AppDomain.CurrentDomain.BaseDirectory);
                            LinkedResource inline = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                            inline.ContentId = Guid.NewGuid().ToString();

                            SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
                            server.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailUser"], ConfigurationManager.AppSettings["PassUser"]);
                            server.EnableSsl = true;
                            MailMessage mnsj = new MailMessage();
                            mnsj.Subject = "Informe ANS";
                            mnsj.To.Add(new MailAddress(item.Correo));
                            mnsj.From = new MailAddress(item.Correo, "Plataforma Gruas");
                            //mnsj.Attachments.Add(new Attachment("C:\\archivo.pdf"));
                            string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
                            body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\"></HEAD>";
                            body += "<BODY><div style='width: 677px;height: 64px;'>";
                            body += String.Format(@"<img src=""cid:{0}"" style ='height:100%;width:100%'/> ", inline.ContentId);
                            body += "</div>";
                            body += "<DIV style='width: 643px;background-color: white;height: auto;border: 2px solid #64A7C0;text-align:center;padding:15px;'>";
                            body += "<h1 style='color: cadetblue'>Informe ANS</h1>";
                            body += total;
                            body += "</DIV></BODY></HTML>";

                            ContentType mimeType = new System.Net.Mime.ContentType("text/html");


                            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                            mnsj.AlternateViews.Add(alternate);
                            alternate.LinkedResources.Add(inline);
                            mnsj.Body = body;
                            server.Send(mnsj);
                        }
                    }
                    else
                    {
                        return "No hay Correos asociados, no se puede generar el informe.";
                    }
                    return "Informe ANS generado satisfactoriamente.";
                }
                else
                {
                    return "No se genero informe.";
                }
                
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

    }
}
