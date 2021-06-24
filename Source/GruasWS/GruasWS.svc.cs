using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Services.DataAccess;
using Services.Entitys.Entities;
using Services.Entitys.DTOs;
using Newtonsoft.Json;

namespace GruasWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GruasServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GruasServices.svc or GruasServices.svc.cs at the Solution Explorer and start debugging.
    public class GruasServices : IGruasWS
    {

        Context context = new Context();
        static TimeZoneInfo horazone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        DateTime horacol = TimeZoneInfo.ConvertTime(DateTime.Now, horazone);        


        public dynamic WSRespuestaSol(string JSON)
        {

            try
            {
                var select = context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());

                if (desjson.ID_solicitud == 0 || desjson.ID_solicitud == null)
                {
                    return "Debe Envíar un Numero de Solicitud Para Poder Continuar.";
                }
                else
                {

                    SolicitudGruas ob = context.SolicitudGruas.Where(t => t.ID_solicitud == desjson.ID_solicitud).FirstOrDefault();

                    ob.Fecha_y_hora_solicitud_servicio_res = desjson.Fecha_y_hora_solicitud_servicio_res;
                    ob.Direccion_georreferencial_res = desjson.Direccion_georreferencial_res;
                    ob.Confirmacion_de_envio = desjson.Confirmacion_de_envio;
                    ob.Numero_de_orden_del_servicio = desjson.Numero_de_orden_del_servicio;
                    ob.Causal_de_rechazo = desjson.Causal_de_rechazo;
                    ob.Fecha_y_hora_de_orden_de_servicio = desjson.Fecha_y_hora_de_orden_de_servicio;
                    ob.Placa_grua_Numero = desjson.Placa_grua_Numero;
                    ob.Tipo_documento_identificacion_conductor_grua = desjson.Tipo_documento_identificacion_conductor_grua;
                    ob.Numero_de_identificacion_conductor_grua = desjson.Numero_de_identificacion_conductor_grua;
                    //ob.Tipo_de_Grua = desjson.Tipo_de_Grua;
                    //ob.Numero_de_gruas_solicitadas_por_tipo_de_grua = desjson.Numero_de_gruas_solicitadas_por_tipo_de_grua;

                    if (desjson.Causa_de_inmovilizacion == "2")
                    {
                        ob.Parqueadero_destino = "PARQUEADERO TRANSITORIO";
                    }

                    if (desjson.Confirmacion_de_envio == "1")
                    {
                        ob.Estado = "APROBADA";
                        var anstime = context.ConfigCierreAutos.Where(t => t.Id == "2").FirstOrDefault();
                        ob.Fecha_Cierre_Auto = horacol.AddMinutes(Convert.ToDouble(select.Horas));
                        ob.ANSTime = horacol.AddMinutes(Convert.ToDouble(anstime.Horas));
                    }
                    else
                    {
                        ob.Estado = "RECHAZADA";
                    }
                    context.SaveChanges();


                    Estados est = new Estados();
                    est.ID_solicitud = desjson.ID_solicitud;
                    if (desjson.Confirmacion_de_envio == "1")
                    {
                        est.Nombre = "APROBADA";
                        est.Observaciones = null;
                    }
                    else
                    {
                        est.Nombre = "RECHAZADA";
                        est.Observaciones = desjson.Causal_de_rechazo;
                    }

                    est.Fecha = horacol;
                    context.Estados.Add(est);
                    context.SaveChanges();


                    if (desjson.Causa_de_inmovilizacion == "2" && desjson.Confirmacion_de_envio == "1")
                    {
                        List<string> lista1 = new List<string>();

                        lista1.Add("Parqueadero Tansitorio");
                        lista1.Add("Código de resultado: EXITOSO");
                        lista1.Add("Severidad: INFORMACION");
                        lista1.Add("Confirmación enviada exitosamente");

                        return lista1;
                    }
                    else if (desjson.Confirmacion_de_envio == "2")
                    {
                        List<string> lista1 = new List<string>();

                        lista1.Add("Estado Solicitud: RECHAZADO");
                        lista1.Add("Severidad: INFORMACIÓN");
                        lista1.Add("Solicitud Rechazada por " + desjson.Causa_de_Cancelacion_de_Servicio);
                        return "Respuesta Recibida Satisfactoriamente.";
                    }
                    else
                    {
                        List<string> lista1 = new List<string>();                        
                        lista1.Add("Código de resultado: EXITOSO");
                        lista1.Add("Severidad: INFORMACION");
                        lista1.Add("Confirmación enviada exitosamente");

                        return lista1;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw ex;
            }
        }

        public dynamic WSRegistrarAt(string JSON)
        {
            try
            {

                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());

                if (desjson.ID_solicitud == 0 || desjson.ID_solicitud == null)
                {
                    return "Debe Envíar un Numero de Solicitud Para Poder Continuar.";
                }
                else
                {

                    SolicitudGruas ob = context.SolicitudGruas.Where(t => t.ID_solicitud == desjson.ID_solicitud).FirstOrDefault();

                    ob.Numero_de_orden_del_servicio = desjson.Numero_de_orden_del_servicio;
                    ob.Fecha_hora_inicio_atencion_servicio = desjson.Fecha_hora_inicio_atencion_servicio;
                    ob.Placa_grua_Numero = desjson.Placa_grua_Numero;
                    ob.Placa_vehiculo_trasladar = desjson.Placa_vehiculo_trasladar;
                    ob.Fecha_hora_Finalizacion_servicio = desjson.Fecha_hora_Finalizacion_servicio;
                    ob.Parqueadero_destino = desjson.Parqueadero_destino;
                    ob.Link_video_inmovilizacion = desjson.Link_video_inmovilizacion;
                    ob.Fecha_hora_envio_informacion = desjson.Fecha_hora_envio_informacion;
                    ob.Estado = "ATENDIDA";
                    context.SaveChanges();

                    Estados est = new Estados();
                    est.ID_solicitud = desjson.ID_solicitud;
                    est.Nombre = "ATENDIDA";
                    est.Observaciones = null;
                    est.Fecha = horacol;
                    context.Estados.Add(est);
                    context.SaveChanges();

                    return "Atención Registrada Satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw;
            }
        }

        public dynamic WSRegistrarNovedad(string JSON)
        {
            try
            {
                var select = context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());

                if (desjson.ID_solicitud == 0 || desjson.ID_solicitud == null)
                {
                    return "Debe Envíar un Numero de Solicitud Para Poder Continuar.";
                }
                else
                {

                    SolicitudGruas ob = context.SolicitudGruas.Where(t => t.ID_solicitud == desjson.ID_solicitud).FirstOrDefault();

                    ob.Numero_de_orden_del_servicio = desjson.Numero_de_orden_del_servicio;
                    ob.Fecha_hora_novedad = desjson.Fecha_hora_novedad;
                    ob.Tipo_novedad = desjson.Tipo_novedad;
                    ob.Placa_vehiculo_trasladar = desjson.Placa_vehiculo_trasladar;
                    ob.Placa_grua_Numero = desjson.Placa_grua_Numero;
                    ob.Tipo_documento_identificacion_conductor_grua = desjson.Tipo_documento_identificacion_conductor_grua;
                    ob.Numero_de_identificacion_conductor_grua = desjson.Numero_de_identificacion_conductor_grua;
                    ob.Placa_grua_Nueva = desjson.Placa_grua_Nueva;
                    ob.Tipo_Doc_Conductor_Grua_Nueva = desjson.Tipo_Doc_Conductor_Grua_Nueva;
                    ob.Numero_identificacion_conductor_grua_Nueva = desjson.Numero_identificacion_conductor_grua_Nueva;
                    ob.Fecha_hora_reasignacion = desjson.Fecha_hora_reasignacion;
                    ob.Parqueadero_destino = desjson.Parqueadero_destino;
                    ob.Observaciones_Novedad = desjson.Observaciones_Novedad;
                    ob.Fecha_hora_llegada_lugar_solicitud = desjson.Fecha_hora_llegada_lugar_solicitud;
                    ob.Estado = "REASIGNADA";
                    ob.Fecha_Cierre_Auto = horacol.AddMinutes(Convert.ToDouble(select.Horas));


                    Estados est = new Estados();
                    est.ID_solicitud = desjson.ID_solicitud;
                    est.Nombre = "REASIGNADA";
                    est.Observaciones = desjson.Observaciones_Novedad;
                    est.Fecha = horacol;
                    context.Estados.Add(est);
                    context.SaveChanges();

                    return "Novedad Registrada Satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                return "Error " + ex;
                throw;
            }
        }

    }
}