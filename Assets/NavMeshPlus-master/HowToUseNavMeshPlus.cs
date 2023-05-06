//To use it in your project:

//Create Empty Object in scene root.
//Add "Navigation Surface" component to Empty Object and add NavMeshCollecSources2d component after.
//Click Rotate Surface to XY (to face surface toward standard 2d camera x-90; y0; z0)
//Add "Navigation Modifier" component to scene objects obstacles, override the area.
//In "Navigation Surface" hit Bake.
//How does it works:

//It uses NavMeshSurface as base implementation.
//Implements world bound calculation.
//Implements source collector of tiles, sprites and 2d colliders
//Creates walkable mesh box from world bounds.
//Convert tiles, sprites and 2d colliders to sources as NavMeshBuilder would do.