angular.module('solicituduser', [])
    .controller('SolicitudesUsuarioCtrl', ['$rootScope', '$scope', '$http', 'growl', 'registerServics', function ($rootScope, $scope, $http, growl, registerServics) {

        $scope.IsActivo = true;
        $scope.IdRol = "";
        GetSolicitudes();
        $scope.placacopy = "";
        $scope.filter = [];

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
            $scope.placacopy = $scope.filter.PlacaAgente;

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
            $scope.filter.PlacaAgente = $scope.placacopy;
        };

        $scope.rdgen = function myfunction() {
            $scope.tpuser = false;
            $scope.filter.Entidad = "Secretaría Distrital de Movilidad";
            $scope.agentebool = false;
            $scope.filter.PlacaAgente = "";
        }


        $scope.registeruser = function (idreq) {

            if ($scope.filter.firstName == undefined || $scope.filter.firstName == "") {
                growl.error('Debe digitar un nombre y apellido para poder guardar', { title: 'Error!' });
            } else if ($scope.filter.Email == undefined || $scope.filter.Email == "") {
                growl.error('Debe digitar un email para poder guardar', { title: 'Error!' });
            } else if ($scope.filter.Entidad == undefined || $scope.filter.Entidad == "") {
                growl.error('Debe digitar una entidad para poder guardar', { title: 'Error!' });
            } else if ($scope.filter.NumeroDocumento == undefined || $scope.filter.NumeroDocumento == "") {
                growl.error('Debe digitar un numero de documento para poder guardar', { title: 'Error!' });
            } else if ($scope.filter.DiasExpiracion == undefined || $scope.filter.DiasExpiracion == "") {
                growl.error('Debe digitar un tiempo de expiración de contraseña para poder guardar', { title: 'Error!' });
            } else if ($scope.IdRol == "") {
                growl.error("Debe seleccione un rol para poder guardar", { title: 'Error!' });
            } else if (($scope.filter.PlacaAgente == "" || $scope.filter.PlacaAgente == null) && $("#rdage").is(':checked')) {
                growl.error('Debe digitar un placa de agente para poder guardar', { title: 'Error!' });
            } else {

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
                $scope.placacopy = "";
                $http.post('/api/Account/Register', $scope.filter)
                    .success(function (data, status, headers, config) {
                        $scope.successMessage = "Registration Complete. ";
                        growl.success($scope.successMessage, { title: 'Success!' });
                        $scope.registerrol(data, $scope.IdRol)

                        $('#modalreguser').modal('hide');
                    })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                        $('#modalreguser').modal('hide');
                    });
            }

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