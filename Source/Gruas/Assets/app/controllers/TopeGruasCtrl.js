angular.module('topegruas', [])
    .controller('TopeGruasCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        consultarnumgr();

        function consultarnumgr() {
            var res = solgruasservices.consultarnumgr();
            res.success(function (data, status, headers, config) {
                $scope.listgr = data.Conteo;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.Actualizar = function () {

            if ($scope.ngruas == undefined || $scope.ngruas == "") {
                growl.error("Debe digitar un numero de solicitud para poder modificar el tope.", { title: 'Error!' });
            } else {

                var res = solgruasservices.actnumgr($scope.ngruas);
                res.success(function (data, status, headers, config) {
                    growl.success("Tope Actualizado Correctamente", { title: 'Succes!' });
                    $scope.ngruas = "";
                    consultarnumgr();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

    }]);