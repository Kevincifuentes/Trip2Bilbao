
var ultimaBusqueda = false;
var map;
var markers = [];
var colores = ['#DF0174', '#905c1b', '#000000'];
var iconos = ["imagenes/defecto.png", "imagenes/farmacias.png", "imagenes/parking.png", "imagenes/incidencias.png", "imagenes/centros.png", "imagenes/hospitales.png", "imagenes/bilbobus.png", "imagenes/bizkaibus.png", "imagenes/euskotren.png", "imagenes/metro.png", "imagenes/tranvia.png", "imagenes/puntobici.png"];
var rutas = [];
var infoWindows = [];
var primera = true;
var visible = false;
var anterior = 0;
var pos;
var latlngParking;
var destino;
var noParkings = false;
var tiempoIrParking;
var latlong;
var ultimaBusquedaPorNombre;
var numeroDeParkings = 0;
var latlngParkingUnico;
var conClick = false;
var tambienDestino = false;

function initMap() {
    modoViaje = google.maps.TravelMode.DRIVING;
    map = new google.maps.Map(document.getElementById('map'), {
        zoom: 13,
        center: { lat: 43.271037, lng: -2.938546 }
    });

    var trafficLayer = new google.maps.TrafficLayer();
    trafficLayer.setMap(map);

    geocoder = new google.maps.Geocoder();

    $body = $("body");

    $(document).on({
        ajaxStart: function() { $body.addClass("loading"); },
        ajaxStop: function() { $body.removeClass("loading"); }
    });

    $('input:radio[name=medio]').change(function() {
        if (this.value === 'DRIVING' && ultimaBusqueda === true) {
            if (ultimaBusquedaPorNombre === true) {
                buscarPorNombre();
            } else {
                enrutar(destinoFinal, 0);
            }
        } else if (this.value === 'WALKING' && ultimaBusqueda === true) {
            if (ultimaBusquedaPorNombre === true) {
                buscarPorNombre();
            } else {
                enrutar(destinoFinal, 1);
            }
        }
    });

    $('#btnAceptar').click(function() {
        $('#modalTiempoEx').modal('hide');
        tambienDestino = true;
        if ($('input:radio[name=opcion]:checked').val() === "pie") {
            //alert("a pie");
            enrutarOrigenDestino(latlngParking, destinoFinal, google.maps.TravelMode.WALKING);
            obtenerCodigo(latlngParking);
        } else {

            //alert("en transporte publico");
            enrutarOrigenDestino(latlngParking, destinoFinal, google.maps.TravelMode.TRANSIT);
            obtenerCodigo(latlngParking);
        }
    });

    //AUTOCOMPLETAR

    var input = /** @type {!HTMLInputElement} */(
        document.getElementById('busqueda'));

    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', map);

    autocomplete.addListener('place_changed', function() {
        var place = autocomplete.getPlace();
        if (!place.geometry) {
            window.alert("Autocomplete's returned place contains no geometry");
            return;
        }
    });

    var input2 = /** @type {!HTMLInputElement} */(
        document.getElementById('desde'));

    var autocomplete2 = new google.maps.places.Autocomplete(input2);
    autocomplete.bindTo('bounds', map);

    autocomplete2.addListener('place_changed', function () {
        var place = autocomplete2.getPlace();
        if (!place.geometry) {
            window.alert("Autocomplete's returned place contains no geometry");
            return;
        }
    });

    //BOTÓN ENTER
    $("#busqueda").keyup(function (event) {
        if (event.keyCode == 13) {
            conClick = false;
            $("#btnBuscar").click();
        }
    });
}

function obtener(x, y) {
    if (primera || x != anterior || seleccionar === true || conClick === true) {
        anterior = x;
        primera = false;
        visible = true;
        $(document).ready(function() {
            // Send an AJAX request
            $.getJSON(y)
                .fail(function() {
                    if (x === 1) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Farmacias cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 2) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Parkings cercanos en Destino.");
                        noParkings = true;
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 3) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Incidencias de tráfico cernanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 4) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Centros de salud cercanos en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 5) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Hospitales cercanos en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 6) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Paradas de Bilbobus cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 7) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Paradas de Bizkaibus cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 8) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Paradas de Euskotren cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 9) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Paradas de Metro cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 10) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Paradas de Tranvía cercanas en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    } else if (x === 11) {
                        var node = document.createElement("LI");
                        var textnode = document.createTextNode("No se han encontrado Puntos de Bicis cercanos en Destino.");
                        node.appendChild(textnode);
                        document.getElementById("noencontrado").appendChild(node);
                    }
                })
                .done(function(data) {
                    // On success, 'data' contains a list of products.
                    if (x === 2) {
                        numeroDeParkings = 0;
                    }
                    $.each(data, function(key, item) {
                        if (x === 2) {
                            numeroDeParkings++;
                            latlngParkingUnico = new google.maps.LatLng(item.latitud, item.longitud);
                        }
                        // Add a list item for the product.
                        if (item.latitud > 0 || item.longitud < 0) {
                            var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                            var contentString = formatItem(item, x, y);

                            var infowindow = new google.maps.InfoWindow({
                                content: contentString
                            });

                            var marker = new google.maps.Marker({
                                position: myLatlng,
                                title: contentString,
                                icon: iconos[x]
                            });
                            infoWindows.push(infowindow);
                            markers.push(marker);


                            marker.addListener('click', function(event) {
                                for (var i = 0; i < infoWindows.length; i++) {
                                    infoWindows[i].close();
                                }
                                latlngParking = event.latLng;
                                //alert(latlngParking);
                                infowindow.open(map, marker);
                            });

                            // To add the marker to the map, call setMap();
                            marker.setMap(map);

                            if (x === 2) {
                                noParkings = false;
                            }
                        }

                    });
                }).always(function() {
                    if (x === 2) {
                        resultadoParking();
                    }
                });
        });

    } else {
        if (visible) {
            clearMarkers();
            visible = false;
        } else {
            showMarkers();
            visible = true;
        }
    }
}

function clearMarkers() {
    setMapOnAll(null);
}

// Shows any markers currently in the array.
function showMarkers() {
    setMapOnAll(map);
}

function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

function formatItem(item, x, y) {
    if (x === 1) {
        return "<div><h3>" + item.nombreFarmacia + "</h3><br><b> Contácto:</b> " + item.telefono + "</div>";
    } else if (x === 2) {
        if (y === 'api/parkings/parkings' || seleccionar === true) {
            return "<div><h3><b>Parking:</b> " + item.nombreParking + "</h3><br><b> Capacidad:</b> " + item.capacidad + "<br><button type='button' class='btn btn-info' onclick='seleccionParking()'>Seleccionar Parking</button></div>";
        } else {
            return "<div><h3><b>Parking:</b> " + item.nombreParking + "</h3><br><b> Capacidad:</b> " + item.capacidad + "</div>";
        }
    } else if (x === 3) {
        return "<div><h3><b>" + item.tipo + ":</b> " + item.descripcion + "</h3><br> <b>Inicio:</b> " + item.inicio + " / <b>Fin: </b>" + item.fin + "</div>";
    } else if (x === 4) {
        return "<div><h3><b>Centro de Salud:</b> " + item.nombreCentro + "</h3><br><b>Dirección:</b> " + item.direccionCompleta + "</div>";
    } else if (x === 5) {
        return "<div><h3><b>Hospital:</b> " + item.nombreHospital + "</h3><br><b>Dirección:</b> " + item.direccionCompleta + "</div>";
    } else if (x === 6) {
        return "<div><h3><b>Parada de Bilbobus:</b> " + item.nombreParada + "</h3><br><b>Abreviatura:</b> " + item.abreviatura + "</div>";
    } else if (x === 7) {
        return "<div><h3><b>Parada de Bizkaibus:</b> " + item.nombreParada + "</h3></div>";
    } else if (x === 8) {
        return "<div><h3><b>Parada de Euskotren:</b> " + item.nombreParada + "</h3><br><b>Código de parada:</b> " + item.codigoParada + "</div>";
    } else if (x === 9) {
        return "<div><h3><b>Parada de Metro:</b> " + item.nombreParada + "</h3><br><b>Código de parada:</b> " + item.codigoParada + "</div>";
    } else if (x === 10) {
        return "<div><h3><b>Parada de Tranvía:</b> " + item.nombreParada + "</h3><br><b>Código de parada:</b> " + item.descripcion + "</div>";
    } else if (x === 11) {
        return "<div><h3><b>Punto de bicis:</b> " + item.nombrePunto + "</h3><br><b>Estado:</b> " + item.estado + "<br><b>Capacidad:</b> " + item.capacidad + "</div>";
    }

}

var directionsService, directionsDisplay;
var modoViaje;

function enrutar(latlng, modo) {
    ultimaBusquedaPorNombre = false;
    ultimaBusqueda = true;
    if (seleccionado === false) {
        limpiarRutas();
    }
    if (modo === 1) {
        modoViaje = google.maps.TravelMode.WALKING;
    } else {
        modoViaje = google.maps.TravelMode.DRIVING;
    }

    directionsService = new google.maps.DirectionsService();
    directionsDisplay = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map,
        preserveViewport: false,
        polylineOptions: {
            strokeColor: "black"
        }
    });

    //PARA EL PANEL
    directionsDisplay.setPanel(document.getElementById('right-panel'));

    if (navigator.geolocation) {
        var desde = $("#desde").val();
        var opcion;
        navigator.geolocation.getCurrentPosition(function(position) {
            pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            destino = latlng;

            if (desde === "" || desde === undefined) {
                opcion = pos;
                map.setCenter(pos);
            } else {
                opcion = desde;
            }
            directionsService.route({
                origin: opcion,
                destination: latlng,
                provideRouteAlternatives: true,
                travelMode: modoViaje,
                region: "ES",
                drivingOptions: {
                    departureTime: new Date(Date.now()),
                    trafficModel: "optimistic" //"pessimistic"
                }
            }, function(response, status) {
                if (status === google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                    tiempoIrParking = response.routes[0].legs[0].duration.value;
                    //alert(tiempoIrParking);
                    //alert("Número de rutas: " + response.routes.length);
                    for (z = 0; z < response.routes.length; z++) {

                        var polyline = new google.maps.Polyline({
                            path: [],
                            strokeColor: colores[z],
                            strokeOpacity: 0.3,
                            strokeWeight: 5
                        });

                        var bounds = new google.maps.LatLngBounds();
                        var legs = response.routes[z].legs;
                        for (i = 0; i < legs.length; i++) {
                            var steps = legs[i].steps;
                            for (j = 0; j < steps.length; j++) {
                                var nextSegment = steps[j].path;
                                for (k = 0; k < nextSegment.length; k++) {
                                    polyline.getPath().push(nextSegment[k]);
                                    bounds.extend(nextSegment[k]);
                                }
                            }
                        }
                        polyline.setMap(map);
                        rutas.push(polyline);
                    }
                    if (seleccionado === false) {
                        //alert(latlng);
                        obtenerCodigo(latlng);
                    } else {
                        //alert(latlng);
                        obtenerInformacionDistanciasTiempos(latlng, destinoFinal);
                    }
                } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }, function() {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        directionsService.route({
            origin: new google.maps.LatLng(43.271142, -2.938802),
            destination: new google.maps.LatLng(43.259995, -2.946041),
            provideRouteAlternatives: true,
            travelMode: google.maps.TravelMode.DRIVING,
            region: "ES",
            drivingOptions: {
                departureTime: new Date(Date.now()),
                trafficModel: "optimistic" //"pessimistic"
            }
        }, function(response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                directionsDisplay.setDirections(response);
                //alert("Número de rutas: " + response.routes.length);
                for (z = 0; z < response.routes.length; z++) {

                    var polyline = new google.maps.Polyline({
                        path: [],
                        strokeColor: colores[z],
                        strokeOpacity: 0.3,
                        strokeWeight: 5
                    });

                    var bounds = new google.maps.LatLngBounds();
                    var legs = response.routes[z].legs;
                    for (i = 0; i < legs.length; i++) {
                        var steps = legs[i].steps;
                        for (j = 0; j < steps.length; j++) {
                            var nextSegment = steps[j].path;
                            for (k = 0; k < nextSegment.length; k++) {
                                polyline.getPath().push(nextSegment[k]);
                                bounds.extend(nextSegment[k]);
                            }
                        }
                    }
                    polyline.setMap(map);
                }
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

//google.maps.event.addDomListener(window, "load", initMap);


var habilitado = false;

function habilitarEnrutado() {
    if (habilitado === true) {
        $("#habilitarEnrutado").html('Habilitar encontrar ruta');
        habilitado = false;
        conClick = false;
        google.maps.event.clearListeners(map, 'click');
    } else {
        conClick = true;
        google.maps.event.addListener(map, 'click', function(event) {
            limpiarRutas();
            destinoFinal = event.latLng;
            if (document.getElementById('pie').checked) {
                enrutar(event.latLng, 1);
            } else {
                destinoFinal = event.latLng;
                obtenerCodigo(event.latLng);
            }

        });
        habilitado = true;
        $("#habilitarEnrutado").html('Deshabilitar encontrar ruta');
    }
}

function limpiarRutas() {

    if (directionsDisplay != undefined) {
        directionsDisplay.setMap(null);
    }

    if (otro2 != undefined) {
        otro2.setMap(null);
    }


    for (i = 0; i < rutas.length; i++) {
        rutas[i].setMap(null);
    }

    document.getElementById("right-panel").innerHTML = "";

    clearMarkers();
    markers = [];

    var myNode = document.getElementById("noencontrado");
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }
}

function despejarParaEnrutadoDoble() {

    if (directionsDisplay != undefined) {
        directionsDisplay.setMap(null);
    }

    if (otro2 != undefined) {
        otro2.setMap(null);
    }


    for (i = 0; i < rutas.length; i++) {
        rutas[i].setMap(null);
    }

    document.getElementById("right-panel").innerHTML = "";

    var myNode = document.getElementById("noencontrado");
    while (myNode.firstChild) {
        myNode.removeChild(myNode.firstChild);
    }
}

function buscarPorNombre() {
    conClick = false;
    ultimaBusquedaPorNombre = true;
    ultimaBusqueda = true;
    var introducido = $("#busqueda").val();

    if (introducido === "" || introducido === undefined) {

        $('#errorRuta').modal('show');
    } else {
        ultimaBusquedaPorNombre = true;
        limpiarRutas();

        if (document.getElementById('pie').checked) {
            modoViaje = google.maps.TravelMode.WALKING;
        } else {
            modoViaje = google.maps.TravelMode.DRIVING;
        }

        directionsService = new google.maps.DirectionsService();
        directionsDisplay = new google.maps.DirectionsRenderer({
            draggable: true,
            map: map,
            preserveViewport: false,
            polylineOptions: {
                strokeColor: "black"
            }
        });

        //PARA EL PANEL
        directionsDisplay.setPanel(document.getElementById('right-panel'));


        if (navigator.geolocation) {
            var desde = $("#desde").val();
            var opcion;
                
            navigator.geolocation.getCurrentPosition(function(position) {
                pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                var lugar = introducido;
                if (desde === "" || desde === undefined) {
                    opcion = pos;
                    map.setCenter(pos);
                } else {
                    opcion = desde;
                }
                alert(opcion);
                directionsService.route({
                    origin: opcion,
                    destination: lugar,
                    provideRouteAlternatives: true,
                    travelMode: modoViaje,
                    region: "ES",
                    drivingOptions: {
                        departureTime: new Date(Date.now()),
                        trafficModel: "optimistic" //"pessimistic"
                    }
                }, function(response, status) {
                    if (status === google.maps.DirectionsStatus.OK) {
                        directionsDisplay.setDirections(response);
                        //alert("Número de rutas: " + response.routes.length);
                        for (z = 0; z < response.routes.length; z++) {

                            var polyline = new google.maps.Polyline({
                                path: [],
                                strokeColor: colores[z],
                                strokeOpacity: 0.3,
                                strokeWeight: 5
                            });

                            var bounds = new google.maps.LatLngBounds();
                            var legs = response.routes[z].legs;
                            for (i = 0; i < legs.length; i++) {
                                var steps = legs[i].steps;
                                for (j = 0; j < steps.length; j++) {
                                    var nextSegment = steps[j].path;
                                    for (k = 0; k < nextSegment.length; k++) {
                                        polyline.getPath().push(nextSegment[k]);
                                        bounds.extend(nextSegment[k]);
                                    }
                                }
                            }
                            polyline.setMap(map);
                            rutas.push(polyline);
                        }

                        //obtener el punto
                        geocoder.geocode({ 'address': lugar }, function(results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                //alert(results[0].geometry.location);
                                destinoFinal = results[0].geometry.location;
                                obtenerCodigo(results[0].geometry.location);
                            } else {
                                alert("No se pudo obtener el punto exacto por: " + status);
                            }
                        });
                    } else {
                        window.alert('La petición no pudo ser realizada porque: ' + status);
                    }
                });
            }, function() {
                handleLocationError(true, infoWindow, map.getCenter());
            });
        } else {
            alert("La geolocalización no está habilitado o soportada.");
        }
    }

}

function obtenerInfoCodigo(codigo) {

    if (modoViaje === google.maps.TravelMode.WALKING || seleccionado === true) {

        obtener(1, 'api/farmacias/codigo/' + codigo);
        obtener(4, 'api/centros/codigo/' + codigo);
        obtener(5, 'api/hospitales/codigo/' + codigo);
        obtener(6, 'api/paradasbilbo/codigo/' + codigo);
        obtener(7, 'api/paradasbizkaibus/codigo/' + codigo);
        obtener(8, 'api/paradaseuskotren/codigo/' + codigo);
        obtener(9, 'api/paradasmetro/codigo/' + codigo);
        obtener(10, 'api/paradastranvia/codigo/' + codigo);
        obtener(11, 'api/puntosbici/codigo/' + codigo);
        if (tambienDestino === true) {
            tambienDestino = false;
            obtenerInfoDePosicion(destinoFinal);
        }
        seleccionado = false;
    } else {
        codigoPostalParking = codigo;
        obtener(2, 'api/parkings/codigo/' + codigo);
        /*obtener(1, 'api/farmacias/codigo/' + codigo);
        obtener(3, 'api/incidencias/codigo/' + codigo);
        obtener(4, 'api/centros/codigo/' + codigo);
        obtener(5, 'api/hospitales/codigo/' + codigo);
        obtener(6, 'api/paradasbilbo/codigo/' + codigo);
        obtener(7, 'api/paradasbizkaibus/codigo/' + codigo);
        obtener(8, 'api/paradaseuskotren/codigo/' + codigo);
        obtener(9, 'api/paradasmetro/codigo/' + codigo);
        obtener(10, 'api/paradastranvia/codigo/' + codigo);
        obtener(11, 'api/puntosbici/codigo/' + codigo);*/
    }
}

var codigoPostal;
var geocoder;

function obtenerCodigo(latilong) {
    destino = latilong;
    geocoder.geocode({ 'location': latilong }, function(results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            if (results[0].address_components[6] === undefined) {
                codigoPostal = results[0].address_components[5].long_name;
            } else {
                codigoPostal = results[0].address_components[6].long_name;
            }
            //alert(codigoPostal);
            obtenerInfoCodigo(codigoPostal);
        } else {
            alert("No pudo obtenerse el código postal por: " + status);
        }
    });

}

function obtenerInfoDePosicion(latilong) {
    var cp;
    geocoder.geocode({ 'location': latilong }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0].address_components[6] === undefined) {
                cp = results[0].address_components[5].long_name;
            } else {
                cp = results[0].address_components[6].long_name;
            }
            obtener(1, 'api/farmacias/codigo/' + cp);
            obtener(4, 'api/centros/codigo/' + cp);
            obtener(5, 'api/hospitales/codigo/' + cp);
            obtener(6, 'api/paradasbilbo/codigo/' + cp);
            obtener(7, 'api/paradasbizkaibus/codigo/' + cp);
            obtener(8, 'api/paradaseuskotren/codigo/' + cp);
            obtener(9, 'api/paradasmetro/codigo/' + cp);
            obtener(10, 'api/paradastranvia/codigo/' + cp);
            obtener(11, 'api/puntosbici/codigo/' + cp);
        } else {
            alert("No pudo obtenerse el código postal por: " + status);
            return null;
        }
    });

}

var seleccionado = false;
var seleccionar = false;
var codigoPostalParking;
var codigoPostalDestino;
function resultadoParking() {
    if (noParkings === false) {
        if (seleccionar === true) {
            codigoPostalParking = codigoPostal;
            seleccionar = false;
        } else {
            if (numeroDeParkings === 1) {
                //Realizar ruta a dicho parking y después al destino
                latlngParking = latlngParkingUnico;
                seleccionParking();
            } else {
                //permitir elegir entre los parkings
                seleccionar = true;
                $("body").removeClass("loading");
                $('#variosParkings').modal('show');
                obtener(2, 'api/parkings/codigo/' + codigoPostal);
            }
            obtener(1, 'api/farmacias/codigo/' + codigoPostalParking);
            obtener(3, 'api/incidencias/codigo/' + codigoPostalParking);
            obtener(4, 'api/centros/codigo/' + codigoPostalParking);
            obtener(5, 'api/hospitales/codigo/' + codigoPostalParking);
            obtener(11, 'api/puntosbici/codigo/' + codigoPostalParking);
        }
    } else {
        seleccionar = true;
        $("body").removeClass("loading");
        $('#noParking').modal('show');
        obtener(2, 'api/parkings/parkings');
    }
}

var destinoFinal;

function seleccionParking() {
    for (var i = 0; i < infoWindows.length; i++) {
        infoWindows[i].close();
    }
    //alert(latlngParking);
    seleccionado = true;
    despejarParaEnrutadoDoble();
    enrutar(latlngParking, 0);
}

var otro, otro2;

function enrutarOrigenDestino(origen, destino, modo) {
    ultimaBusqueda = true;
    if (seleccionado === false) {
        limpiarRutas();
    }


    otro = new google.maps.DirectionsService();
    otro2 = new google.maps.DirectionsRenderer({
        draggable: true,
        map: map,
        preserveViewport: false,
        polylineOptions: {
            strokeColor: "black"
        }
    });

    //PARA EL PANEL
    otro2.setPanel(document.getElementById('right-panel'));


    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
            pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            map.setCenter(pos);
            otro.route({
                origin: origen,
                destination: destino,
                provideRouteAlternatives: true,
                travelMode: modo,
                region: "ES",
                drivingOptions: {
                    departureTime: new Date(Date.now()),
                    trafficModel: "optimistic" //"pessimistic"
                }
            }, function(response, status) {
                if (status === google.maps.DirectionsStatus.OK) {
                    otro2.setDirections(response);
                    //alert("Número de rutas: " + response.routes.length);
                    for (z = 0; z < response.routes.length; z++) {

                        var polyline = new google.maps.Polyline({
                            path: [],
                            strokeColor: "#0000ff",
                            strokeOpacity: 0.3,
                            strokeWeight: 5
                        });

                        var bounds = new google.maps.LatLngBounds();
                        var legs = response.routes[z].legs;
                        for (i = 0; i < legs.length; i++) {
                            var steps = legs[i].steps;
                            for (j = 0; j < steps.length; j++) {
                                var nextSegment = steps[j].path;
                                for (k = 0; k < nextSegment.length; k++) {
                                    polyline.getPath().push(nextSegment[k]);
                                    bounds.extend(nextSegment[k]);
                                }
                            }
                        }
                        polyline.setMap(map);
                        rutas.push(polyline);
                    }
                } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }, function() {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        otro.route({
            origin: new google.maps.LatLng(43.271142, -2.938802),
            destination: new google.maps.LatLng(43.259995, -2.946041),
            provideRouteAlternatives: true,
            travelMode: google.maps.TravelMode.DRIVING,
            region: "ES",
            drivingOptions: {
                departureTime: new Date(Date.now()),
                trafficModel: "optimistic" //"pessimistic"
            }
        }, function(response, status) {
            if (status === google.maps.DirectionsStatus.OK) {
                otro2.setDirections(response);
                //alert("Número de rutas: " + response.routes.length);
                for (z = 0; z < response.routes.length; z++) {

                    var polyline = new google.maps.Polyline({
                        path: [],
                        strokeColor: colores[z],
                        strokeOpacity: 0.3,
                        strokeWeight: 5
                    });

                    var bounds = new google.maps.LatLngBounds();
                    var legs = response.routes[z].legs;
                    for (i = 0; i < legs.length; i++) {
                        var steps = legs[i].steps;
                        for (j = 0; j < steps.length; j++) {
                            var nextSegment = steps[j].path;
                            for (k = 0; k < nextSegment.length; k++) {
                                polyline.getPath().push(nextSegment[k]);
                                bounds.extend(nextSegment[k]);
                            }
                        }
                    }
                    polyline.setMap(map);
                }
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        });
        handleLocationError(false, infoWindow, map.getCenter());
    }

}

function obtenerInformacionDistanciasTiempos(origin, destiny) {
    //seleccionado = false;
    //informacion andando
    var service = new google.maps.DistanceMatrixService();
    service.getDistanceMatrix(
    {
        origins: [origin],
        destinations: [destiny],
        travelMode: google.maps.TravelMode.TRANSIT
    }, function(response, status) {
        if (status == google.maps.DistanceMatrixStatus.OK) {
            $("#ttransporte").html(response.rows[0].elements[0].duration.text);
            $("#dtransporte").html(response.rows[0].elements[0].distance.text);
        }
    });
    service.getDistanceMatrix(
    {
        origins: [origin],
        destinations: [destiny],
        travelMode: google.maps.TravelMode.WALKING
    }, function(response, status) {
        if (status == google.maps.DistanceMatrixStatus.OK) {
            var distanciaMetros = response.rows[0].elements[0].distance.value;
            var tiempoSecs = response.rows[0].elements[0].duration.value;
            $("#tpie").html(response.rows[0].elements[0].duration.text);
            $("#dpie").html(response.rows[0].elements[0].distance.text);
            if (tiempoSecs > (tiempoIrParking / 2) || distanciaMetros > 500) {
                $('#modalTiempoEx').modal('show');
            } else {
                //alert(tiempoSecs + " > " + (tiempoIrParking / 2));
                enrutarOrigenDestino(origin, destiny, google.maps.TravelMode.WALKING);
                obtenerCodigo(latlngParking);
            }
        }
    });
}