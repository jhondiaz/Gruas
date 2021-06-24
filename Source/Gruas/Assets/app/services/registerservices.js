app.service("registerServics", ["$http", function ($http) {


    this.getAllDespachadores = function () {

        var config = {
            headers: { 'Accept': 'application/json' }
        };

        return $http.get('/api/Registers/getAllDespachadores', config);
    }

    this.GetUsers = function () {
        return $http.get('/api/Registers/GetUsers');
    }

    this.GetRoles = function () {
        return $http.get('/api/Registers/GetRoles');
    }

    this.registerrol = function (UserId, RoleId) {

        var config = {
            UserId: UserId,
            RoleId: RoleId
        };

        return $http.post('/api/Registers/registerrol', config);
    }

    this.GetMenus = function () {
        return $http.get('/api/Registers/GetMenus');
    }

    this.GetMenusfilter = function (Id) {

        var config = {
            Id: Id
        }

        console.log(config)

        return $http.post('/api/Registers/Menusfilter', config);
    }

    this.MenusUsers = function (IdUser) {

        var config = {
            IdUser: IdUser
        }

        return $http.post('/api/Registers/MenusUsers', config);
    }

    this.savechanges = function (ListMenu, Menu, MenuGeneral) {

        arr = $.grep(ListMenu, function (n) {
            return (n);
        });

        var config = {
            IdProject: arr.toString(),
            IdUser: Menu,
            IdMenu: MenuGeneral
        };

        return $http.post('/api/Registers/savechanges', config);
    }
   
    this.GetSolicitudes = function () {
        return $http.get('/api/Registers/GetSolicitudes');
    }


    this.RechazarReq = function (itm) {

        var config = {
            Id: itm.Id
        };

        return $http.post('/api/Registers/RechazarReq', config);
    }
}]);