angular.module('reports', [])
    .controller('reportsCtrl', ['$scope', '$http', 'growl', 'ReportsServics', function ($scope, $http, growl, ReportsServics) {


        ///Caja vales
        $scope.generarReports = function () {
            var res = ReportsServics.setGenerarReports($scope.IdUser, $scope.StartDate, $scope.EndDate).then(function (pl) {
                var data = pl.data;
                if (data.length == 0) {
                    growl.warning('No hay Resultados!', { title: 'Warning!' });
                    return;
                }

                $scope.ListVales = data;

            },
          function (errorPl) {
              if (angular.isArray(errorPl.data))
                  $scope.errorMessages = errorPl.data;
              else
                  $scope.errorMessages = new Array(errorPl.data.replace(/["']{1}/gi, ""));

              growl.error($scope.errorMessages, { title: 'Error!' });
          });

        }
        $scope.generarReportsPuc = function () {
            var res = ReportsServics.setGenerarReportsPuc($scope.IdUser, $scope.StartDate, $scope.EndDate).then(function (pl) {
                var data = pl.data;
                if (data.length == 0) {
                    growl.warning('No hay Resultados!', { title: 'Warning!' });
                    return;
                }

                $scope.ListVales = data;

            },
          function (errorPl) {
              if (angular.isArray(errorPl.data))
                  $scope.errorMessages = errorPl.data;
              else
                  $scope.errorMessages = new Array(errorPl.data.replace(/["']{1}/gi, ""));

              growl.error($scope.errorMessages, { title: 'Error!' });
          });


        }



        //Caja General
        $scope.generarReportsGeneral = function () {
            var res = ReportsServics.setGenerarReportsGeneral($scope.IdUser, $scope.StartDate, $scope.EndDate).then(function (pl) {
                var data = pl.data;
                if (data.length == 0) {
                    growl.warning('No hay Resultados!', { title: 'Warning!' });
                    return;
                }

                $scope.ListVales = data;

            },
          function (errorPl) {
              if (angular.isArray(errorPl.data))
                  $scope.errorMessages = errorPl.data;
              else
                  $scope.errorMessages = new Array(errorPl.data.replace(/["']{1}/gi, ""));

              growl.error($scope.errorMessages, { title: 'Error!' });
          });

        }



        $scope.generarReportsPucGeneral = function () {
            var res = ReportsServics.setGenerarReportsPucGeneral($scope.IdUser, $scope.StartDate, $scope.EndDate).then(function (pl) {
                var data = pl.data;
                if (data.length == 0) {
                    growl.warning('No hay Resultados!', { title: 'Warning!' });
                    return;
                }

                $scope.ListVales = data;

            },
          function (errorPl) {
              if (angular.isArray(errorPl.data))
                  $scope.errorMessages = errorPl.data;
              else
                  $scope.errorMessages = new Array(errorPl.data.replace(/["']{1}/gi, ""));

              growl.error($scope.errorMessages, { title: 'Error!' });
          });


        }



        $scope.generarReportsAllPucGeneral = function () {
            var res = ReportsServics.setGenerarReportsAllPucGeneral($scope.IdUser, $scope.StartDate, $scope.EndDate).then(function (pl) {
                var data = pl.data;
                if (data.length == 0) {
                    growl.warning('No hay Resultados!', { title: 'Warning!' });
                    return;
                }

                $scope.ListVales = data;

            },
          function (errorPl) {
              if (angular.isArray(errorPl.data))
                  $scope.errorMessages = errorPl.data;
              else
                  $scope.errorMessages = new Array(errorPl.data.replace(/["']{1}/gi, ""));

              growl.error($scope.errorMessages, { title: 'Error!' });
          });


        }

    }]);