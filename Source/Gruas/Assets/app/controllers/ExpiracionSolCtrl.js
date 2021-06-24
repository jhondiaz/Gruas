angular.module('expsol', [])
    .controller('ExpiracionSolCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {
        
        $scope.ids = "";
        consultarnumest();

        function consultarnumest() {
            var res = solgruasservices.consultarnumest();
            res.success(function (data, status, headers, config) {
                $scope.listestday = data;
                $scope.ids = data.Id;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.Actualizar = function (val) {

            if ($scope.horastope == "" || $scope.horastope  == undefined) {
                growl.error("Debe digitar un valor para poder guardar", { title: 'Error!' });
            } else {

                var res = solgruasservices.actualizardiasexp($scope.ids,$scope.horastope);
                res.success(function (data, status, headers, config) {
                    growl.success("Numero de Horas Actualizado", { title: 'Succes!' });
                    $scope.horastope = "";
                    consultarnumest();
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

    }]);