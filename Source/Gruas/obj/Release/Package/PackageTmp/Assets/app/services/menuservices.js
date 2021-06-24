app.service("MenusServices", ["$http", function ($http) {

    this.GetRoles = function () {
        return $http.get('/api/Registers/GetRoles')
    }

    this.GetMenus = function () {
        return $http.get('/api/Registers/GetMenus')
    }

    this.GetMenusall = function () {

        return $http.get('/api/Registers/GetMenusall')
    }

    this.setRol = function (Name, des) {

        var config = {
            Name: Name,
            Descripcion: des
        }

        return $http.post('/api/Menus/setRol', config)
    }

    this.putMenusnew = function (Menu, OrderMenu, Icon, IdSubMenu, AController, AUrl, ATemplateUrl, menu) {

        if (menu == "menu") {
            Menu = Menu;
            subMenu1 = null;
            IdSubMenu = null;
        } else {
            subMenu1 = Menu;
            Menu = null;
        }

        var config = {
            Id: "",
            Menu: Menu,
            OrderMenu: OrderMenu,
            Icon: Icon,
            IdSubMenu: IdSubMenu,
            SubMenu: subMenu1,
            ControllerName: null,
            ActionName: null,
            AUrl: AUrl,
            ATemplateUrl: ATemplateUrl,
            AController: AController,
            CreateDate: ""
        }

        return $http.post('/api/Menus/Menusnew', config)
    }


    this.GetFilterRol = function (Id) {

        var config = {
            Id: Id
        }

        return $http.post('/api/Menus/FilterRolGet', config)
    }


    this.registermenurol = function (IdsMenus, IdRol) {

        arr = $.grep(IdsMenus, function (n) {
            return n
        });

        var config = {
            IdRol: IdRol,
            IdsMenus: arr.toString()
        }

        return $http.post('/api/Menus/registermenurol', config)
    }

    this.inactivarelement = function (Id) {

        var config = {
            Id: Id
        }

        return $http.post('/api/Menus/inactivarelement', config)
    }

    this.actelement = function (Id) {

        var config = {
            Id: Id
        }

        return $http.post('/api/Menus/actelement', config)
    }


    this.actgen = function (Id, Name, Descripcion) {

        var config = {
            Id: Id,
            Name: Name,
            Descripcion: Descripcion
        }

        return $http.post('/api/Menus/actgen', config)
    }
}]);