angular.module('signIn', ['ngCookies', 'ngAnimate', 'angular-loading-bar', 'angular-growl'])
    .controller('signInCtrl', ['$scope', '$rootScope', '$http', '$cookies', '$cookieStore', '$location', '$routeParams', 'cfpLoadingBar', 'growl',
        function ($scope, $rootScope, $http, $cookies, $cookieStore, $location, $routeParams, cfpLoadingBar, growl) {
            $scope.message = $routeParams.message;
            if (window.location.hostname == 'localhost') {
                $scope.password = "Javier123$$";
                $scope.username = "jreyes@webnet.com.co";
            }

            $scope.valDate = function () {
                $scope.dislog = true;
                var config = {
                    Email: $scope.username
                };

                $http.post('/api/Account/ValAcceso', config).success(function (data) {
                    if (data == "La Contraseña Expiro") {
                        growl.success(data, { title: 'Succes!' });
                        $scope.signIn();
                    } else {
                        $scope.signIn();
                    }

                });

            }

            $scope.signIn = function () {

                $scope.showMessage = false;
                var params = "grant_type=password&username=" + $scope.username + "&password=" + $scope.password;
                $http({
                    url: '/Token',
                    method: "POST",
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    data: params
                })
                    .success(function (data, status, headers, config) {

                        $scope.dislog = false;
                        $http.defaults.headers.common.Authorization = "Bearer " + data.access_token;
                        $http.defaults.headers.common.RefreshToken = data.refresh_token;

                        $cookieStore.put('_Token', data.access_token);


                        var config = {
                            RoleId: data.Role,
                            IdUser: data.id
                        };
                        $http.post('/api/Registers/ObMenusRol', config).success(function (data) {
                            if (data == 0) {
                                growl.error('El rol se encuentra inactivo, contacte a el administrador.', { title: 'Error!' });
                            } else {
                                localStorage.setItem("_Routes", JSON.stringify(data));
                                $rootScope.ListMenuLog = data;
                                angular.forEach(data, function (route) {
                                    angular.forEach(route.SubMenu, function (Subroute) {
                                        $routeProviderReference.when(Subroute.AUrl, { templateUrl: Subroute.ATemplateUrl, controller: Subroute.AController });
                                    });
                                });
                            }
                        })

                        window.location = '#/principal';

                        //switch (data.Role) {
                        //    case 'ADMIN': window.location = '#/register'; break;
                        //    case 'REPORTS': window.location = '#/reportsallcajageneralpuc'; break;

                        //    default: window.location = '#/signin'; growl.error('El Usuario no tiene Roles Asignado', { title: 'Error!' });
                        //}


                        growl.success('Bienvenido.', { title: 'Info!' });

                    })
                    .error(function (data, status, headers, config) {
                        $scope.message = data.error_description.replace(/["']{1}/gi, "");
                        growl.error($scope.message, { title: 'Error!' });
                        $scope.dislog = false;
                    });
            }



            $scope.changepassbyemail = function (data) {

                var config = {
                    Email: data
                };

                $http.post('/api/Account/ResetPass', config).success(function (data) {
                    growl.success(data, { title: 'Succes!' });

                    $("#modaledit").modal('hide');
                }).error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

            }


            $scope.openmode = function () {
                $("#modaledit").modal({
                    escapeClose: true,
                    clickClose: false,
                    showClose: true
                })
            }



            $scope.TipoDoc = "CC";
            $scope.Entidad = "Policía de tránsito";
            $("#rdage").prop("checked", true);
            $scope.tpuser = true;
            $scope.agentebool = true;

            $scope.rdage = function () {
                $scope.tpuser = true;
                $scope.Entidad = "Policía de tránsito";
                $scope.agentebool = true;
            };

            $scope.rdgen = function myfunction() {
                $scope.tpuser = false;
                $scope.Entidad = "Secretaría Distrital de Movilidad";
                $scope.agentebool = false;
            }

            $scope.validaremail = function (mail) {
                return /^\w+([\.\+\-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/.test(mail);
            }

            $scope.RegRequest = function () {
                if ($scope.validaremail($scope.Email)) {

                    var config = {
                        firstname: $scope.firstname,
                        Email: $scope.Email,
                        Entidad: $scope.Entidad,
                        PhoneNumber: $scope.Telefono,
                        TipoDocumento: $scope.TipoDoc,
                        NumeroDocumento: $scope.NumDoc,
                        PlacaAgente: $scope.PlacaAgente,
                        NombreJefe: $scope.NombreJefe,
                        TelefonoJefe: $scope.TelefonoJefe,
                        Agente: $scope.agentebool
                    };

                    $http.post('/api/Registers/RegRequest', config).success(function (data) {
                        growl.success(data, { title: 'Succes!' });


                        $scope.firstname = "";
                        $scope.Email = "";
                        $("#rdage").prop("checked", true);
                        $scope.Entidad = "Policía de tránsito";
                        $scope.Telefono = "";
                        $scope.TipoDoc = "CC";
                        $scope.NumDoc = "";
                        $scope.PlacaAgente = "";
                        $scope.NombreJefe = "";
                        $scope.TelefonoJefe = "";


                        $scope.showdata();
                    }).error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
                } else {
                    growl.error("Debe digitar un correo valido.", { title: 'Error!' });
                }
            }

        }]);