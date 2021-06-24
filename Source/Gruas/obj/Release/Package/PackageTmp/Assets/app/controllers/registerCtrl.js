angular.module('register', [])
    .controller('registerCtrl', ['$rootScope', '$scope', '$http', 'growl', 'registerServics', function ($rootScope, $scope, $http, growl, registerServics) {

        $scope.listUsers = [];
        $scope.listRoles = {};
        $scope.IdRol = "";
        $scope.IsActivo = true;
        $scope.menus = {};
        $scope.menusfilter = {};
        $scope.menuselect = "-1";

        $scope.userselected = "";
        $scope.usernameselected = "";
        $scope.TipoDoc = "CC";
        $scope.listfilter = {};

        $scope.agentebool = true;

        GetMenus();

        $scope.tpuser = true;
        $scope.Entidad = "Policía de tránsito";
        $("#rdage").prop("checked", true);

        $scope.rdage = function () {
            $scope.tpuser = true;
            $scope.Entidad = "Policía de tránsito";
            $scope.agentebool = true;
        };

        $scope.rdgen = function myfunction() {
            $scope.tpuser = false;
            $scope.Entidad = "Secretaría Distrital de Movilidad";
            $scope.agentebool = false;
            $scope.PlacaAgente = "";
        }


        $scope.ChangePass = function () {
            $http.post('/api/Account/ChangePass', $scope.listfilter)
                .success(function (data, status, headers, config) {
                    $scope.successMessage = "Contraseña Actualizada Correctamente";
                    growl.success($scope.successMessage, { title: 'Success!' });

                    $scope.listfilter.OldPassword = "";
                    $scope.listfilter.NewPassword = "";
                    $scope.listfilter.ConfirmPassword = "";

                    $('#modalpass').modal('hide');
                })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.updatereg = function () {
            if ($scope.listfilter.LockoutEnabled == "1") {
                $scope.listfilter.LockoutEnabled = true;
            } else {
                $scope.listfilter.LockoutEnabled = false;
            }
            $http.post('/api/Account/updatereg', $scope.listfilter)
                .success(function (data, status, headers, config) {
                    $scope.successMessage = data;
                    growl.success($scope.successMessage, { title: 'Success!' });
                                        
                    $('#modaledit').modal('hide');
                    GetUsers();
                })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });


        }

        $scope.register = function () {
            if ($scope.firstname == null || $scope.firstname == "") {
                growl.error('Debe digitar un nombre y apellido para poder guardar', { title: 'Error!' });
            } else if ($scope.Email == null || $scope.Email == "") {
                growl.error('Debe digitar un email para poder guardar', { title: 'Error!' });
            } else if ($scope.Entidad == null || $scope.Entidad == "") {
                growl.error('Debe digitar una entidad para poder guardar', { title: 'Error!' });
            } else if ($scope.NumDoc == null || $scope.NumDoc == "") {
                growl.error('Debe digitar un numero de documento para poder guardar', { title: 'Error!' });
            } else if ($scope.exp == null || $scope.exp == "") {
                growl.error('Debe digitar un tiempo de expiración de contraseña para poder guardar', { title: 'Error!' });
            }else if (($scope.PlacaAgente == "" || $scope.PlacaAgente == null) && $("#rdage").is(':checked')) {
                growl.error('Debe digitar un placa de agente para poder guardar', { title: 'Error!' });
            }else if ($scope.IdRol == "") {
                growl.error('Debe seleccione un rol para poder guardar', { title: 'Error!' });
            } else {

                var params = {
                    firstName: $scope.firstname,
                    Email: $scope.Email,
                    Password: $scope.password1,
                    ConfirmPassword: $scope.password2,
                    LockoutEnabled: $scope.IsActivo,
                    PhoneNumber: $scope.Telefono,
                    TipoDocumento: $scope.TipoDoc,
                    NumeroDocumento: $scope.NumDoc,
                    PlacaAgente: $scope.PlacaAgente,
                    NombreJefe: $scope.NombreJefe,
                    TelefonoJefe: $scope.TelefonoJefe,
                    Entidad: $scope.Entidad,
                    Agente: $scope.agentebool,
                    DiasExpiracion: $scope.exp
                };
                $http.post('/api/Account/Register', params)
                    .success(function (data, status, headers, config) {
                        $scope.successMessage = "Registration Complete. ";


                        $scope.firstname = "";
                        $scope.Email = "";
                        $scope.password1 = "";
                        $scope.password2 = "";
                        $scope.IsActivo = "";
                        $scope.Telefono = "";
                        $scope.TipoDoc = "CC";
                        $scope.NumDoc = "";
                        $scope.PlacaAgente = "";
                        $scope.NombreJefe = "";
                        $scope.TelefonoJefe = "";
                        $scope.exp = "";


                        growl.success($scope.successMessage, { title: 'Success!' });
                        $scope.registerrol(data, $scope.IdRol)
                    })
                    .error(function (data, status, headers, config) {
                        console.log(data);
                        if (data == "Passwords must have at least one non letter or digit character. Passwords must have at least one lowercase ('a'-'z'). Passwords must have at least one uppercase ('A'-'Z').") {
                            growl.error("La contraseña debe tener un caracter especial,debe contener una letra mayuscula de la ('A'-'Z') y minusculas de la ('A'-'Z'), debe tener un longitud de 8 caracteres.", { title: 'Error!' });
                        } else if (data == "Email address is already in use.") {
                            growl.error("El correo ya se encuentra en uso.", { title: 'Error!' });
                        } else {
                            growl.error(data, { title: 'Error!' });
                        }

                        
                    });
            }
        }


        $scope.registerReportes = function () {
            if ($scope.username == null) {
                growl.error('Digite todo los datos', { title: 'Error!' });
                return;
            }

            var params = {
                Name: $scope.name,
                Email: $scope.email,
                Password: $scope.password1,
                ConfirmPassword: $scope.password2
            };
            $http.post('/api/Account/RegisterReports', params)
                .success(function (data, status, headers, config) {
                    $scope.successMessage = "Registration Complete.";
                    growl.success($scope.successMessage, { title: 'Success!' });
                    $scope.getAllDespachadores();
                })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        GetUsers();

        function GetUsers() {
            var res = registerServics.GetUsers();

            res.success(function (data, status, headers, config) {
                $scope.listUsers = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data.message, { title: 'Error!' });
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

        function ObtenerRoles() {
            var res = registerServics.GetRoles();
            res.success(function (data, status, headers, config) {
                $scope.listRoles = data;

            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

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

        $scope.searchmenu = function (itm) {
            $("#myModaladmin").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })


            $scope.userselected = itm.Id;
            $scope.usernameselected = itm.Email;
        }


        function GetMenus() {
            var res = registerServics.GetMenus();
            res.success(function (data, status, headers, config) {
                $scope.menus = data;

                ObtenerRoles()
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.searchmenufilter = function () {
            var res = registerServics.GetMenusfilter($scope.menuselect);
            res.success(function (data, status, headers, config) {
                $scope.menusfilter = data;
                $scope.MenusUsers();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.MenusUsers = function () {
            var res = registerServics.MenusUsers($scope.userselected);
            res.success(function (data, status, headers, config) {

                $scope.menuser = data;

                for (var i = 0; i < $scope.menus.length; i++) {
                    $("#ch" + [i]).prop('checked', false);
                }

                $scope.menuser.forEach(function (element) {

                    for (var i = 0; i < $scope.menusfilter.length; i++) {
                        if (element.IdMenu == $("#ch" + [i]).val()) {
                            $("#ch" + [i]).prop('checked', true);
                        }
                    }
                });

            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.savechanges = function () {

            $scope.listmenuselected = [];

            for (var i = 0; i < $(".chelement").length; i++) {
                if ($("#ch" + [i]).prop('checked') == true) {
                    $scope.listmenuselected[i] = $("#ch" + [i]).val();
                }
            }

            var res = registerServics.savechanges($scope.listmenuselected, $scope.userselected, $scope.menuselect);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

        }

        $scope.salir = function () {
            $scope.menuselect = "-1";
            $scope.menusfilter = {};
            $('#myModaladmin').modal('hide');
        }


        $scope.openedit = function (it) {
            $scope.listfilter = angular.copy(it);

            if ($scope.listfilter.LockoutEnabled == true) {
                $scope.listfilter.LockoutEnabled = "1";
            } else {
                $scope.listfilter.LockoutEnabled = "0";
            }
            

            $("#modaledit").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        };


        $scope.modalpass = function (it) {
            $scope.listfilter = it;          

            $("#modalpass").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        };
    }]);