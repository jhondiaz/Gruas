angular.module('listuser', [])
    .controller('ListadoUserCtrl', ['$scope', '$rootScope', 'solgruasservices', '$http', 'growl', '$filter', function ($scope, $rootScope, solgruasservices, $http, growl, $filter) {

        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.pages = [];

        var f = new Date();
        $scope.fecha = f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear();

        ReportUserRoles();

        

        function ReportUserRoles() {
            var res = solgruasservices.ReportUserRoles();
            res.success(function (data, status, headers, config) {
                $scope.reportl = data;
                $scope.configPages();

                console.log($scope.reportl);
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
                //if (Math.ceil($scope.reportl.length / $scope.pageSize) > 10)
                //    fin = 10;
                //else
                    fin = Math.ceil($scope.reportl.length / $scope.pageSize);
            } else {
                if (ini >= Math.ceil($scope.reportl.length / $scope.pageSize) - 10) {
                    ini = Math.ceil($scope.reportl.length / $scope.pageSize) - 10;
                    fin = Math.ceil($scope.reportl.length / $scope.pageSize);
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


            $("#myTable").append("<table id='tblexport'><tr><td><div></div></td><td>Reporte Usuarios del Sistema</td><td></td></tr><tbody id='mytblbod'></tbody></table>")


            $scope.reportl.forEach(function (item) {
                var est = "";
                if (item.Estado == true) {
                    est = "Activo"
                } else {
                    est = "Incativo"
                }
                $('#mytblbod').append('<tr><td>' + item.Rol + '</td><td>' + item.User + '</td><td>' + item.Doc + '</td><td>' + est + '</td></tr>')
            });                              

            //$("#tblexport").tableExport({
            //    formats: ["xls", "csv", "txt"]
            //});           

            window.open('data:application/vnd.ms-excel,%EF%BB%BF' + encodeURIComponent($('div[id$=myTable]').html()));
            $("#myTable").remove();
        }


        $scope.exportpdf = function () {

            var x = document.createElement("div");
            x.setAttribute("id", "impr");
            document.body.appendChild(x);

            $("#impr")[0].innerHTML = $("#valoresenc")[0].innerHTML;
            $("#impr").append("<table id='tblexport' style='width:100%'><tr><td><div></div></td><td>Reporte Usuarios del Sistema</td><td></td></tr><tr><td>Nombre Rol</td><td>Documento Usuario</td><td>Usuario</td><td>Estado</td></tr><tbody id='mytblbod'></tbody></table>")
            $("#impr .im1").remove();
            $("#impr .im2").remove();
            $scope.reportl.forEach(function (item) {
                var est = "";
                if (item.Estado == true) {
                    est = "Activo"
                } else {
                    est = "Incativo"
                }
                $('#mytblbod').append('<tr><td>' + item.Rol + '</td><td>' + item.User + '</td><td>' + item.Doc + '</td><td>' + est + '</td></tr>')
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