using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WSG
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWSG
    {
        /// <summary>
        /// Respuesta de si se Asgina o Cancela una Solicitud de Grúa.
        /// WSRespuestaSol
        /// </summary>
        /// <param JSON=Data de Solicitud de Grúas></param>
        /// <returns></returns>
        [OperationContract]
        dynamic WSRespuestaSol(string JSON);

        /// <summary>
        /// Respuesta de Atención del Servicio de gruas.
        /// WSRegistrarAt
        /// </summary>
        /// <param JSON=Data con Información del Servicio.></param>
        /// <returns></returns>
        [OperationContract]
        dynamic WSRegistrarAt(string JSON);

        /// <summary>
        /// Para Registrar Novedades de las Grúas durante el servicio.
        /// WSRegistrarNovedad
        /// </summary>
        /// <param JSON= Data para Registrar Novedad en el Servicio de Grúas.></param>
        /// <returns></returns>
        [OperationContract]
        dynamic WSRegistrarNovedad(string JSON);
    }
}
