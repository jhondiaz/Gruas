angular.module('solgruas', [])
    .controller('SolicitudGruasCtrl', ['$rootScope', '$scope', '$http', 'growl', 'solgruasservices', function ($rootScope, $scope, $http, growl, solgruasservices) {

        $scope.fl = [];
        $scope.fl.causain = "-1";
        $scope.fl.cnvhi = "-1";
        $scope.fl.vgcuadra = "-1";
        var bis = "";

        $scope.fl.tgrua = "-1";
        $scope.fl.numgruas = "-1";
        $scope.fl.tvehiculo = "-1";
        $scope.fl.iduser = "";
        $scope.fl.userentidad = "";
        $scope.fl.useremail = "";
        $scope.fl.tservicio = "-1";

        $scope.fl.magna = "";


        var lat = "";
        var longi = "";
        var altu = "";
        var infowindow = "";
        var address = "";


        searchinfcodes();

        $("#rdpl").attr('checked', true);

        $scope.conc = function () {

            var vgcuadra = "";
            var tvcuadrante = "";


            if ($("#chbis").is(':checked')) {
                bis = "BIS";
            } else {
                bis = "";
            }


            if ($("#tvcuadrante").val() == "SUR") {
                tvcuadrante = "SUR";
            } else if ($("#tvcuadrante").val() == "ESTE") {
                tvcuadrante = "ESTE"
            } else {
                tvcuadrante = "";
            }

            if ($("#vgcuadra").val() == "SUR") {
                vgcuadra = "SUR";
            } else if ($("#vgcuadra").val() == "ESTE") {
                vgcuadra = "ESTE"
            } else {
                vgcuadra = "";
            }

            $scope.fl.dirr = ($("#tvprin").val() == "-1" ? "" : $("#tvprin").val()) + " " + $("#numprin").val() + ($("#tlprins").val() == "-1" ? "" : $("#tlprins").val()) + " " + bis + ($("#lbvprin").val() == "-1" ? "" : $("#lbvprin").val()) + " " + tvcuadrante + " # " + $("#numvg").val() + ($("#lvgen").val() == "-1" ? "" : $("#lvgen").val()) + " - " + $("#placagen").val() + " " + vgcuadra;
        }


        function searchinfcodes() {
            var res = solgruasservices.infcodes();
            res.success(function (data, status, headers, config) {

                var options = {

                    data: data,
                    getValue: "Codigo",

                    list: {
                        match: {
                            enabled: true
                        }
                    },

                    theme: "square"
                };
                $("#coninf").easyAutocomplete(options);
                consultarnumgr();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        function consultarnumgr() {
            var res = solgruasservices.consultarnumgr();
            res.success(function (data, status, headers, config) {
                $scope.listgruas = data;

                $scope.conscinmo();

                for (var i = 1; i < data.Conteo + 1; i++) {
                    $("#numgruas").append("<option value='" + i + "'>" + i + "</option>");
                }
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        //SELECTS
        $scope.conscinmo = function () {
            var res = solgruasservices.conscinmo();
            res.success(function (data, status, headers, config) {
                $scope.listcausas = data;
                $scope.T_S_Translados();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.T_S_Translados = function () {
            var res = solgruasservices.T_S_Translados();
            res.success(function (data, status, headers, config) {
                $scope.T_S_Translados = data;
                $scope.Localidades();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.Localidades = function () {
            var res = solgruasservices.Localidades();
            res.success(function (data, status, headers, config) {
                $scope.Localidades = data;
                $scope.TipoGruas();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        //$scope.SentidoViales = function () {
        //    var res = solgruasservices.SentidoViales();
        //    res.success(function (data, status, headers, config) {
        //        $scope.SentidoViales = data;
        //        $scope.TipoGruas();
        //    })
        //        .error(function (data, status, headers, config) {
        //            growl.error(data, { title: 'Error!' });
        //        });
        //}

        $scope.TipoGruas = function () {
            var res = solgruasservices.TipoGruas();
            res.success(function (data, status, headers, config) {
                $scope.TipoGruas = data;
                $scope.TVehiculoInmovilizars();
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.TVehiculoInmovilizars = function () {
            var res = solgruasservices.TVehiculoInmovilizars();
            res.success(function (data, status, headers, config) {
                $scope.TVehiculoInmovilizars = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        //FIN

        $scope.constope = function (id) {
            var res = solgruasservices.constope(id);
            res.success(function (data, status, headers, config) {
                $scope.topeuser = data;

                if ($scope.topeuser != "") {
                    growl.info(data, { title: 'Alerta !' });
                }
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.openmodemap = function (direccion) {
            //disbtn = true;
            $("#btndissend").attr("disabled", true);
        
            if ($scope.fl.dirr == "" | $scope.fl.dirr == undefined) {
                growl.error("El campo dirección debe estar diligenciado para continuar.", { title: 'Error!' });
                $("#btndissend").attr("disabled", false);
            } else {
                iniciar(direccion);
            }
        }



        function iniciar(direccion) {
            //$(".map").toggle('slow');
            //$(".map").css('display','none');
            var uluru = { lat: parseFloat("4.734634"), lng: parseFloat("-74.098969") };

            google.maps.event.trigger(map, "resize");
            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 15,
                center: uluru
            });
            //var marker = new google.maps.Marker({
            //    position: uluru,
            //    map: map
            //});

            initMap(direccion)
        }
        var addvr = 0;

        function initMap(dirr) {

            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 12,
                center: { lat: 4.734634, lng: -74.098969 }
            });

            var geocoder = new google.maps.Geocoder();
            infowindow = new google.maps.InfoWindow({ map: map });
            var elevator = new google.maps.ElevationService;

            if (addvr == 0) {
                addvr = 1;
                document.getElementById('submit').addEventListener('click', function () {
                    geocodeAddress(geocoder, map);
                });

                map.addListener('click', function (event) {
                    displayLocationElevation(event.latLng, elevator);
                });

            }


            $('#address').val(dirr + " Bogotá, Colombia");
            setTimeout(function () {
                $(".btncl").click();
            }, 300)

        }


        $scope.closemap = function () {
            $(".map").toggle('slow');
        }


        function geocodeAddress(geocoder, resultsMap) {
            address = document.getElementById('address').value;
            geocoder.geocode({ 'address': address }, function (results, status) {
                //console.log(status);
                if (status === 'OK') {
                    resultsMap.setCenter(results[0].geometry.location);
                    var marker = new google.maps.Marker({
                        map: resultsMap,
                        position: results[0].geometry.location
                    });
                    var elevator = new google.maps.ElevationService;
                    displayLocationElevation(marker.map.center, elevator);

                } else {
                    growl.error("Verifique la Geolicalización", { title: 'Error!' });
                }
            });
        }



        function displayLocationElevation(location, elevator) {
            //console.log(location);
            infowindow.setPosition(location);
            elevator.getElevationForLocations({
                'locations': [location]
            }, function (results, status) {
                if (status === 'OK') {
                    if (results[0]) {
                        altu = results[0].elevation;
                        document.getElementById('latlong').innerText = location;
                        var valuesmap = document.getElementById('latlong').innerText.replace('(', '').replace(')', '').split(',')
                        lat = valuesmap[0];
                        longi = valuesmap[1];
                        Coordenada(lat, longi);
                    }
                }
            });




        }


        function Coordenada(ltai, lond) {
            var lat = ltai;
            var lng = lond;
            var latn = Math.abs(lat); /* Devuelve el valor absoluto de un número, sea positivo o negativo */
            var latgr = Math.floor(latn * 1); /* Redondea un número hacia abajo a su entero más cercano */
            var latmin = Math.floor((latn - latgr) * 60); /* Vamos restando el número entero para transformarlo en minutos */
            var latseg = ((((latn - latgr) * 60) - latmin) * 60); /* Restamos el entero  anterior ahora para segundos */
            var latc = (latgr + "º " + latmin + "\' " + latseg.toFixed(2) + '\"'); /* Prolongamos a centésimas de segundo */
            if (lat > 0) {
                x = latc + ' N'; /* Si el número original era positivo, es Norte */
            } else {
                x = latc + ' S'; /* Si el número original era negativo, es Sur */
            } /* Repetimos el proceso para la longitud (Este, -W-Oeste) */
            var lngn = Math.abs(lng);
            var lnggr = Math.floor(lngn * 1);
            var lngmin = Math.floor((lngn - lnggr) * 60);
            var lngseg = ((((lngn - lnggr) * 60) - lngmin) * 60);
            var lngc = (lnggr + "º " + lngmin + "\' " + lngseg.toFixed(2) + '\"');
            if (lng > 0) {
                y = lngc + ' E';
            } else {
                y = lngc + ' W';
            }

            var latlng = {
                lat: parseFloat(ltai),
                lng: parseFloat(lond)
            };

            var lats = "";
            var longis = "";

            var xhttp = new XMLHttpRequest();
            xhttp.open("POST", "https://sitidata-stdr.appspot.com/api/geocoder");
            xhttp.setRequestHeader("Content-type", "application/json");
            xhttp.setRequestHeader("Authorization", "Token BYAYJLDX1RNCEUHT8S8T58M5WDOGNJ");
            xhttp.send(JSON.stringify({ address: $scope.fl.dirr, city: 'Bogota' }));
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    var data = JSON.parse(this.responseText);
                    lat = data.data.latitude.toString();
                    longi = data.data.longitude.toString();

                    var lca = ((data.data.localidad).toUpperCase()).toString();
                    var brr = ((data.data.barrio).toUpperCase()).toString();
                    var direc = ((data.data.dirtrad).toUpperCase()).toString();

                    //$scope.fl.brr = data.data.barrio;
                    //$("#brr").val(data.data.barrio);

                    if (data.data.localidad == "") {
                        growl.error("Verifique la Dirección", { title: 'Error!' });
                        $("#brr").val();
                        $("#localidad").val();
                        $scope.fl.dirr = "";
                        $scope.fl.localidad = "";
                        $("#btndissend").attr("disabled", false);
                    } else {

                        var radlat = (Math.PI * lat / 180);
                        var radlog = (Math.PI * longi / 180);
                        var al = altu;

                        var a = 6378137;
                        var e2 = 0.00669438;
                        var n = a / (Math.sqrt(1 - (e2 * Math.pow(radlat, 2))))

                        var xm = (n + altu) * (Math.cos(radlat) * Math.cos(radlog));
                        var ym = (n + altu) * (Math.cos(radlat) * Math.sin(radlog));
                        var zm = ((1 - e2) * n + al) * Math.sin(radlat);


                        $scope.fl.magna = "X:" + xm + ",Y:" + ym + ",Z:" + zm;

                        $scope.fun(lca, brr, direc);
                    }
                    $("#btndissend").attr("disabled", false);
                }
            };


            infowindow.setContent('Latitud: ' + lat + '<br>' + 'Longitud: ' + longi + '<br>' + 'Elevación: <br>' + altu + ' meters.' + '<br>' + ' Coordenadas GMS  Latitud: ' + x + ' Longitud: ' + y);
        }

        $scope.fun = function (loc, brr, direc) {

            $scope.fl.brr = brr;
            $("#brr").val(brr);

            $scope.fl.localidad = loc;
            $("#localidad").val(loc);

            $scope.fl.dirr2 = direc;
        }

        $scope.searchagent = function () {
            var res = solgruasservices.searchagent();
            res.success(function (data, status, headers, config) {
                $scope.listuserssearch = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        $scope.busplacaag = function () {

            if ($("#rdced").is(":checked")) {
                $scope.tipo = "Cedula";
            }

            if ($("#rdpl").is(":checked")) {
                $scope.tipo = "Placa"
            }

            var res = solgruasservices.searchagent($scope.fl.placasearch, $scope.tipo);
            res.success(function (data, status, headers, config) {
                //console.log(data);
                if (data == null) {
                    $scope.fl.iduser = "";
                    $scope.fl.agensearch = "";
                    $scope.fl.useremail = "";
                    $scope.fl.Placa = "";
                    $("#agensearch").text("----");
                    growl.error("La busqueda del agente no arrojo ningún resultado.", { title: 'Error!' });
                } else {
                    $scope.listuserssearch = data;
                    $scope.fl.iduser = $scope.listuserssearch.Id;
                    $scope.fl.agensearch = $scope.listuserssearch.firstName;
                    $scope.fl.useremail = $scope.listuserssearch.Email;
                    $scope.fl.userentidad = $scope.listuserssearch.Entidad;
                    $scope.fl.Placa = $scope.listuserssearch.PlacaAgente;
                    $("#agensearch").text($scope.fl.agensearch);

                    $scope.constope($scope.fl.iduser);
                }

            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }


        $scope.validarcorr = function (direccion) {
            $scope.disbtn = true;
            $("#btndissend").attr("disabled", true);
            $scope.validarcampos();
        };

        $scope.validarcampos = function () {

            //for (var i = 0; i < tvh.length; i++) {
            //    if (tvh[i] == undefined) {
            //        console.log(tvh);
            //        tvh.splice(i);
            //    }
            //}

            if (tvh.length == 0) {
                pos = 0;
                conteo = 0;
            }


            if ($scope.fl.tservicio == 1) {
                $scope.fl.CodInfracc = $("#coninf").val();
            } else if ($scope.fl.tservicio == 4) {
                $scope.fl.CodInfracc = "ZZ99"
            }

            //$scope.disbtn = false;

            if ($scope.fl.iduser == "" || $scope.fl.iduser == undefined) {
                growl.error("Debe Registrar el Agente que Reporta la Solicitud.", { title: 'Error!' });
                $scope.disbtn = false;
            } else if ($scope.fl.causain == "-1") {
                growl.error("Debe Seleccionar una Causa de la Inmovilización Para Poder Generar la Solicitud.", { title: 'Error!' });
                $scope.disbtn = false;
            } else if ($scope.fl.dirr == "" | $scope.fl.dirr == undefined) {
                growl.error("Debe Digitar una Dirección Para Poder Generar la Solicitud.", { title: 'Error!' });
                $scope.disbtn = false;
            } else if (tvh.length == 0 && $scope.fl.tservicio != "4") {
                growl.error("Debe Seleccionar Como minimo un Tipo de vehiculo y una cantidad Para Poder Generar la Solicitud.", { title: 'Error!' });
                $scope.disbtn = false;
            } else {
                $scope.saveinfgru();
            }
        };

        $scope.saveinfgru = function () {
            //$scope.fl.coninf = $("#coninf").val();
            //$scope.fl.tservicio = 1;
            $scope.fl.Bis = bis;
            $scope.fl.tvh = tvh;
            $scope.fl.dirr = $scope.fl.dirr2;

            var res = solgruasservices.saveinfgru($scope.fl);
            res.success(function (data, status, headers, config) {

                if (data == "El código de infracción no existe, favor validar.") {
                    growl.error(data, { title: 'Error!' });
                    $scope.disbtn = false;
                    return;
                } else {
                    if (data[0] == "0") {
                        for (var i = 1; i < data.length; i++) {
                            growl.error(data[i], { title: 'Error!' });
                        }
                    } else {
                        growl.success(data, { title: 'Succes!' });
                    }    
                    
                    $scope.disbtn = false;
                    pos = 0;
                    conteo = 0;
                }

                $scope.fl = [];

                $scope.fl.causain = "-1";
                $scope.fl.localidad = "";
                $scope.fl.tvprin = "-1";
                $scope.fl.tlprin = "-1";
                $scope.fl.lbvprin = "-1";
                $scope.fl.tvcuadrante = "-1";
                $scope.fl.lvgen = "-1";
                $scope.fl.vgcuadra = "-1";
                $scope.fl.sentidovial = "-1";
                $scope.fl.tgrua = "-1";
                $scope.fl.numgruas = "-1";
                $scope.fl.tvehiculo = "-1";
                $scope.fl.cnvhi = "-1";
                $scope.fl.tservicio = "-1";
                $scope.fl.array = tvh;
                tvh = [];
                bis = "";
                $("#agensearch").text("----");

                $("#add tr").remove();

            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });
        }

        var tvh = [];
        var conteo = 0;
        var clicks = 0;

        $scope.addvehi = function () {

            if ($scope.fl.tvehiculo == "-1") {
                growl.error("Debe seleccionar un tipo de vehículo para poder agregar.", { title: 'Error!' });
            } else if ($scope.fl.cnvhi == "-1") {
                growl.error("Debe seleccionar una cantidad de vehículo para poder agregar.", { title: 'Error!' });
            } else {
                for (var i = 0; i < tvh.length; i++) {
                    if (tvh[i] == undefined) {
                    } else {
                        if (tvh[i].split(':')[0] == $scope.fl.tvehiculo) {
                            growl.error("El vehículo ya se selecciono previamente.", { title: 'Error!' });
                            return;
                        }
                    }
                }

                $("#add").append("<tr conts='" + conteo + "' tvis='" + $scope.fl.tvehiculo + "' valval='" + $scope.fl.tvehiculo + ":" + $scope.fl.cnvhi + "' ><td style='text-align:center'>" + $("#tvlist option:selected")[0].textContent + "</td><td style='text-align:center'>" + $scope.fl.cnvhi + "</td><td style='text-align:center' class='point'><i class='fa fa-minus-circle point' style='pointer:cursor' ></i></td><td class='pointedit' onclik='asd()' style='text-align:center'><i class='fa fa-edit'></i></td></tr>")

                tvh.push($scope.fl.tvehiculo + ":" + $scope.fl.cnvhi);
                conteo = conteo + 1;
            }

            var pos = 0;

            $(".point").unbind("click").click(function () {

                event.stopPropagation();
                pos = $(this).parents('tr').attr('valval');
                var index = tvh.indexOf(pos);
                tvh.splice(index, 1);
                $(this).parents('tr').remove();

            });

            $(".pointedit").unbind("click").click(function () {

                var val1 = $(this).parents('tr').attr('tvis');
                var val2 = $(this).parents('tr').find("td").eq(1).html();

                $scope.fl.tvehiculo = val1;
                $("#tvlist").val(val1)

                $scope.fl.cnvhi = val2;
                $("#numgruas").val(val2);


                event.stopPropagation();
                pos = $(this).parents('tr').attr('valval');
                var index = tvh.indexOf(pos);
                tvh.splice(index, 1);
                $(this).parents('tr').remove();
            });

            $scope.fl.tvehiculo = "-1";
            $("#tvlist").val("-1")
            $scope.fl.cnvhi = "-1";
            $("#numgruas").val("-1");
        }

        $scope.SearchTr = function () {

            var res = solgruasservices.C_Inmovilizaciones($scope.fl.tservicio);
            res.success(function (data, status, headers, config) {
                $scope.fl.causain = "-1";
                $scope.C_Inmovilizaciones = data;
            })
                .error(function (data, status, headers, config) {
                    growl.error(data, { title: 'Error!' });
                });

            if ($scope.fl.tservicio == 4) {
                $scope.CusaDis = true;
                tvh = [];
                $("tbody tr", editable).remove();
                $scope.fl.tvehiculo = "-1";
                $scope.fl.cnvhi = "-1";
                $scope.fl.coninf = "";

            } else {
                $scope.CusaDis = false;
            }


        }

    }]);