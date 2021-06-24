angular.module('menus', [])
    .controller('MenusCtrl', ['$scope', 'MenusServices', '$http', 'growl', '$filter', function ($scope, MenusServices, $http, growl, $filter) {
        
        $scope.listRoles = [];
        $scope.listRolesFilter = [];
        $scope.listMenus = [];

        $scope.listMenusselected = [];
        $scope.rolselected = "";
        $scope.iddelete = "";
        $scope.accionin = true;
        $scope.REstado = "";

        ObtenerRoles();

        function ObtenerRoles() {
            var res = MenusServices.GetRoles();
            res.success(function (data, status, headers, config) {
                $scope.listRoles = data;
                ObtenerMenus();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        function ObtenerMenus() {
            var res = MenusServices.GetMenus();
            res.success(function (data, status, headers, config) {
                $scope.listMenus = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.ch = function (pos, val) {
            $(".rditm").removeClass('fa fa-check-circle');
            $(".rditm").addClass('fa fa-circle');

            $("#ch" + pos).removeClass('fa fa-circle');
            $("#ch" + pos).addClass('fa fa-check-circle');

            var res = MenusServices.GetFilterRol(val.Id);
            res.success(function (data, status, headers, config) {
                $scope.listRolesFilter = data;

                for (var i = 0; i < $(".chmen").length; i++) {
                    $("#chmen" + [i]).prop('checked', false);
                }


                $scope.listRolesFilter.forEach(function (element) {
                    for (var i = 0; i < $scope.listMenus.length; i++) {
                        if (element.IdMenu == $("#chmen" + [i]).val()) {
                            $("#chmen" + [i]).prop('checked', true);
                        }
                    }
                });

                if (val.Activo == false) {
                    $scope.accionin = true;
                } else {
                    $scope.accionin = false;
                }



            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        };

        $scope.registerrol = function () {

            if ($scope.rol == undefined || $scope.rol == "") {
                growl.error('DEBE DIGITAR UN ROL PARA PODER GUARDAR', { title: 'Error!' });
            } else if ($scope.des == undefined || $scope.des == "") {
                growl.error('DEBE DIGITAR UNA DESCRIPCIÓN DE ROL PARA PODER GUARDAR', { title: 'Error!' });
            }else {
                var res = MenusServices.setRol($scope.rol, $scope.des);
                res.success(function (data, status, headers, config) {
                    if (data == "EL ROL YA SE ENCUENTRA CREADO.") {
                        growl.error(data, { title: 'Error!' });
                        $scope.rol = "";
                        $scope.des = "";
                    } else {
                        growl.success('ROL CREADO EXITOSAMENTE.', { title: 'Succes!' });
                        $scope.rol = "";
                        $scope.des = "";
                        ObtenerRoles();
                    }
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        };


        $scope.registermenurol = function () {

            $scope.listMenusselected = [];

            for (var i = 0; i < $(".chmen").length; i++) {
                if ($("#chmen" + [i]).prop('checked') == true) {
                    $scope.listMenusselected[i] = $("#chmen" + [i]).val();
                }
            }

            for (var i = 0; i < $(".rditm").length; i++) {
                if ($("#ch" + [i]).hasClass('fa fa-check-circle')) {
                    $scope.rolselected = $("#ch" + [i]).attr('valor');
                }
            }



            var res = MenusServices.registermenurol($scope.listMenusselected, $scope.rolselected);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
                $scope.ch();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.inactivarelementos = function (itm) {
            $scope.iddelete = itm.Id;
            $(".Bloquear").css("display", "block");
        };


        $scope.inactivarelement = function () {
            var res = MenusServices.inactivarelement($scope.iddelete);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
                $(".Bloquear").css("display", "none");
                $scope.accionin = true;
                ObtenerRoles();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.actelement = function (itm) {
            var res = MenusServices.actelement(itm.Id);
            res.success(function (data, status, headers, config) {
                growl.success(data, { title: 'Success!' });
                $scope.accionin = true;
                ObtenerRoles();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }



        $scope.edit = function (val, itm) {

            $(".fg").css('padding', '0px !important')

            $("#" + val + itm + "").css('width', '100%');
            var maxw = $("#" + val + itm + "").width();

            $("#fg" + itm).css('max-width', maxw + "px");
            $("#" + val + itm).css('max-width', maxw + "px");
            $("#" + val + itm).css('border', '1px solid #ccc');
            $("#" + val + itm).css('background-color ', 'white');
            $("#" + val + itm).attr('contenteditable', 'true');

        };

        $scope.saveupdate = function (itm) {

            if ($("#name" + itm.Id)[0].outerText == "") {
                growl.error("El nombre no puede estar vacio", { title: 'Error!' });
            } else if ($("#des" + itm.Id)[0].outerText == "") {
                growl.error("La descripción no puede estar vacia", { title: 'Error!' });
            } else {
                $scope.valsupId = itm.Id;
                $scope.valsupName = $("#name" + itm.Id)[0].outerText;
                $scope.valsupDescripcion = $("#des" + itm.Id)[0].outerText;

                var res = MenusServices.actgen($scope.valsupId, $scope.valsupName, $scope.valsupDescripcion);
                res.success(function (data, status, headers, config) {
                    growl.success(data, { title: 'Success!' });               
                    ObtenerRoles();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }        
    }]);