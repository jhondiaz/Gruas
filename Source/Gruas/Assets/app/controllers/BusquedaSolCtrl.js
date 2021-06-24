angular.module('busquedasol', [])
    .controller('BusquedaSolCtrl', ['$rootScope', '$scope', '$http', '$location','growl', 'solgruasservices', function ($rootScope, $scope, $http, $location ,growl, solgruasservices) {

        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.pages = [];
        $scope.estadosselect = "";
        $scope.causaselect = "-1";

        buscarsolicitudesgr();

        $scope.closemap = function () {
            $(".map").toggle();
        }

        setInterval(function () {
            if ($location.path() === "/busquedasol") {
                buscarsolicitudesgr();
            }
        }, 60000);


        function buscarsolicitudesgr() {
            var res = solgruasservices.buscarsolicitudesgr();
            res.success(function (data, status, headers, config) {
                $scope.listgr = data;
                $scope.listgr1 = data;

                console.log($scope.listgr1);

                $scope.conscinmo();

            }).error(function (data, status, headers, config) {
                growl.error(data, { title: 'Error!' });
            });
        }

        $scope.conscinmo = function () {
            var res = solgruasservices.conscinmo();
            res.success(function (data, status, headers, config) {
                $scope.listcausas = data;
                $scope.configPages();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.configPages = function () {
            $scope.pages.length = 0;
            var ini = $scope.currentPage - 4;
            var fin = $scope.currentPage + 5;

            if (ini < 1) {
                ini = 1;
                //if (Math.ceil($scope.listgr.length / $scope.pageSize) > 10)
                //    fin = 10;
                //else
                fin = Math.ceil($scope.listgr.length / $scope.pageSize);
            } else {
                if (ini >= Math.ceil($scope.listgr.length / $scope.pageSize) - 10) {
                    ini = Math.ceil($scope.listgr.length / $scope.pageSize) - 10;
                    fin = Math.ceil($scope.listgr.length / $scope.pageSize);
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

        $scope.setPage = function (index) {
            $scope.currentPage = index - 1;
        };


        $scope.$watch('currentPage', function () {
            var valorpages = $scope.currentPage + parseInt(1);
            $("#lks").val(valorpages)
        });

        $scope.filertbl = function (a) {
            alert(a);
        }

        $scope.dateFilter = function () {

            if ($scope.fechasolini != undefined && $scope.fechasolfin == undefined) {
                growl.error("Debe completar los campos 'Fecha Solicitud' para realizar la busqueda", { title: 'Error!' });
                return;
            }

            if ($scope.fechasolini == undefined && $scope.fechasolfin != undefined) {
                growl.error("Debe completar los campos 'Fecha Solicitud' para realizar la busqueda", { title: 'Error!' });
                return;
            }

            if ($scope.fechaordini != undefined && $scope.fechaordfin == undefined) {
                growl.error("Debe completar los campos 'Fecha Orden' para realizar la busqueda", { title: 'Error!' });
                return;
            }

            if ($scope.fechaordini == undefined && $scope.fechaordfin != undefined) {
                growl.error("Debe completar los campos 'Fecha Orden' para realizar la busqueda", { title: 'Error!' });
                return;
            }

            $scope.listgr = $scope.listgr1;


            if ($scope.nsolicitud != undefined && $scope.nsolicitud != "") {
                var input = $scope.listgr;
                result = [];
                for (var i = 0, len = input.length; i < len; i++) {
                    if (input[i].ID_solicitud == $scope.nsolicitud) {
                        result.push(input[i]);
                    }
                }
                $scope.listgr = result;
            }

            if ($scope.estadosselect != "") {
                var input = $scope.listgr;
                result = [];
                for (var i = 0, len = input.length; i < len; i++) {

                    if (input[i].Estado == $scope.estadosselect) {
                        result.push(input[i]);
                    }
                }
                $scope.listgr = result;
            }

            if ($scope.Placa != undefined && $scope.Placa != "") {
                var input = $scope.listgr;
                result = [];
                for (var i = 0, len = input.length; i < len; i++) {
                    if (input[i].Placa == $scope.Placa) {
                        result.push(input[i]);
                    }
                }
                $scope.listgr = result;
            }

            if ($scope.NOrden != undefined && $scope.NOrden != "") {
                var input = $scope.listgr;
                result = [];
                for (var i = 0, len = input.length; i < len; i++) {
                    if (input[i].Numero_de_orden_del_servicio == $scope.NOrden) {
                        result.push(input[i]);
                    }
                }
                $scope.listgr = result;
            }


            $scope.filter2($scope.listgr);
        }

        $scope.filter2 = function (valor) {

            if ($scope.fechasolini > $scope.fechasolfin) {
                growl.error("La Fecha Desde debe Ser Inferior a la Fecha Hasta", { title: 'Error!' });
                return;
            }

            if ($scope.fechaordini > $scope.fechaordfin) {
                growl.error("La Fecha Desde debe Ser Inferior a la Fecha Hasta", { title: 'Error!' });
                return;
            }

            if ($scope.fechasolini != undefined && $scope.fechasolfin == undefined) {
                growl.error("Debe diligenciar la fecha hasta de la fecha solicitud", { title: 'Error!' });
                return;
            }

            if ($scope.fechasolini == undefined && $scope.fechasolfin != undefined) {
                growl.error("Debe diligenciar la fecha desde de la fecha solicitud", { title: 'Error!' });
                return;
            }

            if ($scope.fechaordini != undefined && $scope.fechaordfin == undefined) {
                growl.error("Debe diligenciar la fecha hasta de la fecha orden", { title: 'Error!' });
                return;
            }
            if ($scope.fechaordini == undefined && $scope.fechaordfin != undefined) {
                growl.error("Debe diligenciar la fecha desde de la fecha orden", { title: 'Error!' });
                return;
            }

            if (($scope.fechasolini != undefined && $scope.fechasolfin != undefined) && ($scope.fechaordini == undefined && $scope.fechaordfin == undefined)) {
                var input = valor;
                $scope.startDate = $scope.fechasolini;
                $scope.endDate = $scope.fechasolfin;

                var inputDate = new Date(input.Fecha_y_hora_solicitud_servicio),
                    startDate = new Date($scope.startDate),
                    endDate = new Date($scope.endDate),
                    result = [];

                for (var i = 0, len = input.length; i < len; i++) {
                    inputDate = new Date(input[i].Fecha_y_hora_solicitud_servicio);

                    if (startDate <= inputDate && inputDate <= endDate) {
                        result.push(input[i]);
                    }
                }

                $scope.listgr = result;

            } else if (($scope.fechaordini != undefined && $scope.fechaordfin != undefined) && ($scope.fechasolini == undefined && $scope.fechasolfin == undefined)) {

                var input = valor;
                $scope.startDate = $scope.fechaordini;
                $scope.endDate = $scope.fechaordfin;

                var inputDate = new Date(input.Fecha_y_hora_solicitud_servicio),
                    startDate = new Date($scope.startDate),
                    endDate = new Date($scope.endDate),
                    result = [];

                for (var i = 0, len = input.length; i < len; i++) {
                    inputDate = new Date(input[i].Fecha_y_hora_de_orden_de_servicio);
                    if (startDate <= inputDate && inputDate <= endDate) {
                        result.push(input[i]);
                    }
                }

                $scope.listgr = result;

            } else if (($scope.fechaordini != undefined || $scope.fechaordfin != undefined) && ($scope.fechasolini != undefined || $scope.fechasolfin != undefined)) {

                var input = valor;
                $scope.startDate = $scope.fechasolini;
                $scope.endDate = $scope.fechasolfin;

                var inputDate = new Date(input.Fecha_y_hora_solicitud_servicio),
                    startDate = new Date($scope.startDate),
                    endDate = new Date($scope.endDate),
                    result = [];

                for (var i = 0, len = input.length; i < len; i++) {
                    inputDate = new Date(input[i].Fecha_y_hora_solicitud_servicio);
                    if (startDate <= inputDate && inputDate <= endDate) {
                        result.push(input[i]);
                    }
                }

                $scope.listgr = result;

                var input = $scope.listgr;
                $scope.startDate = $scope.fechaordini;
                $scope.endDate = $scope.fechaordfin;

                var inputDate = new Date(input.Fecha_y_hora_solicitud_servicio),
                    startDate = new Date($scope.startDate),
                    endDate = new Date($scope.endDate),
                    result = [];

                for (var i = 0, len = input.length; i < len; i++) {
                    inputDate = new Date(input[i].Fecha_y_hora_de_orden_de_servicio);
                    if (startDate <= inputDate && inputDate <= endDate) {
                        result.push(input[i]);
                    }
                }

                $scope.listgr = result;
            }

            $scope.currentPage = 0;
        }


        $scope.clean = function () {
            $scope.nsolicitud = undefined;
            $scope.NOrden = undefined;
            $scope.Placa = undefined;
            $scope.estadosselect = "";

            $scope.listgr = $scope.listgr1;
            $('input[type=datetime-local]').val('');
            $scope.fechasolini = undefined;
            $scope.fechasolfin = undefined;
            $scope.fechaordini = undefined;
            $scope.fechaordfin = undefined;
        }

        $scope.opencancel = function (item) {
            $("#modaldetalles").modal('hide');

            $scope.detallesol = angular.copy(item);

            $("#modalcans").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })

        };


        $scope.cancelsolicitud = function (ordserv) {
            if ($scope.causacans == "" || $scope.causacans == undefined) {
                growl.error("Debe Digitar una Observación Para Poder Continuar.", { title: 'Error!' });
            } else if ($scope.causaselect == "-1") {
                growl.error("Debe Seleccionar una Causa de Cancelación Para Poder Continuar.", { title: 'Error!' });
            } else {
                var res = solgruasservices.cacelarservicio($scope.detallesol.ID_solicitud, $scope.causaselect, $scope.causacans, ordserv);
                res.success(function (data, status, headers, config) {
                    if (data[0] == "0") {
                        for (var i = 1; i < data.length; i++) {
                            growl.error(data[i], { title: 'Error!' });
                        }
                    } else {
                        growl.success(data, { title: 'Succes!' });
                    }

                    $scope.causacans = "";
                    $scope.causaselect = "-1";

                    buscarsolicitudesgr();

                    $('#modalcans').modal('hide');
                })
                    .error(function (data, status, headers, config) {
                        growl.error(data, { title: 'Error!' });
                    });
            }
        }


        $scope.opendetalle = function (itm) {
            var res = solgruasservices.solicitudporid(itm.ID_solicitud, itm.Estado);
            res.success(function (data, status, headers, config) {
                $scope.detallesol = data;
                $scope.searchtvi(itm.ID_solicitud);
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

            $("#modaldetalles").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        };




        $scope.opendetallecancel = function (itm) {
            var res = solgruasservices.solicitudporid(itm.ID_solicitud,itm.Estado);
            res.success(function (data, status, headers, config) {
                $scope.detallesol = data;
                $scope.searchtvi(itm.ID_solicitud);
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

            $("#modaldetll").modal({
                escapeClose: true,
                clickClose: false,
                showClose: true
            })
        };


        $scope.searchtvi = function (itm) {

            var res = solgruasservices.constvxid(itm);
            res.success(function (data, status, headers, config) {
                $scope.detallesvehi = data;
                $scope.searchgrubyid(itm);
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.searchgrubyid = function (itm) {

            var res = solgruasservices.searchgrubyid(itm);
            res.success(function (data, status, headers, config) {
                $scope.detallesgrua = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.listaUbicaciones = [];
        var listaMarkers = [];

        $scope.SearchGrua = function (itm) {
            $scope.listaUbicaciones = [];
            var res = solgruasservices.SearchGrua(itm.Numero_de_orden_del_servicio);
            res.success(function (data, status, headers, config) {
                $scope.listaUbicaciones = data;
                //$scope.Ubicacion = data.Ubicacion;
                //$scope.DetallePos = data.Tiempo;
                //$scope.PlacaGrua = data.Placagrua;

                $scope.OpenMap();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.OpenMap = function () {

            $(".map").toggle();

            var map = null;

            //var iconmar = '../img/Solicitada.png';

            for (var i = 0; i < listaMarkers.length; i++) {
                if (listaMarkers[i] != null && listaMarkers[i] != undefined) { 
                    listaMarkers[i].setMap(null);
                }
            }

            for (var i = 0; i < $scope.listaUbicaciones.length; i++) {
                var ubicacion = $scope.listaUbicaciones[i];
                var uluru = { lat: parseFloat(ubicacion.Ubicacion.split(',')[0]), lng: parseFloat(ubicacion.Ubicacion.split(',')[1]) };


                //google.maps.event.trigger(map, "resize");
                if (map == null) {
                    map = new google.maps.Map(document.getElementById('map'), {
                        zoom: 15,
                        center: uluru
                    });
                }

                var marker = new google.maps.Marker({
                    position: uluru,
                    map: map,
                    title: ubicacion.PlacaGrua,
                    //icon: iconmar
                });

                map.addListener('center_changed', function () {
                    // 3 seconds after the center of the map has changed, pan back to the
                    // marker.
                    window.setTimeout(function () {
                        map.panTo(marker.getPosition());
                    }, 3000);
                });

                marker.addListener('click', function () {
                    map.setZoom(18);
                    map.setCenter(marker.getPosition());
                });

                listaMarkers[i] = marker;
            }

            
        }

    }]);