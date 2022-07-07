class Drag extends ol.interaction.Pointer {
    constructor() {
        super({
            handleDownEvent: handleDownEvent,
            handleDragEvent: handleDragEvent,
            handleMoveEvent: handleMoveEvent,
            handleUpEvent: handleUpEvent,
        });

        /**
         * @type {import("../src/ol/coordinate.js").Coordinate}
         * @private
         */
        this.coordinate_ = null;

        /**
         * @type {string|undefined}
         * @private
         */
        this.cursor_ = 'pointer';

        /**
         * @type {ol.Feature}
         * @private
         */
        this.feature_ = null;

        /**
         * @type {string|undefined}
         * @private
         */
        this.previousCursor_ = undefined;
    }
}

/**
 * @param {import("../src/ol/MapBrowserEvent.js").default} evt Map browser event.
 * @return {boolean} `true` to start the drag sequence.
 */
function handleDownEvent(evt) {
    const eventMap = evt.map;

    const feature = eventMap.forEachFeatureAtPixel(evt.pixel, function (feature) {
        return feature;
    });

    if (feature) {
        this.coordinate_ = evt.coordinate;
        this.feature_ = feature;
    }

    return !!feature;
}

/**
 * @param {import("../src/ol/MapBrowserEvent.js").default} evt Map browser event.
 */
function handleDragEvent(evt) {
    const deltaX = evt.coordinate[0] - this.coordinate_[0];
    const deltaY = evt.coordinate[1] - this.coordinate_[1];

    const geometry = this.feature_.getGeometry();
    geometry.translate(deltaX, deltaY);

    this.coordinate_[0] = evt.coordinate[0];
    this.coordinate_[1] = evt.coordinate[1];
}

/**
 * @param {import("../src/ol/MapBrowserEvent.js").default} evt Event.
 */
function handleMoveEvent(evt) {
    if (this.cursor_) {
        const map = evt.map;
        const feature = map.forEachFeatureAtPixel(evt.pixel, function (feature) {
            return feature;
        });
        const element = evt.map.getTargetElement();
        if (feature) {
            if (element.style.cursor != this.cursor_) {
                this.previousCursor_ = element.style.cursor;
                element.style.cursor = this.cursor_;
            }
        } else if (this.previousCursor_ !== undefined) {
            element.style.cursor = this.previousCursor_;
            this.previousCursor_ = undefined;
        }
    }
}

/**
 * @return {boolean} `false` to stop the drag sequence.
 */
function handleUpEvent() {
    this.coordinate_ = null;
    this.feature_ = null;
    document.getElementById("Latitude").value = ol.proj.toLonLat(pointFeature.getGeometry().getCoordinates())[1].toFixed(7);
    document.getElementById("Longitude").value = ol.proj.toLonLat(pointFeature.getGeometry().getCoordinates())[0].toFixed(7);
    return false;
}


const centerLat = 39.9092;
const centerLon = 32.7539;
const center = ol.proj.transform([centerLon, centerLat], 'EPSG:4326', 'EPSG:3857');
const pointFeature = new ol.Feature(new ol.geom.Point(center));


const map = new ol.Map({
    interactions: ol.interaction.defaults().extend([new Drag()]),
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM()
        }),
        new ol.layer.Vector({
            source: new ol.source.Vector({
                features: [pointFeature]
            }),
            style: new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [0.5, 1.0],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'fraction',
                    name: 'pin',
                    opacity: 0.9,
                    scale: [0.3, 0.3],
                    src: 'https://upload.wikimedia.org/wikipedia/commons/thumb/f/fb/Map_pin_icon_green.svg/94px-Map_pin_icon_green.svg.png'
                })
            })
        })
    ],
    target: 'map',
    view: new ol.View({
        center: center,
        zoom: 11,
    }),
});