angular.module('solagents', [])
    .controller('TopeAgentesCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        consultartopeac();

        function consultartopeac() {
            var res = solgruasservices.consultartopeac();
            res.success(function (data, status, headers, config) {
                $scope.listtope = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.Actualizar = function () {

            if ($scope.nsol == undefined || $scope.nsol == "") {
                growl.error("Debe digitar un numero de solicitud para poder modificar el tope.", { title: 'Error!' });
            } else {

                var res = solgruasservices.actparams($scope.nsol);
                res.success(function (data, status, headers, config) {
                    growl.success("Tope Actualizado Correctamente", { title: 'Succes!' });
                    $scope.nsol = "";
                    consultartopeac();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

    }]);