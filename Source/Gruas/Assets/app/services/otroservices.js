app.service("otrosServics", ["$http", function ($http) {


    this.getAllCitys = function () {

        var config = {
            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Citys/getAllCitys', config);
    }
   

    this.getUserAppByEmail = function (Email) {

        var config = {
            params: { email: Email },
            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Bussines/GetUserAppByEmail', config);
    }


}]);