angular.module('createmenus', [])
    .controller('CreateMenusCtrl', ['$scope', 'MenusServices', '$http', 'growl', '$filter', function ($scope, MenusServices, $http, growl, $filter) {

        $scope.menua = "-1";
        $scope.menus = {};
        $scope.menusall = {};
        $scope.listmenus = {};

        GetMenus();


        $("#rdmenu").prop("checked", true);


        $scope.rdmenu = function () {
            $scope.menua = "-1";
            $scope.modulo = "";
            $scope.ruta = "";
            $scope.Controlador = "";

            $(".submenudis").css('display', 'none');
        };


        $scope.rdsubmenu = function () {
            $(".submenudis").css('display', 'block');
        };


        function GetMenus() {
            var res = MenusServices.GetMenus();
            res.success(function (data, status, headers, config) {
                $scope.menus = data;
                GetMenusall();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        function GetMenusall() {
            var res = MenusServices.GetMenusall();
            res.success(function (data, status, headers, config) {
                $scope.menusall = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.openmodaldevices = function () {
            $("#myModal").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        }

        $scope.selected = function (valor) {
            $('#myModal').modal('hide');
            $scope.icono = valor;
        }


        $("#myInput").keyup(function () {
            var input, filter, table, tr, td, i;
            input = document.getElementById("myInput");
            filter = input.value.toUpperCase();
            table = document.getElementById("myTable");
            tr = table.getElementsByTagName("tr");
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        })

        $scope.validar = function () {
            if ($("#rdmenu").prop("checked") == true) {
                if ($scope.name == undefined || $scope.name == "") {
                    growl.error('Debe Digitar un Nombre de Menú', { title: 'Error!' });
                } else if ($scope.orden == undefined || $scope.orden == "") {
                    growl.error('Debe Digitar un Orden', { title: 'Error!' });
                } else if ($scope.icono == undefined || $scope.icono == "") {
                    growl.error('Debe Seleccionar un Icono', { title: 'Error!' });
                } else {
                    $scope.putMenusnew();
                }

            } else if ($("#rdsubmenu").prop("checked") == true) {
                if ($scope.name == undefined || $scope.name == "") {
                    growl.error('Debe Digitar un Nombre de Menú', { title: 'Error!' });
                } else if ($scope.orden == undefined || $scope.orden == "") {
                    growl.error('Debe Digitar un Orden', { title: 'Error!' });
                } else if ($scope.icono == undefined || $scope.icono == "") {
                    growl.error('Debe Seleccionar un Icono', { title: 'Error!' });
                } else if ($scope.menua == "-1") {
                    growl.error('Debe Seleccionar un Menu a Asociar', { title: 'Error!' });
                } else if ($scope.modulo == undefined || $scope.modulo == "") {
                    growl.error('Debe Digitar un Modulo', { title: 'Error!' });
                } else if ($scope.ruta == undefined || $scope.ruta == "") {
                    growl.error('Debe Digitar la Ruta del Proyecto', { title: 'Error!' });
                } else if ($scope.Controlador == undefined || $scope.Controlador == "") {
                    growl.error('Debe Digitar un Controlador', { title: 'Error!' });
                } else {
                    $scope.putMenusnew();
                }
            }
        }

        $scope.putMenusnew = function () {

            var rdmenu = "";
            $scope.isDisabled = true;

            if ($("#rdmenu").prop("checked") == true) {
                rdmenu = "menu";
            } else if ($("#rdsubmenu").prop("checked") == true) {
                rdmenu = "submenu";
            }

            var res = MenusServices.putMenusnew($scope.name, $scope.orden, $scope.icono, $scope.menua, $scope.Controlador, $scope.modulo, $scope.ruta, rdmenu);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Succes!' });

                $scope.isDisabled = false;
                $scope.name = "";
                $scope.orden = "";
                $scope.icono = "";
                $scope.menua = "-1";
                $scope.modulo = "";
                $scope.ruta = "";
                $scope.Controlador = "";

                GetMenus()
                GetMenusall();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        };

    }]);