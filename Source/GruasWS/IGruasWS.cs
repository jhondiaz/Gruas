using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GruasWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGruasWS
    {

        //[OperationContract]
        //string GetData(string val1);

        [OperationContract]
        dynamic WSRespuestaSol(string JSON);

        [OperationContract]
        dynamic WSRegistrarAt(string JSON);
        // TODO: Add your service operations here

        [OperationContract]
        dynamic WSRegistrarNovedad(string JSON);
        // TODO: Add your service operations here
    }
}
