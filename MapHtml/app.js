ymaps.ready(init);
var map, mark;
var coords = {
	center: [55.76, 37.64],
	zoom: 10
};

function init(){
	
	var geo = ymaps.geocode(document.location.hash.substring(1));
	geo.then(geoRes,geoErr);
}

function geoRes(res){
	initMap(res.geoObjects.get(0).geometry.getBounds());
}

function geoErr(err){
	initMap();
}

function initMap(bounds){
	map = new ymaps.Map ("map", {
		center: coords.center,
		zoom: coords.zoom,
		bounds: bounds
	});
	if (map.getZoom() > 16)
		map.setZoom(16);
	coords.center = map.getCenter();
	coords.zoom = map.getZoom();
	saveData();
	map.geoObjects.add(createMark(map.getCenter()));
	map.events.add("boundschange", onMapMove);
}

function onMapMove(event){
	map.geoObjects.remove(mark);
	coords.center = event.get("newCenter");
	coords.zoom = event.get("newZoom");
	map.geoObjects.add(createMark(coords.center));
	saveData();
}

function onMarkDrag(event){
	coords.center = event.get("target").geometry.getCoordinates();
	map.setCenter(coords.center);
}

function createMark(coordinates){
	mark = new ymaps.Placemark(coordinates,{},{draggable:true});
	mark.events.add("dragend",onMarkDrag);
	return mark;
}

function saveData(){
	document.title = coords.center[0]+','+coords.center[1]+','+coords.zoom;
}