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

namespace WSG
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class WSG : IWSG
    {
        Context context = new Context();
        static TimeZoneInfo horazone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        DateTime horacol = TimeZoneInfo.ConvertTime(DateTime.Now, horazone);

        /// <summary>
        /// Respuesta de si se Asgina o Cancela una Solicitud de Grúa.
        /// WSRespuestaSol
        /// </summary>
        /// <param JSON=Data de Solicitud de Grúas></param>
        /// <returns></returns>
        public dynamic WSRespuestaSol(string JSON)
        {

            try
            {
                var select = context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());
                CanGruasSolicitudes objtvs = new CanGruasSolicitudes();


                if (desjson.Numero_de_orden_del_servicio == "" || desjson.Numero_de_orden_del_servicio == null)
                {
                    return "Mensaje de error, campo No de Orden de servicio es  obligatorio.";
                }
                else if (desjson.Placa_grua_Numero == "" || desjson.Placa_grua_Numero == null)
                {
                    return "Mensaje de error, campo Placa_grua_Numero es  obligatorio.";
                }
                else if (desjson.Tipo_documento_identificacion_conductor_grua == "" || desjson.Tipo_documento_identificacion_conductor_grua == null)
                {
                    return "Mensaje de error, campo Tipo_documento_identificacion_conductor_grua es obligatorio.";
                }
                else if (desjson.Numero_de_identificacion_conductor_grua == "" || desjson.Numero_de_identificacion_conductor_grua == null)
                {
                    return "Mensaje de error, campo Numero_de_identificacion_conductor_grua es  obligatorio.";
                }
                else
                {

                    SolicitudGruas ob = context.SolicitudGruas.Where(t => t.Numero_de_orden_del_servicio == desjson.Numero_de_orden_del_servicio).FirstOrDefault();

                    if (ob ==  null)
                    {
                        return "El numero de la orden no existe, favor validar.";
                    }

                    ob.Fecha_y_hora_solicitud_servicio_res = desjson.Fecha_y_hora_solicitud_servicio_res;
                    ob.Placa_grua_Numero = desjson.Placa_grua_Numero;
                    ob.Tipo_documento_identificacion_conductor_grua = desjson.Tipo_documento_identificacion_conductor_grua;
                    ob.Numero_de_identificacion_conductor_grua = desjson.Numero_de_identificacion_conductor_grua;

                    var cons = context.SolicitudGruas.FirstOrDefault(t => t.Numero_de_orden_del_servicio == desjson.Numero_de_orden_del_servicio);

                    var consulta = context.CanGruasSolicitudes.FirstOrDefault(t => t.IdSol == cons.ID_solicitud);


                    if (consulta  == null)
                    {
                        foreach (var item in desjson.Tipo_de_Grua)
                        {
                            if (item != null)
                            {
                                var tgua = context.TipoGruas.FirstOrDefault(t => t.Id == item.Tipo_de_Grua);

                                if (item.Tipo_de_Grua == "" || item.Tipo_de_Grua == null)
                                {
                                    return "Mensaje de error, campo Tipo_de_Grua es obligatorio.";
                                }
                                else if (item.Numero_de_gruas_solicitadas_por_tipo_de_grua == 0)
                                {
                                    return "Mensaje de error, campo No de Gruas Solicitado es obligatorio.";
                                }
                                if (tgua == null)
                                {
                                    return "Mensaje de error, tipo grua no existe.";
                                }
                                objtvs.IdSol = cons.ID_solicitud;
                                objtvs.TipoGrua = item.Tipo_de_Grua;
                                objtvs.Cantidad = item.Numero_de_gruas_solicitadas_por_tipo_de_grua.ToString();

                                context.CanGruasSolicitudes.Add(objtvs);
                            }

                        }

                    }
                    else
                    {
                        return "La solicitud ya tiene registrada una atención.";
                    }

                    if (cons.Causa_de_inmovilizacion == "2")
                    {
                        ob.Parqueadero_destino = "PARQUEADERO TRANSITORIO";
                    }

                    context.SaveChanges();
                    return "Respuesta Recibida Satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return "Error " + ex.Message + ", " + ex.InnerException.Message;
                }
                else
                {
                    return "Error " + ex.Message;
                }
                throw ex;
            }
        }

        /// <summary>
        /// Respuesta de Atención del Servicio de gruas.
        /// WSRegistrarAt
        /// </summary>
        /// <param JSON=Data con Información del Servicio.></param>
        /// <returns></returns>
        public dynamic WSRegistrarAt(string JSON)
        {
            try
            {

                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());

                if (desjson.Numero_de_orden_del_servicio == "" || desjson.Numero_de_orden_del_servicio == null)
                {
                    return "Mensaje de error, campo No de Orden de servicio es  obligatorio.";
                }
                else if (desjson.Placa_grua_Numero == "" || desjson.Placa_grua_Numero == null)
                {
                    return "Mensaje de error, campo No de Placa de la Grúa es obligatorio.";
                }
                else
                {

                    SolicitudGruas ob = context.SolicitudGruas.FirstOrDefault(t => t.Numero_de_orden_del_servicio == desjson.Numero_de_orden_del_servicio);

                    if (ob == null)
                    {
                        return "El numero de la orden no existe, favor validar.";
                    }

                    //ob.Numero_de_orden_del_servicio = desjson.Numero_de_orden_del_servicio;
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
                    est.ID_solicitud = ob.ID_solicitud;
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
                if (ex.InnerException != null)
                {
                    return "Error " + ex.InnerException.Message;
                }
                else
                {
                    return "Error " + ex.Message;
                }
                throw;
            }
        }

        /// <summary>
        /// Para Registrar Novedades de las Grúas durante el servicio.
        /// WSRegistrarNovedad
        /// </summary>
        /// <param JSON= Data para Registrar Novedad en el Servicio de Grúas.></param>
        /// <returns></returns>
        public dynamic WSRegistrarNovedad(string JSON)
        {
            try
            {
                var select = context.ConfigCierreAutos.FirstOrDefault();
                var JsonObject = JsonConvert.SerializeObject(JSON);
                var objectdes = JsonConvert.DeserializeObject(JsonObject);
                SolicitudGruasParams desjson = JsonConvert.DeserializeObject<SolicitudGruasParams>(objectdes.ToString());

                if (desjson.Numero_de_orden_del_servicio == "" || desjson.Numero_de_orden_del_servicio == null)
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
                if (ex.InnerException != null)
                {
                    return "Error " + ex.InnerException.Message;
                }
                else
                {
                    return "Error " + ex.Message;
                }
                
                throw;
            }
        }
    }
}