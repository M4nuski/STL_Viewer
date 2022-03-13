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
  * AlgoOptimization

Algorithms are now in nlogn and linear isntead of the naive n^2 implementation
| Number of Polygons   | Unique vertex finding time (Old) | Unique vertex finding time (New) | Edge finding time (Old) | Edge finding time (New)
|:---------------------|:----------:|:--------------------------:|:-----------------:|:-----------------:|
| 28   | 111ms | 113ms | 1ms   | 2ms   |
| 1K   | 16ms  | 41ms  | 25ms  | 4ms   |
| 2K   | 87ms  | 38ms  | 48ms  | 42ms  |
| 5K   | 113ms | 122ms | 281ms | 15ms  |
| 10K  | 322ms | 100ms | 1.3s  | 44ms  | 
| 58K  | 9.2s  | 205ms | 29.3s | 286ms |
| 100K | 26.7s | 313ms | 2.1m  | 474ms |
| 357K | 5.8m  | 1.3s  | 31.8m | 1.8s  |
| 600K | N/A   | 6.2s  | N/A   | 4.7s  |
| 1.2M | N/A   | 12.3s | N/A   | 5.7s  |
| 2.0M | N/A   | 32.3s | N/A   | 13.0s |
| 3.0M | N/A   | 86.5s | N/A   | 33.2s |
 
