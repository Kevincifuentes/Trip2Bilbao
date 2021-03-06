﻿
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
var noHayMeteo = true;
var ultimaBusquedaPorNombre;
var numeroDeParkings = 0;
var latlngParkingUnico;
var conClick = false;
var tambienDestino = false;
var lineasBilbobusArray = {};

var paradasConTiempo = {};
var estadoParkings = {};
var estadosBici = {};

var primeraTiemposLinea = true;
var puntos = [];
var informacionParadas = {};
var ruta;
var idRuta;
var paradas = [];
var mostrando;
var informacionSobreParadas = {};

window.initMap = function() {
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

    inicializarActiveMQ();
    obtenerLineasEuskotren();
    obtenerLineasBizkaibus();
}

function obtener(x, y) {
    $('#lineaeleg').html("");
    if (primera || x != anterior || seleccionar === true || conClick === true) {
        anterior = x;
        primera = false;
        visible = true;
        $(document).ready(function() {
            //se envia una petición AJAX
            $.getJSON(y)
                .done(function (data) {
                    //si la petición ha sido correcta, tendremos una lista de objetos JSON
                    if (x === 2) {
                        numeroDeParkings = 0;
                    }
                    $.each(data, function (key, item) {
                        if (x === 2) {
                            numeroDeParkings++;
                            latlngParkingUnico = new google.maps.LatLng(item.latitud, item.longitud);
                        }

                        // Si la posición es correcta, se añade un marcador al mapa
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

                            //se añade un evento al marcador
                            marker.addListener('click', function (event) {
                                for (var i = 0; i < infoWindows.length; i++) {
                                    infoWindows[i].close();
                                }
                                latlngParking = event.latLng;
                                //alert(latlngParking);
                                infowindow.open(map, marker);
                            });

                            marker.setMap(map);

                            if (x === 2) {
                                noParkings = false;
                            }
                        }

                    });
                }).fail(function () {
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
                }).always(function () {
                    if (x === 2) {
                        console.log(numeroDeParkings);
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
        return "<div><h3>" + item.nombreFarmacia + "</h3><br><b> Dirección:</b> " + item.direccionAbreviada + "<br><b> Contácto:</b> " + item.telefono + "</div>";
    } else if (x === 2) {
        if (y === 'api/parkings/parkings' || seleccionar === true) {
            return "<div><h3><b>Parking:</b> " + item.nombreParking + "</h3><br><b> Capacidad: </b> " + item.capacidad + "<br><h4> Disponibilidad actual: </b>"+estadoParkings[item.nombreParking]+"<br><button type='button' class='btn btn-info' onclick='seleccionParking()'>Seleccionar Parking</button></div>";
        } else {
            return "<div><h3><b>Parking:</b> " + item.nombreParking + "</h3><br><b> Capacidad:</b> " + item.capacidad + "</div>";
        }
    } else if (x === 3) {
        return "<div><h3><b>" + item.tipo + ":</b> " + item.descripcion + "</h3><br> <b>Inicio:</b> " + item.inicio + " / <b>Fin: </b>" + item.fin + "</div>";
    } else if (x === 4) {
        return "<div><h3><b>Centro de Salud:</b> " + item.nombreCentro + "</h3><br><b>Dirección:</b> " + item.direccionCompleta + "</div>";
    } else if (x === 5) {
        return "<div><h3><b>Hospital:</b> " + item.nombreHospital + "</h3><br><b>Dirección:</b> " + item.direccionCompleta + "<br><b>Teléfono</b>" + item.telefono + "<br><a href='" + item.web + "' class='button' target='_blank'><b>Página web:</b></a></div>";
    } else if (x === 6) {
        var inicial = "<div><h3><b>Parada de Bilbobus:</b> " + item.nombreParada + "</h3><br><b>Abreviatura:</b><table border='1' style='width:100%' border-spacing='5px' ><tr><th>Id de Linea</th><th>Nombre de Linea</th><th>Tiempo Restante</th></tr>" + item.abreviatura;
        if (item.id in informacionSobreParadas) {
            //Poner tiempos
            var tiempos = informacionSobreParadas[item.id];
            for (var i = 0; i < tiempos.length; i++) {
                inicial = inicial + "<tr><td>" + tiempos[i].id + "</td><td>" + tiempos[i].linea + "</td><td>" + tiempos[i].tiempo + "</td></tr>";
            }
        }
        inicial = inicial +"</table></div>";
        return inicial;
    } else if (x === 7) {
        return "<div><h3><b>Parada de Bizkaibus:</b> " + item.nombreParada + "</h3></div>";
    } else if (x === 8) {
        return "<div><h3><b>Parada de Euskotren:</b> " + item.nombreParada + "</h3><br><b>Código de parada:</b> " + item.codigoParada + "</div>";
    } else if (x === 9) {
        return "<div><h3><b>Parada de Metro:</b> " + item.nombreParada + "</h3><br><b>Código de parada:</b> " + item.codigoParada + "<br><button type='button' class='btn btn-info' onclick='mostrarInfoMetro()'>Más información...</button></div>";
    } else if (x === 10) {
        return "<div><h3><b>Parada de Tranvía:</b> " + item.nombreParada + "</h3><br><b>Localización:</b> " + item.descripcion + "</div>";
    } else if (x === 11) {
        return "<div><h3><b>Punto de bicis:</b> " + item.nombrePunto + "</h3><br><b>Estado:</b> " + item.estado + "<br><b>Capacidad:</b> " + item.capacidad + "<br><h4>Disponibilidad en Tiempo Real</h4>"+estadosBici[item.nombrePunto]+"</div>";
    }

}

var directionsService, directionsDisplay;
var modoViaje;

function enrutar(latlng, modo) {
    $('#lineaeleg').html("");
    mostrando = false;
    puntos = [];
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
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
    $('#lineaeleg').html("");
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
    $('#lineaeleg').html("");
    mostrando = false;
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

        //para mostrar las direcciones en el panel con dicho Id
        directionsDisplay.setPanel(document.getElementById('right-panel'));


        if (navigator.geolocation) {
            var desde = $("#desde").val();
            var opcion;
            
            //obtención de la posición del usuario
            navigator.geolocation.getCurrentPosition(function(position) {
                pos = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                var lugar = introducido;

                //según el origen, se geoposiciona o será el introducido
                if (desde === "" || desde === undefined) {
                    opcion = pos;
                    map.setCenter(pos);
                } else {
                    opcion = desde;
                }

                //peticion para búsqueda de ruta
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
                        //se añaden las direcciones al panel y se dibuja sobre el mapa las rutas
                        directionsDisplay.setDirections(response);
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

                        //obtener el punto del lugar introducido
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
            if (results[0].address_components[5].types[0] === "country" && results[0].address_components[6] === undefined) {
                if (results[1].address_components[5].types[0] === "postal_code") {
                    codigoPostal = results[1].address_components[5].long_name;
                } else {
                    if (results[1].address_components[6].types[0] === "postal_code") {
                        codigoPostal = results[1].address_components[6].long_name;
                    } else {
                        if (results[1].address_components[7].types[0] === "postal_code") {
                            codigoPostal = results[1].address_components[7].long_name;
                        } else {
                            codigoPostal = results[1].address_components[8].long_name;
                        }
                    }
                }
            } else {
                if (results[0].address_components[5].types[0] === "postal_code") {
                    codigoPostal = results[0].address_components[5].long_name;
                } else {
                    if (results[0].address_components[6].types[0] === "postal_code") {
                        codigoPostal = results[0].address_components[6].long_name;
                    } else {
                        if (results[0].address_components[7].types[0] === "postal_code") {
                            codigoPostal = results[0].address_components[7].long_name;
                        } else {
                            codigoPostal = results[0].address_components[8].long_name;
                        }
                    }
                }
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
    console.log("No PARKING: "+ noParkings);
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
    $('#lineaeleg').html("");
    mostrando = false;
    puntos = [];
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
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

function inicializarActiveMQ() {
    //Inicializo el WebSocket al puerto e Ip del ActiveMQ. Se utilizará un servicio STOMP.
    ws = new WebSocket('ws://localhost:61614', 'stomp');

    //Notificar para la conexión
    ws.onopen = function (){
        ws.send('CONNECT\n\n\0');
 
    //Notificar que nos suscribimos y a que nos suscribimos
        ws.send('SUBSCRIBE\ndestination:/topic/Trip2Bilbao\nack:auto\nactivemq.retroactive:true\n\n\0');
    };
    //En caso de mensaje de ejecutará la función
    ws.onmessage = function (e) {
        //Se comprueba que sea un mensaje de datos
        if (e.data.startsWith('MESSAGE')) {

            var lines = e.data.split('\n');
            var tipo = lines[2].substring(lines[2].indexOf(":") + 1, lines[2].length);

            var expirado = lines[5].substring(0, lines[5].indexOf(":"));

            if (expirado !== "originalExpiration") {
                var expirado = lines[6].substring(0, lines[6].indexOf(":"));
                if (expirado !== "originalExpiration") {
                    var expirado = lines[7].substring(0, lines[7].indexOf(":"));

                    if (expirado !== "originalExpiration") {

                        var parser, xmlDoc;
                        //Según el tipo se realiza un procesamiento
                        //console.log(tipo);
                        switch (tipo) {
                            case "TiempoCiudad":
                                if (window.DOMParser) {
                                    parser = new DOMParser();
                                    xmlDoc = parser.parseFromString(lines[8], "text/xml");
                                    var nombreCiudad = xmlDoc.getElementsByTagName("Nombre")[0].childNodes[0].nodeValue;
                                    var descripcionGeneralHES = xmlDoc.getElementsByTagName("ES")[0].childNodes[0].nodeValue;
                                    var descripcionGeneralHEU = xmlDoc.getElementsByTagName("EU")[0].childNodes[0].nodeValue;
                                    var descripcionHES = xmlDoc.getElementsByTagName("DescripcionES")[0].childNodes[0].nodeValue;
                                    var descripcionHEU = xmlDoc.getElementsByTagName("DescripcionEU")[0].childNodes[0].nodeValue;
                                    var tempMaxH = xmlDoc.getElementsByTagName("TempMax")[0].childNodes[0].nodeValue;
                                    var tempMinH = xmlDoc.getElementsByTagName("TempMin")[0].childNodes[0].nodeValue;

                                    var descripcionGeneralMES = xmlDoc.getElementsByTagName("ES")[1].childNodes[0].nodeValue;
                                    var descripcionGeneralMEU = xmlDoc.getElementsByTagName("EU")[1].childNodes[0].nodeValue;
                                    var descripcionMES = xmlDoc.getElementsByTagName("DescripcionES")[1].childNodes[0].nodeValue;
                                    var descripcionMEU = xmlDoc.getElementsByTagName("DescripcionEU")[1].childNodes[0].nodeValue;
                                    var tempMaxM = xmlDoc.getElementsByTagName("TempMax")[1].childNodes[0].nodeValue;
                                    var tempMinM = xmlDoc.getElementsByTagName("TempMin")[1].childNodes[0].nodeValue;

                                    var descripcionGeneralPES = xmlDoc.getElementsByTagName("ES")[2].childNodes[0].nodeValue;
                                    var descripcionGeneralPEU = xmlDoc.getElementsByTagName("EU")[2].childNodes[0].nodeValue;
                                    var descripcionPES = xmlDoc.getElementsByTagName("DescripcionES")[2].childNodes[0].nodeValue;
                                    var descripcionPEU = xmlDoc.getElementsByTagName("DescripcionEU")[2].childNodes[0].nodeValue;
                                    var tempMaxP = xmlDoc.getElementsByTagName("TempMax")[2].childNodes[0].nodeValue;
                                    var tempMinP = xmlDoc.getElementsByTagName("TempMin")[2].childNodes[0].nodeValue;

                                    if (noHayMeteo === true) {
                                        $("#hoy").html(" <table class='table table-bordered'><tbody><td><img height='50' width='50' src='imagenes/cold.png'/><span id='spanHmin'>Min.<b>" + tempMinH + "°C</b></span></td><td><img height='50' width='50' src='imagenes/hot.png'/><span id='spanHmax'>Min.<b>" + tempMaxH + "°C</b></span></td></tbody></table>");
                                        $("#manana").html(" <table class='table table-bordered'><tbody><td><img height='50' width='50' src='imagenes/cold.png'/><span id='spanMmin'>Min.<b>" + tempMinM + "°C</b></span></td><td><img height='50' width='50' src='imagenes/hot.png'/><span id='spanMmax'>Min.<b>" + tempMaxM + "°C</b></span></td></tbody></table>");
                                        $("#pasado").html(" <table class='table table-bordered'><tbody><td><img height='50' width='50' src='imagenes/cold.png'/><span id='spanPmin'>Min.<b>" + tempMinP + "°C</b></span></td><td><img height='50' width='50' src='imagenes/hot.png'/><span id='spanPmax'>Min.<b>" + tempMaxP + "°C</b></span></td></tbody></table>");
                                        $("#esHoy").html("<p>" + descripcionGeneralHES + "<br><b>" + descripcionHES + "</b></p>");
                                        $("#euHoy").html("<p>" + descripcionGeneralHEU + "<br><b>" + descripcionHEU + "</b></p>");
                                        $("#esMa").html("<p>" + descripcionGeneralMES + "<br><b>" + descripcionMES + "</b></p>");
                                        $("#euMa").html("<p>" + descripcionGeneralMEU + "<br><b>" + descripcionMEU + "</b></p>");
                                        $("#esPa").html("<p>" + descripcionGeneralPES + "<br><b>" + descripcionPES + "</b></p>");
                                        $("#euPa").html("<p>" + descripcionGeneralPEU + "<br><b>" + descripcionPEU + "</b></p>");
                                        noHayMeteo = false;
                                    } else {
                                        $("#spanHmin").html("Min.<b>" + tempMinH + "°C</b>");
                                        $("#spanHmax").html("Max.<b>" + tempMaxH + "°C</b>");
                                        $("#spanMmin").html("Min.<b>" + tempMinM + "°C</b>");
                                        $("#spanMmax").html("Max.<b>" + tempMaxM + "°C</b>");
                                        $("#spanPmin").html("Min.<b>" + tempMinP + "°C</b>");
                                        $("#spanPmax").html("Max.<b>" + tempMaxP + "°C</b>");
                                        $("#esHoy").html("<p>" + descripcionGeneralHES + "<br><b>" + descripcionHES + "</b></p>");
                                        $("#euHoy").html("<p>" + descripcionGeneralHEU + "<br><b>" + descripcionHEU + "</b></p>");
                                        $("#esMa").html("<p>" + descripcionGeneralMES + "<br><b>" + descripcionMES + "</b></p>");
                                        $("#euMa").html("<p>" + descripcionGeneralMEU + "<br><b>" + descripcionMEU + "</b></p>");
                                        $("#esPa").html("<p>" + descripcionGeneralPES + "<br><b>" + descripcionPES + "</b></p>");
                                        $("#euPa").html("<p>" + descripcionGeneralPEU + "<br><b>" + descripcionPEU + "</b></p>");
                                    }
                                }
                                break;
                            case "TiemposLinea":
                                if (window.DOMParser) {
                                    parser = new DOMParser();
                                    xmlDoc = parser.parseFromString(lines[9], "text/xml");
                                    var id = xmlDoc.getElementsByTagName("TiemposLinea")[0].getAttribute("Id");
                                    var nombre = xmlDoc.getElementsByTagName("TiemposLinea")[0].getAttribute("Nombre");
                                    if (primeraTiemposLinea === true) {
                                        primeraTiemposLinea = false;
                                        $("#columna3").empty();
                                        $("#columna2").empty();
                                        $("#columna1").empty();

                                    }
                                    if (!(id in lineasBilbobusArray)) {
                                        if (Object.keys(lineasBilbobusArray).length > 14) {
                                            if (Object.keys(lineasBilbobusArray).length > 29) {
                                                $("#columna3").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerRuta(&quot;" + id + "&quot;,&quot;" + nombre + "&quot;);'>" + id + " : " + nombre + "</a></li>");
                                            } else {
                                                $("#columna2").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerRuta(&quot;" + id + "&quot;,&quot;" + nombre + "&quot;);'>" + id + " : " + nombre + "</a></li>");
                                            }
                                        } else {
                                            $("#columna1").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerRuta(&quot;" + id + "&quot;,&quot;" + nombre + "&quot;);'>" + id + " - " + nombre + "</a></li>");
                                        }
                                    }
                                    lineasBilbobusArray[id] = xmlDoc;
                                    if (mostrando === true) {
                                        //actualizar
                                        if (idRuta === id) {
                                            obtenerTiemposParadas(id);
                                            for (var i = 0; i < paradas.length; i++) {
                                                if (paradas[i].id in paradasConTiempo) {
                                                    //console.log(paradas[i].nombreParada + " Nuevo tiempo: " + paradasConTiempo[paradas[i].id]);
                                                    informacionParadas[paradas[i].id].setContent(formatearParada(paradas[i], paradas[i].id));
                                                }
                                            }
                                        }
                                    }

                                    var paradas = xmlDoc.getElementsByTagName("Paradas")[0].childNodes;
                                    for (var i = 0; i < paradas.length; i++) {
                                        var idP = paradas[i].getElementsByTagName("Id")[0].childNodes[0].nodeValue;
                                        var tiempo = paradas[i].getElementsByTagName("TiempoRestante")[0].childNodes[0].nodeValue;
                                        //console.log(idP + " " + tiempo);
                                        if (idP in informacionSobreParadas) {
                                            informacionSobreParadas[idP].push({ id: id, linea: nombre, tiempo: tiempo });
                                        } else {
                                            informacionSobreParadas[idP] = [];
                                            informacionSobreParadas[idP].push({ id: id, linea: nombre, tiempo: tiempo });
                                        }
                                    }
                                }
                                break;
                            case "Parkings":
                                if (window.DOMParser) {
                                    parser = new DOMParser();
                                    //console.log(lines[10]);
                                    xmlDoc = parser.parseFromString(lines[9], "text/xml");
                                    nombre = xmlDoc.getElementsByTagName("Nombre")[0].childNodes[0].nodeValue;
                                    var disponibilidad = xmlDoc.getElementsByTagName("Disponibilidad")[0].childNodes[0].nodeValue;
                                    estadoParkings[nombre] = disponibilidad;
                                    //console.log(nombre + " " +disponibilidad);
                                }
                                break;

                            case "Deusto":
                                if (window.DOMParser) {
                                    parser = new DOMParser();
                                    xmlDoc = parser.parseFromString(lines[9], "text/xml");
                                    var general = xmlDoc.getElementsByTagName("General")[0].childNodes[0].nodeValue;
                                    var dbs = xmlDoc.getElementsByTagName("Dbs")[0].childNodes[0].nodeValue;
                                    estadoParkings["UD: DBS"] = dbs;
                                    estadoParkings["UD: General"] = general;
                                }
                                break;

                            case "Bicis":
                                if (window.DOMParser) {
                                    //Lo convertimos mediante un parseador
                                    parser = new DOMParser();
                                    //Obtenemos solo el XML
                                    xmlDoc = parser.parseFromString(lines[9], "text/xml");
                                    //Se obtienen los datos que interesan
                                    nombre = xmlDoc.getElementsByTagName("Nombre")[0].childNodes[0].nodeValue;
                                    var disponibilidadbicis = xmlDoc.getElementsByTagName("BicisLibres")[0].childNodes[0].nodeValue;
                                    var disponibilidadAnclajes = xmlDoc.getElementsByTagName("DisponibilidadAnclaje")[0].childNodes[0].nodeValue;
                                    estadosBici[nombre] = "<b>Bicis Libres: </b>" + disponibilidadbicis + " / <b> Anclajes Libres: </b>" + disponibilidadAnclajes;
                                }
                                break;
                            default:
                        }
                    }

                }
            }

            
        }

    };
    
}

function mostrarMasInfo() {
    $('#meteoInfo').modal('show');
}

function obtenerRuta(id, nombre) {
    $('#lineaeleg').html("Linea Bilbobus "+id+" : "+ nombre);
    puntos = [];
    limpiarRutas();
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
    var anteriorMarcador;
    //alert("Bien " + id);
    if (idRuta !== id) {
        paradasConTiempo = {};
        obtenerTiemposParadas(id);
        idRuta = id;
    }
    obtenerTiemposParadas(id);
    $.getJSON("api/paradasbilbo/linea/"+id)
                .done(function(data) {
                    $.each(data, function(key, item) {

                        // Si la posición es correcta, se añade un marcador al mapa
                            var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                            var contentString = formatearParada(item, id);

                            var infowindow = new google.maps.InfoWindow({
                                content: contentString
                            });

                            var marker = new google.maps.Marker({
                                position: myLatlng,
                                title: contentString,
                                icon: iconos[6]
                            });
                            infoWindows.push(infowindow);
                            informacionParadas[item.id] = infowindow;
                            paradas.push(item);
                            markers.push(marker);

                            //se añade un evento al marcador
                            marker.addListener('click', function(event) {
                                for (var i = 0; i < infoWindows.length; i++) {
                                    infoWindows[i].close();
                                }
                                //alert(latlngParking);
                                infowindow.open(map, marker);
                            });

                            marker.setMap(map);

                            if (anteriorMarcador === undefined) {
                                anteriorMarcador = marker;
                                puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                            } else {
                                //Pintar la linea de una parada a otra
                                puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                            }

                    });
                }).always(function () {
                    ruta = new google.maps.Polyline({
                        path: puntos,
                        geodesic: true,
                        strokeColor: '#FF0000',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    ruta.setMap(map);
                    mostrando = true;
                    
                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < puntos.length; i++) {
                        bounds.extend(puntos[i]);
                    }
                    map.fitBounds(bounds);
                    
                }).fail(function () {
            alert("Error al obtener la linea");
        });
}

function obtenerTiemposParadas(id) {
    //console.log("Aqui si " + id);
    if (id + "".length === 1) {
        id = "0" + id;
    }
    if (id in lineasBilbobusArray) {
        //console.log("Aqui tambien "+ id);
        var xml = lineasBilbobusArray[id];
        var paradas = xml.getElementsByTagName("Paradas")[0].childNodes;
        for (var i = 0; i < paradas.length; i++) {
            var idP = paradas[i].getElementsByTagName("Id")[0].childNodes[0].nodeValue;
            var tiempo = paradas[i].getElementsByTagName("TiempoRestante")[0].childNodes[0].nodeValue;
            //console.log(idP + " " + tiempo);
            paradasConTiempo[idP] = tiempo;
        }
    }
}

function formatearParada(item, id) {
    var resultado = "<div><h3><b>Parada de Bilbobus:</b> " + item.nombreParada + "</h3><br><b>Abreviatura:</b> " + item.abreviatura + "<br>";
    //console.log(item.id);
    if (item.id in paradasConTiempo) {
        //console.log(paradasConTiempo[item.id]);
        resultado = resultado + "<h4><b> Tiempo Real Restante: </b>"+paradasConTiempo[item.id]+"</h4><br>";
    }
    resultado = resultado + "</div>";
    return resultado;
}

function obtenerLineaTranvia() {
    $('#lineaeleg').html("Tranvía");
    //alert("aqui");
    puntos = [];
    limpiarRutas();
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
    var anteriorMarcador;
    $.getJSON("api/paradastranvia/linea")
                .done(function (data) {
                    $.each(data, function (key, item) {

                        // Si la posición es correcta, se añade un marcador al mapa
                        var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                        var contentString = formatItem(item, 10, "api/paradastranvia/linea");

                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });

                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            title: contentString,
                            icon: iconos[10]
                        });
                        infoWindows.push(infowindow);
                        markers.push(marker);

                        //se añade un evento al marcador
                        marker.addListener('click', function (event) {
                            for (var i = 0; i < infoWindows.length; i++) {
                                infoWindows[i].close();
                            }
                            //alert(latlngParking);
                            infowindow.open(map, marker);
                        });

                        marker.setMap(map);

                        if (anteriorMarcador === undefined) {
                            anteriorMarcador = marker;
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        } else {
                            //Pintar la linea de una parada a otra
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        }

                    });
                }).always(function () {
                    ruta = new google.maps.Polyline({
                        path: puntos,
                        geodesic: true,
                        strokeColor: '#00ff00',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    ruta.setMap(map);
                    mostrando = true;

                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < puntos.length; i++) {
                        bounds.extend(puntos[i]);
                    }
                    map.fitBounds(bounds);

                }).fail(function () {
                    alert("Error al obtener la linea");
                });
}

function obtenerLineaMetro() {
    $('#lineaeleg').html("Metro");
    puntos = [];
    limpiarRutas();
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
    var anteriorMarcador;
    $.getJSON("api/paradasmetro/paradasmetro")
                .done(function (data) {
                    $.each(data, function (key, item) {

                        // Si la posición es correcta, se añade un marcador al mapa
                        var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                        var contentString = formatItem(item, 9, "api/paradasmetro/paradasmetro");

                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });

                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            title: contentString,
                            icon: iconos[9]
                        });
                        infoWindows.push(infowindow);
                        markers.push(marker);

                        //se añade un evento al marcador
                        marker.addListener('click', function (event) {
                            for (var i = 0; i < infoWindows.length; i++) {
                                infoWindows[i].close();
                            }
                            //alert(latlngParking);
                            infowindow.open(map, marker);
                        });

                        marker.setMap(map);

                        if (anteriorMarcador === undefined) {
                            anteriorMarcador = marker;
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        } else {
                            //Pintar la linea de una parada a otra
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        }

                    });
                }).always(function () {

                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < puntos.length; i++) {
                        bounds.extend(puntos[i]);
                    }
                    map.fitBounds(bounds);

                }).fail(function () {
                    alert("Error al obtener la linea de metro");
                });
}

function mostrarInfoMetro() {
    $("#modalmetro").modal('show');
}

function obtenerLineasEuskotren() {
    $.getJSON("api/paradaseuskotren/lineas")
               .done(function (data) {
                   $("#euskotren").empty();
                   $.each(data, function (key, item) {
                       $("#euskotren").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerRutaEuskotren(&quot;" + item.id + "&quot;,&quot;" + item.abreviatura + "&quot;,&quot;" + item.nombre + "&quot;);'>" + item.abreviatura + " - " + item.nombre + "</a></li>");
                   });
               }).fail(function () {
                   alert("Error al obtener las lineas de Euskotren");
               });
}

function obtenerRutaEuskotren(id, abreviatura, nombre) {
    $('#lineaeleg').html("Linea Euskotren " + abreviatura + " : " + nombre);
    puntos = [];
    limpiarRutas();
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
    var anteriorMarcador;
    $.getJSON("api/paradaseuskotren/linea/" + id)
                .done(function (data) {
                    $.each(data, function (key, item) {

                        // Si la posición es correcta, se añade un marcador al mapa
                        var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                        var contentString = formatItem(item, 8, "api/paradaseuskotren/paradaseuskotren");

                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });

                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            title: contentString,
                            icon: iconos[8]
                        });
                        infoWindows.push(infowindow);
                        paradas.push(item);
                        markers.push(marker);

                        //se añade un evento al marcador
                        marker.addListener('click', function (event) {
                            for (var i = 0; i < infoWindows.length; i++) {
                                infoWindows[i].close();
                            }
                            //alert(latlngParking);
                            infowindow.open(map, marker);
                        });

                        marker.setMap(map);

                        if (anteriorMarcador === undefined) {
                            anteriorMarcador = marker;
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        } else {
                            //Pintar la linea de una parada a otra
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        }

                    });
                }).always(function () {
                    ruta = new google.maps.Polyline({
                        path: puntos,
                        geodesic: true,
                        strokeColor: '#0000FF',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    ruta.setMap(map);
                    mostrando = true;

                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < puntos.length; i++) {
                        bounds.extend(puntos[i]);
                    }
                    map.fitBounds(bounds);

                }).fail(function () {
                    alert("Error al obtener la linea de euskotren");
                });

}

function obtenerLineasBizkaibus() {
    var contador = 0;
    $.getJSON("api/paradasbizkaibus/lineas")
               .done(function (data) {
                   $("#bizkaibus").empty();
                   $("#bizkaibus").append("<li class='dropdown-header'>Ordenado por código de linea</li>");
                   $.each(data, function (key, item) {
                      /* contador ++;
                       if (contador > 27) {
                           if (contador > 54) {
                               if (contador > 81) {
                                   $("#bizkaibus4").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerLineaBizkaibus(&quot;" + item.id + "&quot;);'>" + item.abreviatura + " : " + item.nombre + "</a></li>");
                               } else {
                                   $("#bizkaibus3").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerLineaBizkaibus(&quot;" + item.id + "&quot;);'>" + item.abreviatura + " : " + item.nombre + "</a></li>");
                               }
                               
                           } else {
                               $("#bizkaibus2").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerLineaBizkaibus(&quot;" + item.id + "&quot;);'>" + item.abreviatura + " : " + item.nombre + "</a></li>");
                           }
                       } else {
                           $("#bizkaibus1").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerLineaBizkaibus(&quot;" + item.id + "&quot;);'>" + item.abreviatura + " - " + item.nombre + "</a></li>");
                       }*/
                       $("#bizkaibus").append("<li><span class='glyphicon glyphicon-menu-right' aria-hidden='true'></span><a onclick='obtenerLineaBizkaibus(&quot;" + item.id + "&quot;,&quot;" + item.abreviatura + "&quot;,&quot;" + item.nombre + "&quot;);'>" + item.abreviatura + " : " + item.nombre + "</a></li>");
                   });
               }).fail(function () {
                   alert("Error al obtener las lineas de Euskotren");
               });
}

function obtenerLineaBizkaibus(id, abreviatura, nombre) {
    $('#lineaeleg').html("Linea Bizkaibus " + abreviatura + " : " + nombre);
    puntos = [];
    limpiarRutas();
    if (ruta !== undefined) {
        ruta.setMap(null);
    }
    var anteriorMarcador;
    $.getJSON("api/paradasbizkaibus/linea/" + id)
                .done(function (data) {
                    $.each(data, function (key, item) {

                        // Si la posición es correcta, se añade un marcador al mapa
                        var myLatlng = new google.maps.LatLng(item.latitud, item.longitud);

                        var contentString = formatItem(item, 7, "api/paradasbizkaibus/paradasbizkaibus");

                        var infowindow = new google.maps.InfoWindow({
                            content: contentString
                        });

                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            title: contentString,
                            icon: iconos[7]
                        });
                        infoWindows.push(infowindow);
                        paradas.push(item);
                        markers.push(marker);

                        //se añade un evento al marcador
                        marker.addListener('click', function (event) {
                            for (var i = 0; i < infoWindows.length; i++) {
                                infoWindows[i].close();
                            }
                            //alert(latlngParking);
                            infowindow.open(map, marker);
                        });

                        marker.setMap(map);

                        if (anteriorMarcador === undefined) {
                            anteriorMarcador = marker;
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        } else {
                            //Pintar la linea de una parada a otra
                            puntos.push(new google.maps.LatLng(item.latitud, item.longitud));
                        }

                    });
                }).always(function () {
                    ruta = new google.maps.Polyline({
                        path: puntos,
                        geodesic: true,
                        strokeColor: '#AEB404',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    ruta.setMap(map);
                    mostrando = true;

                    var bounds = new google.maps.LatLngBounds();
                    for (var i = 0; i < puntos.length; i++) {
                        bounds.extend(puntos[i]);
                    }
                    map.fitBounds(bounds);

                }).fail(function () {
                    alert("Error al obtener la linea de bizkaibus");
                });
}
