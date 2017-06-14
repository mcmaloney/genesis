# Genesis

## About

Genesis is a platform built using Unity for creating map data visualizations in VR environments. 

What's that, you say? There are already at least a half dozen things that render maps in Unity scenes? You're right, and while that's true, those existing tools are really good at the following things:

1. Rendering realistic terrain and 3d models of some real world objects for creating game levels in Unity scenes
2. Making sure those things are placed relative to eachother in the game world as they are in the real world
3. Giving options for extending look-and-feel within Unity once they render

Genesis, however, aims to fill the following gaps in the state-of-the-art:

1. Rendering _any_ spatial data into Unity scenes. ESRI Shapefiles, GeoJSON, CSVs, KMLs, tiles - you name it, Genesis will render it.
2. Making any and all metadata associated with those spatial data sources available to the developer and thus, the end user
3. Providing robust, easy-to-use SDKs to facilitate development of VR environments that expose spatial objects, their properties to the user

## Kinds Of Things We Want Genesis to Do:

The following are user stories at or near the top of the backlog:

1. User can search for a location of interest, be magically teleported there, and see default map layers 
2. Developer can create map layers which display points, lines, and polygons on the default tiles
3. Developer can choose a data source for a map layer (probably GeoJSON endpoint first)
4. User can toggle visibility of pre-defined map layers built by Developer
5. User can interact with map layers and see metadata associated with them
6. Developer can create data visualization layers using metadata from map objects (heatmaps? maybe heatclouds...)

## Friends and Neighbors

### [Mapbox](https://www.mapbox.com/studio/)
We gotta give a huge shout out to Mapbox. Genesis has hijacked their [Unity SDK](https://www.mapbox.com/unity/) and used it as the tileserver so we didn't have to figure out how to render terrain contours, 3D buildings, and satellite images on tiles. They already did a great job with that and a whole bunch of other things; no need to reinvent that wheel.

### The Masked Man Known as `runevision` Who Built [Triangulator](http://wiki.unity3d.com/index.php/Triangulator)
Building procedural polygons is pretty tedious, but you, sir, whoever you are, gave us a fine tool to build our primitive polygons with. There would be no Census Blocks, no MSAs, no State Boundaries in Genesis without you. (Well, maybe there would be, but not without a whole lot of WTF'ing triangles and normals and tangents and stuff.)
