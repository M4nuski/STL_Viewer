# STL_Viewer
Viewer for 3D STL files, classic image viewer controls, OpenGL

## Supports
* Binary STL
* ASCII STL
* Color STL (per facet encoded in attribute word) 
* Some oddball hybrid binary with ASCII header files

## Controls and features
* Display a wireframe build volume
* Auto center model in view
* Left and Right arrows for Previous and Next model in folder
* Enter to toggle default color vs model colors
* Delete to move file to trash

## Settings
Build volume dimensions and default color can be set in the ".config" file.

## Performance and technology
* Base branch uses OpenTK OpenGL 2.0 immediate mode and compiled lists: context takes about a second to initialize.
* Parallel branches to test faster and more modern contexts
  * 4.x OpenGL with shaders and VBO
  * WPF native 3D rendering context etc
 
