app.service("ReportsServics", ["$http", function ($http) {

    /*CAJA VALES*/
    this.setGenerarReports = function (idUser, startDate, endDate) {

        var config = {
            params: {
                IdUser: idUser,
                StartDate: startDate,
                EndDate: endDate,
            },

            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Reports/SetReportsCajaByDate', config);
    }
    this.setGenerarReportsPuc = function (idUser, startDate, endDate) {

        var config = {
            params: {
                IdUser: idUser,
                StartDate: startDate,
                EndDate: endDate,
            },

            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Reports/SetReportsCajaByDatePuc', config);
    }

    /*CAJA GENERAL*/
    this.setGenerarReportsGeneral = function (idUser, startDate, endDate) {

        var config = {
            params: {
                IdUser: idUser,
                StartDate: startDate,
                EndDate: endDate,
            },

            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Reports/SetReportsCajaGeneralByDate', config);
    }
    this.setGenerarReportsPucGeneral = function (idUser, startDate, endDate) {

        var config = {
            params: {
                IdUser: idUser,
                StartDate: startDate,
                EndDate: endDate,
            },

            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Reports/SetReportsCajaGeneralByDatePuc', config);
    }

    this.setGenerarReportsAllPucGeneral = function (idUser, startDate, endDate) {

        var config = {
            params: {
                IdUser: idUser,
                StartDate: startDate,
                EndDate: endDate,
            },

            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Reports/SetReportsAllCajaGeneralByDatePuc', config);
    }


}]);