angular.module('repseragente', [])
    .controller('RepseragenteCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        $scope.tipos = [];
        $scope.bar = 0;
        $scope.tipos.ci = "0";
        $scope.tipos.tg = "0";
        $scope.tipos.tv = "0";
        $scope.tipos.mc = "0";

        $scope.f1 = function () {
            if ($("#chin").is(':checked')) {
                $scope.tipos.ci = "1"
                
            } else {
                $scope.tipos.ci = "0"
            }
        }

        $scope.f2 = function () {
            if ($("#chtg").is(':checked')) {
                $scope.tipos.tg = "1"

            } else {
                $scope.tipos.tg = "0"
            }
        }

        $scope.f3 = function () {
            if ($("#chtv").is(':checked')) {
                $scope.tipos.tv = "1"

            } else {
                $scope.tipos.tv = "0"
            }
        }

        $scope.f4 = function () {
            if ($("#chmc").is(':checked')) {
                $scope.tipos.mc = "1"

            } else {
                $scope.tipos.mc = "0"
            }
        }

        $scope.clean = function () {
            $scope.tipos = [];

            $("#chin").prop('checked', false);
            $("#chtg").prop('checked', false);
            $("#chtv").prop('checked', false);
            $("#chmc").prop('checked', false);
        }

        $scope.Buscar = function () {

            if ($scope.tipos.est == "" || $scope.tipos.est == undefined || $scope.tipos.est == null) {
                $scope.tipos.est = "";
            }

            if (($scope.tipos.pl == undefined || $scope.tipos.pl == "")) {
                growl.error("Debe digitar una placa para poder realizar la consulta.", { title: 'Error!' });
            }
            else if ($scope.tipos.ci == "0" && $scope.tipos.tg == "0" && $scope.tipos.tv == "0" && $scope.tipos.mc == "0") {                
                growl.error("Debe seleccionar algún tipo poder realizar la consulta.", { title: 'Error!' });
            }
            else {

                var res = solgruasservices.reportsolagent($scope.tipos);
                res.success(function (data, status, headers, config) {
                    if (data == "La Placa No se Encuentra Asociada a Ningún Agente.") {
                        growl.error(data, { title: 'Error!' });
                    } else {                        
                        $scope.report = data;
                        $scope.bar = 1;
                    }
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }

        $scope.excel = function (e) {

            if ($scope.report == undefined) {
                growl.error('No hay información para exportar a excel.', { title: 'Error!' });
            } else {

                var x = document.createElement("div");
                x.setAttribute("id", "myTable");
                document.body.appendChild(x);

                $("#myTable").append("<div style='width:100%;text-align:center;'><label style='color:#003e65;font-size:20px;font-weight:bold;margin-top:30px;'>Reporte de Solicitudes por Agente de Transito</label></div></hr>")
                $("#myTable").append("<table border=1 id='tblexport'><tr> <td style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Causa de la Inmovilización</td><td>Conteo</td></tr><tbody id='mytblbod'></tbody></table>")
                $("#myTable").append("<table border=1 id='tblexport1'><tr><td style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Tipo de Grúa</td><td>Conteo</td></tr><tbody id='mytblbod1'></tbody></table>")
                $("#myTable").append("<table border=1 id='tblexport2'><tr><td style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Tipo de Vehículo a Inmovilizar</td><td>Conteo</td></tr><tbody id='mytblbod2'></tbody></table>")
                $("#myTable").append("<table border=1 id='tblexport3'><tr><td style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Causa Cancelación</td><td>Conteo</td></tr><tbody id='mytblbod3'></tbody></table>")

                if ($scope.report.CausaIn !=null) {
                    $scope.report.CausaIn.forEach(function (item) {
                        $('#mytblbod').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.TipoGru != null) {
                    $scope.report.TipoGru.forEach(function (item) {
                        $('#mytblbod1').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.TipoVehi != null) {
                    $scope.report.TipoVehi.forEach(function (item) {
                        $('#mytblbod2').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.CausaCan != null) {
                    $scope.report.CausaCan.forEach(function (item) {
                        $('#mytblbod3').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }

                window.open('data:application/vnd.ms-excel,%EF%BB%BF' + encodeURIComponent($('div[id$=myTable]').html()));
                $("#myTable").remove();
            }
        }


        $scope.exportpdf = function () {
            if ($scope.report == undefined) {
                growl.error('No hay información para exportar a pdf.', { title: 'Error!' });
            } else {
                var x = document.createElement("div");
                x.setAttribute("id", "myTable");
                document.body.appendChild(x);

                $("#myTable").append("<div style='width:100%;text-align:center;'><label style='color:#003e65;font-size:20px;font-weight:bold;margin-top:30px;'>Reporte de Solicitudes por Agente de Transito</label></div></hr>")
                $("#myTable").append("<table style='width:100%' border=1 id='tblexport'><tr> <td colspan='2' style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Causa de la Inmovilización</td><td>Conteo</td></tr><tbody id='mytblbod'></tbody></table></br>")
                $("#myTable").append("<table style='width:100%' border=1 id='tblexport1'><tr><td colspan='2' style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Tipo de Grúa</td><td>Conteo</td></tr><tbody id='mytblbod1'></tbody></table></br>")
                $("#myTable").append("<table style='width:100%' border=1 id='tblexport2'><tr><td colspan='2' style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Tipo de Vehículo a Inmovilizar</td><td>Conteo</td></tr><tbody id='mytblbod2'></tbody></table></br>")
                $("#myTable").append("<table style='width:100%' border=1 id='tblexport3'><tr><td colspan='2' style='text-align:center:'>Busqueda de Solicitudes de Grúas</td></tr><tr><td>Causa Cancelación</td><td>Conteo</td></tr><tbody id='mytblbod3'></tbody></table></br>")

                if ($scope.report.CausaIn != null) {
                    $scope.report.CausaIn.forEach(function (item) {
                        $('#mytblbod').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.TipoGru != null) {
                    $scope.report.TipoGru.forEach(function (item) {
                        $('#mytblbod1').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.TipoVehi != null) {
                    $scope.report.TipoVehi.forEach(function (item) {
                        $('#mytblbod2').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }
                if ($scope.report.CausaCan != null) {
                    $scope.report.CausaCan.forEach(function (item) {
                        $('#mytblbod3').append('<tr><td>' + item.Name + '</td><td>' + item.Conteo + '</td></tr>')
                    });
                }

                var objeto = document.getElementById('myTable');
                var ventana = window.open('', '_blank');
                $("#myTable").remove();
                ventana.document.write(objeto.innerHTML);
                ventana.document.close();
                ventana.print();
                ventana.close();
            }
        }

    }]);