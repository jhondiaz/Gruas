angular.module('reportuser', [])
    .controller('ReporteUsuarioCtrl', ['$scope', '$rootScope', 'solgruasservices', '$http', 'growl', '$filter', function ($scope, $rootScope, solgruasservices, $http, growl, $filter) {


        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.pages = [];


        var f = new Date();
        $scope.fecha = f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear();

        searchreportroluser();

        function searchreportroluser() {
            var res = solgruasservices.searchreportroluser();
            res.success(function (data, status, headers, config) {
                $scope.report = data;
                $scope.configPages();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.setPage = function (index) {
            $scope.currentPage = index - 1;
        };

        $scope.$watch('currentPage', function () {
            var valorpages = $scope.currentPage + parseInt(1);
            $("#lks").val(valorpages)
        });
        $scope.configPages = function () {
            $scope.pages.length = 0;
            var ini = $scope.currentPage - 4;
            var fin = $scope.currentPage + 5;
            if (ini < 1) {
                ini = 1;
                //if (Math.ceil($scope.report.length / $scope.pageSize) > 10)
                //    fin = 10;
                //else
                    fin = Math.ceil($scope.report.length / $scope.pageSize);
            } else {
                if (ini >= Math.ceil($scope.report.length / $scope.pageSize) - 10) {
                    ini = Math.ceil($scope.report.length / $scope.pageSize) - 10;
                    fin = Math.ceil($scope.report.length / $scope.pageSize);
                }
            }
            if (ini < 1) ini = 1;
            for (var i = ini; i <= fin; i++) {
                $scope.pages.push({
                    no: i
                });
            }

            if ($scope.currentPage >= $scope.pages.length)
                $scope.currentPage = $scope.pages.length - 1;


            setTimeout(function () {
                $('#lks option[value="? undefined:undefined ?"]').remove();
                $("#lks").val(1)
            }, 100);
        };

        $scope.excel = function (e) {

            var x = document.createElement("div");
            x.setAttribute("id", "myTable");
            document.body.appendChild(x);


            $("#myTable").append("<table id='tblexport'><tr><td>Reporte Usuarios del Sistema por Rol y Estado</td></tr><tr><td>Nombre Rol</td><td>Estado</td><td>Cantidad</td></tr><tbody id='tbmy'></tbody></table>")


            $scope.report.forEach(function (item) {
                var est = "";
                if (item.Activo == true) {
                    est = "Activo"
                } else {
                    est = "Incativo"
                }
                $('#tbmy').append('<tr><td>' + item.Name + '</td><td>' + est + '</td><td>' + item.Conteo + '</td></tr>')
            });

            window.open('data:application/vnd.ms-excel,%EF%BB%BF' + encodeURIComponent($('div[id$=myTable]').html()));

            $("#myTable").remove();
        }


        $scope.exportpdf = function () {

            var x = document.createElement("div");
            x.setAttribute("id", "impr");
            document.body.appendChild(x);
            
            $("#impr")[0].innerHTML = $("#valoresenc")[0].innerHTML;
            $("#impr").append("<div class='span12'><table id='tblexport' style='width:100%;text-align:center;'><tr><td colspan='3'>Reporte Usuarios del Sistema por Rol y Estado</td></tr><tr><td>Nombre Rol</td><td>Estado</td><td>Cantidad</td></tr><tbody id='tbmy'></tbody></table></div>")
            $("#impr .im1").remove();
            $("#impr .im2").remove();
            $scope.report.forEach(function (item) {
                var est = "";
                if (item.Activo == true) {
                    est = "Activo"
                } else {
                    est = "Incativo"
                }
                $('#tbmy').append('<tr><td>' + item.Name + '</td><td>' + est + '</td><td>' + item.Conteo + '</td></tr>')
            });
                       

            var objeto = document.getElementById('impr');  
            var ventana = window.open('', '_blank'); 
            $("#impr").remove();
            ventana.document.write(objeto.innerHTML);
            ventana.document.close();
            ventana.print();            
            ventana.close();
        }

    }]);