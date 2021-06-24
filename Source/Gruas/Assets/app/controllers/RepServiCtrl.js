angular.module('RepServi', [])
    .controller('RepServiCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        $scope.tipos = [];
        $scope.bar = 0;
        $scope.bar2 = 0;

        $scope.clean = function () {
            $scope.tipos = [];
        }


        $scope.f1 = function () {
            $scope.in1 = "1";
            $scope.in2 = "0";
            $scope.tipos = [];
            $scope.bar2 = 0;
        }

        $scope.f2 = function () {
            $scope.in2 = "1";
            $scope.in1 = "0";
            $scope.tipos = [];
            $scope.bar = 0;
        }

        $scope.Buscar = function () {

            if ($("#chin").is(':checked')) {

                var res = solgruasservices.numsolaagente($scope.tipos);
                res.success(function (data, status, headers, config) {
                    $scope.report = data;
                    $scope.bar = 1;
                    $scope.bar2 = 0;
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            } else if ($("#chtg").is(':checked')) {

                if ($scope.tipos.fini == undefined || $scope.tipos.ffin == undefined) {
                    growl.error("Debe seleccionar una fecha inicio y una fecha fin para poder continuar", { title: 'Error!' });
                }
                else {
                    var res = solgruasservices.timesol($scope.tipos);
                    res.success(function (data, status, headers, config) {
                        $scope.report = data;
                        $scope.bar = 0;
                        $scope.bar2 = 1;
                    })
                        .error(function (data, status, headers, config) {
                            growl.error(data, { title: 'Error!' });
                        });
                }
            }
        }


        $scope.excel = function (e) {

            if ($("#chin").is(':checked')) {

                var x = document.createElement("div");
                x.setAttribute("id", "myTable");
                document.body.appendChild(x);


                $("#myTable").append("<table id='tblexport'><tr><td><div></div></td><td>Reporte de Solicitudes por Agente de Transito</td><td></td></tr><tbody id='mytblbod'><tr><td>Usuario</td><td>Conteo</td></tr></tbody></table>")


                $scope.report.forEach(function (item) {
                    $('#mytblbod').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                });

                window.open('data:application/vnd.ms-excel,%EF%BB%BF' + encodeURIComponent($('div[id$=myTable]').html()));
                $("#myTable").remove();
            } else {

                var x = document.createElement("div");
                x.setAttribute("id", "myTable");
                document.body.appendChild(x);


                $("#myTable").append("<table id='tblexport'><tr><td><div></div></td><td>Reporte Por Tiempo de Solicitud</td><td></td></tr><tbody id='mytblbod'><tr><td># Solicitud</td><td>Fecha Solicitud</td><td>Fecha Respuesta</td><td>Tiempo Respuesta</td><td>Fecha Atención</td><td>Tiempo Atención</td><td>Estados</td></tr></tbody></table>")


                $scope.report.forEach(function (item) {
                    $('#mytblbod').append('<tr><td>' + item.solicitud + '</td><td>' + item.fini + '</td><td>' + item.ffin + '</td><td>' + item.dias + '</td><td>' + item.fat + '</td><td>' + item.dias2 + '</td><td>' + item.Estado + '</td></tr>')
                });

                window.open('data:application/vnd.ms-excel,%EF%BB%BF' + encodeURIComponent($('div[id$=myTable]').html()));
                $("#myTable").remove();
            }
        }

        $scope.exportpdf = function () {

            if ($("#chin").is(':checked')) {
                var objeto = document.getElementById('imp1');  //obtenemos el objeto a imprimir
                var ventana = window.open('', '_blank');  //abrimos una ventana vacía nueva
                ventana.document.write(objeto.innerHTML);  //imprimimos el HTML del objeto en la nueva ventana
                ventana.document.close();  //cerramos el documento
                ventana.print();  //imprimimos la ventana
                ventana.close();  //cerramos la ventana
            }
            else {

                var objeto = document.getElementById('imprimeme');  //obtenemos el objeto a imprimir
                var ventana = window.open('', '_blank');  //abrimos una ventana vacía nueva
                ventana.document.write(objeto.innerHTML);  //imprimimos el HTML del objeto en la nueva ventana
                ventana.document.close();  //cerramos el documento
                ventana.print();  //imprimimos la ventana
                ventana.close();  //cerramos la ventana
            }
        }
        
    }]);