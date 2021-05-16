# STL_Viewer
Viewer for 3D STL files, classic image viewer controls, OpenGL

## Precision 3D printing, STL model corrections
* Allow to compensate the extrusion pattern by thinning or widening the model
* I.E. If the extruder overextrude by 0.10mm in the X axis, a -0.10mm offset can be applied on the faces
* This is not the same as scaling as it preserve the actual overall dimensions and positions
* Controls for all 3 axis
* Save to a new STL binary file, preserving colors, with data/time/compensation markers

## Supports
* Binary STL
* ASCII STL
* Color STL (per facet encoded in attribute word) 
* Some oddball hybrid binary with ASCII header files

## Controls and features
* Display a wireframe build volume
* Auto center model in view
* Left and Right arrows for Previous and Next model in folder
* Ctrl-R to rename file
* C to toggle default color vs model colors
* W to toggle wireframe mode
* Delete to move file to trash
* F12 to activate model compensation controls (Ctrl+S to save)

## Settings
Build volume dimensions and default color can be set in the ".config" file.

## Performance and technology
* Base branch uses OpenTK OpenGL 2.0 immediate mode and compiled lists: context takes about a second to initialize.
* Parallel branches to test faster and more modern contexts
  * 4.x OpenGL with shaders and VBO
  * WPF native 3D rendering context
 
