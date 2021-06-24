app.service("solgruasservices", ["$http", function ($http) {


    this.infcodes = function () {
        return $http.post('/api/SolicitudGruas/infcodes');
    }


    this.searchagent = function (pl, Tipo) {

        config = {
            PlacaAgente: pl,
            Tipo: Tipo,
            NumeroDocumento: pl
        }

        return $http.post('/api/SolicitudGruas/searchagent', config);
    }


    this.saveinfgru = function (vl) {

        config = {
            Entidad: vl.userentidad,
            Id_Usuario: vl.iduser,
            Tipo_de_servicio_de_traslado: vl.tservicio,
            Causa_de_inmovilizacion: vl.causain,
            Codigo_de_infraccion: vl.CodInfracc,
            Direccion: vl.dirr,
            Barrio: vl.brr,
            Direccion_georreferencial: vl.magna,
            Localidad: vl.localidad,
            Tipo_Zona: 2,
            Tipo_de_vehiculo_a_inmovilizar: vl.tvehiculo,
            Tipo_de_Grua: vl.tgrua,
            Numero_de_gruas_solicitadas_por_tipo_de_grua: vl.numgruas,
            Estado: "SOLICITADA",
            CantVti: vl.tvh
        }

        return $http.post('/api/SolicitudGruas/saveinfgru', config);
    }


    this.buscarsolicitudesgr = function () {
        return $http.post('/api/SolicitudGruas/buscarsolicitudesgr');
    }


    this.actparams = function (val) {

        config = {
            Tope: val
        }

        return $http.post('/api/SolicitudGruas/actparams', config);
    }


    this.consultartopeac = function () {
        return $http.post('/api/SolicitudGruas/consultartopeac');
    }

    this.constope = function (val) {

        config = {
            Id: val
        }

        return $http.post('/api/SolicitudGruas/constope', config);
    }

    this.consultarnumgr = function () {
        return $http.post('/api/SolicitudGruas/consultarnumgr');
    }


    this.actnumgr = function (val) {

        config = {
            Conteo: val
        }

        return $http.post('/api/SolicitudGruas/actnumgr', config);
    }

    this.conscinmo = function () {
        return $http.post('/api/SolicitudGruas/conscinmo');
    }

    this.T_S_Translados = function () {
        return $http.post('/api/SolicitudGruas/T_S_Translados');
    }

    this.C_Inmovilizaciones = function (val) {

        config = {
            T_Translado: val
        }

        return $http.post('/api/SolicitudGruas/C_Inmovilizaciones', config);
    }

    this.Localidades = function () {
        return $http.post('/api/SolicitudGruas/Localidades');
    }

    this.SentidoViales = function () {
        return $http.post('/api/SolicitudGruas/SentidoViales');
    }

    this.TipoGruas = function () {
        return $http.post('/api/SolicitudGruas/TipoGruas');
    }

    this.TVehiculoInmovilizars = function () {
        return $http.post('/api/SolicitudGruas/TVehiculoInmovilizars');
    }

    this.cacelarservicio = function (id, causa, obs, ordserv) {

        config = {
            ID_solicitud: id,
            Causa_de_Cancelacion_de_Servicio: causa,
            ObsCancel: obs,
            Numero_de_orden_del_servicio: ordserv
        }
        return $http.post('/api/SolicitudGruas/cacelarservicio', config);
    }

    this.solicitudporid = function (val, est) {

        config = {
            ID_solicitud: val,
            Estado: est
        }

        return $http.post('/api/SolicitudGruas/solicitudporid', config);
    }

    this.consultarnumest = function () {
        return $http.post('/api/SolicitudGruas/consultarnumest');
    }

    this.anscons = function () {
        return $http.post('/api/SolicitudGruas/anscons');
    }

    this.horainfcons = function () {
        return $http.post('/api/SolicitudGruas/horainfcons');
    }

    this.actualizardiasexp = function (id, val) {

        config = {
            Id: id,
            Horas: val
        }
        return $http.post('/api/SolicitudGruas/actualizardiasexp', config);
    }

    this.actans = function (id, val) {

        config = {
            Id: id,
            Horas: val
        }

        return $http.post('/api/SolicitudGruas/actans', config);
    }

    this.ActualizarHora = function (id, val) {

        config = {
            Id: id,
            Horas: val
        }
        return $http.post('/api/SolicitudGruas/ActualizarHora', config);
    }

    this.searchreportroluser = function () {
        return $http.post('/api/SolicitudGruas/searchreportroluser');
    }

    this.ReportUserRoles = function () {
        return $http.post('/api/SolicitudGruas/ReportUserRoles');
    }


    this.listmails = function () {
        return $http.post('/api/SolicitudGruas/listmails');
    }

    this.addmail = function (val) {

        config = {
            Correo: val
        }

        return $http.post('/api/SolicitudGruas/addmail', config);
    }

    this.deletemail = function (val) {
        config = {
            Id: val.Id,
            Correo: val.Correo
        }
        return $http.post('/api/SolicitudGruas/elimemail', config);
    }

    this.reportsolagent = function (val) {

        config = {
            plagente: val.pl,
            finicio: val.fini,
            ffin: val.ffin,
            estadosol: val.est,
            CInmov: val.ci,
            TGrua: val.tg,
            TVehiculo: val.tv,
            MCacncelacion: val.mc
        }
        return $http.post('/api/SolicitudGruas/reportsolagent', config);
    }


    this.reportsol = function (val) {

        config = {
            plagente: val.pl,
            finicio: val.fini,
            ffin: val.ffin,
            estadosol: val.est,
            CInmov: val.ci,
            TGrua: val.tg,
            TVehiculo: val.tv,
            MCacncelacion: val.mc
        }
        console.log(config);
        return $http.post('/api/SolicitudGruas/reportsol', config);
    }

    this.numsolaagente = function (val) {

        config = {
            fini: val.fini,
            ffin: val.ffin,
            Estado: val.est,
        }
        return $http.post('/api/SolicitudGruas/numsolaagente', config);
    }

    this.numsolaagente = function (val) {

        config = {
            fini: val.fini,
            ffin: val.ffin,
            Estado: val.est,
        }
        return $http.post('/api/SolicitudGruas/numsolaagente', config);
    }

    this.timesol = function (val) {

        config = {
            fini: val.fini,
            ffin: val.ffin,
        }
        return $http.post('/api/SolicitudGruas/timesol', config);
    }

    this.constvxid = function (Id) {

        config = {
            Id: Id
        }
        return $http.post('/api/SolicitudGruas/constvxid', config);
    }

    this.searchgrubyid = function (Id) {

        config = {
            Id: Id
        }
        return $http.post('/api/SolicitudGruas/searchgrubyid', config);
    }

    this.SearchGrua = function (Id) {

        config = {
            nroOrden: Id
        }

        return $http.post('/api/SolicitudGruas/SearchGrua', config);
    }

    this.SearchGruaByCoordenadas = function (data) {

        config = {
            strCoordenadas: data
        }

        return $http.post('/api/SolicitudGruas/SearchGruaByCoordenadas', config);
    }


}]);