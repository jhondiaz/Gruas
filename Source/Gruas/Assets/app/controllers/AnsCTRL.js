angular.module('ans', [])
    .controller('AnsCTRL', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        $scope.Idanscons = "";
        $scope.IdInfcons = "";

        anscons();

        function anscons() {
            var res = solgruasservices.anscons();
            res.success(function (data, status, headers, config) {
                $scope.listestday = data;
                $scope.Idanscons = data.Id;
                horainfcons();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        function horainfcons() {
            var res = solgruasservices.horainfcons();
            res.success(function (data, status, headers, config) {
                $scope.listtime = data;
                $scope.IdInfcons = data.Id;
                listmails();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        function listmails() {
            var res = solgruasservices.listmails();
            res.success(function (data, status, headers, config) {
                $scope.listmails = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.Actualizar = function (val) {

            if ($scope.horastope == "" || $scope.horastope == undefined) {
                growl.error("Debe digitar un valor para poder guardar", { title: 'Error!' });
            } else {

                var res = solgruasservices.actans($scope.Idanscons,$scope.horastope);
                res.success(function (data, status, headers, config) {
                    growl.success("Numero de Minutos Actualizado", { title: 'Succes!' });
                    $scope.horastope = "";
                    anscons();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }


        $scope.ActualizarHora = function () {

            var d = new Date($scope.horagen);
            var x = document.getElementById("horagen").value;

            if ($scope.horagen == "" || $scope.horagen == undefined) {
                growl.error("Debe digitar un valor para poder guardar", { title: 'Error!' });
            } else {                

                var res = solgruasservices.ActualizarHora($scope.IdInfcons, x);
                res.success(function (data, status, headers, config) {
                    growl.success(data, { title: 'Succes!' });
                    $scope.horagen = "";
                    anscons();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

        $scope.addmail = function () {
            if ($scope.mailsadd == "" || $scope.mailsadd == undefined) {
                growl.error("Debe digitar un corro para poder guardar", { title: 'Error!' });
            } else {

                var res = solgruasservices.addmail($scope.mailsadd);
                res.success(function (data, status, headers, config) {
                    growl.success("Correo Agregado Correctamente", { title: 'Succes!' });
                    $scope.mailsadd = "";
                    listmails();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

        $scope.deletemail = function (itm) {
            var res = solgruasservices.deletemail(itm);
            res.success(function (data, status, headers, config) {
                growl.success("Correo Eliminado Correctamente", { title: 'Succes!' });
                listmails();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }
    }]);