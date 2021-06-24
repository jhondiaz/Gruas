angular.module('solicituduser', [])
    .controller('SolicitudesUsuarioCtrl', ['$rootScope', '$scope', '$http', 'growl', 'registerServics', function ($rootScope, $scope, $http, growl, registerServics) {

        $scope.IsActivo = true;
        $scope.IdRol = "";
        GetSolicitudes();

        function GetSolicitudes() {
            var res = registerServics.GetSolicitudes();
            res.success(function (data, status, headers, config) {
                $scope.listUsersSol = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.openmod = function (itm) {

            $scope.filter = angular.copy(itm);;

            if (itm.Agente == 1) {
                $("#rdage").prop("checked", true);
            } else {
                $("#rdgen").prop("checked", true);                
            }
            
            $("#modalreguser").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        }

        $scope.tpuser = true;
        $scope.rdage = function () {
            $scope.tpuser = true;
            $scope.filter.Entidad = "Policía de tránsito";
            $scope.agentebool = true;
        };

        $scope.rdgen = function myfunction() {
            $scope.tpuser = false;
            $scope.filter.Entidad = "Secretaría Distrital de Movilidad";
            $scope.agentebool = false;
        }


        $scope.registeruser = function (idreq) {

            if ($("#rdage").prop("checked")) {
                $scope.filter.Agente = true;
            } else {
                $scope.filter.Agente = false;
            }

            if ($scope.agentebool == false) {
                $scope.filter.PlacaAgente = null;
            }

            $scope.filter.LockoutEnabled = $("#chactv").is(':checked');
            $scope.filter.Tipe = "ReqUser";
            $scope.filter.IdSolicitud = idreq;

            $http.post('/api/Account/Register', $scope.filter)
                .success(function (data, status, headers, config) {
                    $scope.successMessage = "Registration Complete. ";
                    growl.success($scope.successMessage, { title: 'Success!' });
                    $scope.registerrol(data, $scope.IdRol)

                    $('#modalreguser').modal('hide');
                })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

        }

        function ObtenerRoles() {
            var res = registerServics.GetRoles();
            res.success(function (data, status, headers, config) {
                $scope.listRoles = data;

            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.openmodal = function () {
            $("#myModalSubProject").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
            ObtenerRoles();
        };

        $scope.obtenervalorsubproj = function (id, nombre) {
            $scope.rol = nombre;
            $scope.IdRol = id;
            $('#myModalSubProject').modal('hide');
        };

        $scope.registerrol = function (UserId, RoleId) {
            var res = registerServics.registerrol(UserId, RoleId);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
                GetUsers();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.rechazar = function (itm) {
            var res = registerServics.RechazarReq(itm);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
                GetSolicitudes();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }
    }]);