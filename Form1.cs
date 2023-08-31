using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

using System.IO;
using Shell32;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.ComponentModel;
using System.Threading;

namespace STLViewer // OpenTK OpenGL 2.0 Immediate mode with pre compiled lists, single BGW for UV and for EF
{
    public partial class Form1 : Form
    {
        public Stopwatch perfCount = new Stopwatch();

        // User build volume
        private float buildVolumeWidthDiv2;
        private float buildVolumeHeight;
        private float buildVolumeDepthDiv2;

        //OpenGL
        public IWindowInfo WindowInfo;
        public GraphicsContext WindowContext;
        private float[] pmat; // perspective matrix
        private const float _fov = 45.0f;

        //Directory and file content
        private List<string> dirList = new List<string>();
        private string basePath;
        private int currentIndex = -1;

        // current file stl data
        private string currentFile;
        private STL_Loader loader = new STL_Loader();
        private BoundingBoxData bbData;

        private int colList = -1; // color list
        private int defList = -1; // default list
        private int holeList = -1;
        private int compList = -1; // compensated list
        private int outlineList = -1; // outline list
        private int outlineMode = 1; // 1 outline and mesh, 2 mesh only, 3 outline only
        private string status = "";
        private string modString = "";

        // Compensation data
        public struct indiceStruct
        {
            public int I1; // ref V1 of face
            public int I2; // ref V2 of face
            public int I3; // ref V3 of face
        }
        public class edgeStruct
        {
            public int I1; // ref V1 of edge
            public int I2; // ref V2 of edge

            public int T1; // index of triangle 1
            public int T2; // index of triangle 2
        }

        public List<FaceData> newData;
        public indiceStruct[] faceIndices;

        private static readonly float epsilon = 0.01f;
        public List<Vector3> uniqueVertices = new List<Vector3>();
        public List<int> uniqueIndices;
        public List<List<int>> uniqueBoundTriangles;

        public class vertexPackedData:IComparable<vertexPackedData> {
            public Vector3 pos;
            public float distance;
            public int originalVertexIndex;
            public bool isUnique = true;
            public int sortedUniqueIndex;
            public int extractedUniqueIndex;
            public int CompareTo(vertexPackedData other)
            {
                if (other == null) return -1;
                if (this.distance < other.distance) return -1;
                if (this.distance > other.distance) return  1;
                return 0;
            } 
        }
        public class trianglePackedData
        {
            public List<int> indiceList = new List<int>();
            public int triangleIndex;
            public List<int> ti1; // neighbourg triangles indices
            public List<int> ti2;
            public List<int> ti3;
        }

        private bool UniqueVerticesReady = false;
        private bool OutlineReady = false;
        private long loadStart = 0;
        private AutoResetEvent _BGW_UV_resetEvent = new AutoResetEvent(true);
        private AutoResetEvent _BGW_EF_resetEvent = new AutoResetEvent(true);

        public List<edgeStruct> edges = new List<edgeStruct>();

        // user inputs
        private float px = 0.0f;
        private float py = -75.0f;
        private float pz = 300.0f;

        private float rx = 11.0f;
        private float ry = -16.0f;

        private int mouse_x;
        private int mouse_y;
        private bool mouse_btn;
        private const float mouseSpeed = 0.15f;
        private const float mouseWheelSpeed = 10.0f;

        private Vector3 pivot;

        private bool originalColors = true;
        private Vector4 defaultColor;
        private Color backColor;
        private Vector4 edgeColor = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
        private Vector4 holeColor = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

        private bool wiremode = false;

        private readonly RenameDialog renameDialog_form = new RenameDialog();

        public Form1()
        {
            perfCount.Start();
            InitializeComponent();
            pmat = new float[16];
            try
            {
                defaultColor = new Vector4(
                    Properties.Settings.Default.defaultColor_R,
                    Properties.Settings.Default.defaultColor_G,
                    Properties.Settings.Default.defaultColor_B,
                    Properties.Settings.Default.defaultColor_A
                );
            }
            catch (Exception ex)
            {
                defaultColor = new Vector4(0.75f, 0.75f, 0.75f, 1.0f);
                Console.WriteLine("error reading default color assembly properties: " + ex.Message);
            }

            try
            {
                backColor = Color.FromArgb(
                    (int)(Properties.Settings.Default.backColor_R * 255),
                    (int)(Properties.Settings.Default.backColor_G * 255),
                    (int)(Properties.Settings.Default.backColor_B * 255)
                );
            }
            catch (Exception ex)
            {
                backColor = Color.FromArgb(35, 105, 219);
                Console.WriteLine("error reading background color assembly properties: " + ex.Message);
            }

            label1.BackColor = backColor;
            trackBarX.BackColor = backColor;
            trackBarY.BackColor = backColor;
            trackBarZ.BackColor = backColor;
            centerCompTrackBar.BackColor = backColor;
            centerLimitTrackBar.BackColor = backColor;

            setPerspective(_fov, (float)ClientSize.Width / ClientSize.Height, 0.1f, 4096.0f);
            pivot = new Vector3(0.0f, 0.0f, 0.0f);
            var args = Environment.GetCommandLineArgs();

            currentFile = args.Length > 1 ? args[1] : "";
            Console.WriteLine("form constructor end " + perfCount.ElapsedMilliseconds);
            try
            {
                buildVolumeWidthDiv2 = (int)Properties.Settings.Default["width_mm"] / 2.0f;
                buildVolumeHeight = (int)Properties.Settings.Default["height_mm"] / 1.0f;
                buildVolumeDepthDiv2 = (int)Properties.Settings.Default["depth_mm"] / 2.0f;
            }
            catch (Exception ex)
            {
                buildVolumeWidthDiv2 = 120.0f;
                buildVolumeHeight = 150.0f;
                buildVolumeDepthDiv2 = 75.0f;
                Console.WriteLine("error reading build volume assembly properties: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // init stuff
            WindowInfo = Utilities.CreateWindowsWindowInfo(renderPanel.Handle);
            var WindowMode = new GraphicsMode(32, 24, 0, 0, 0, 2);
            WindowContext = new GraphicsContext(WindowMode, WindowInfo, 2, 0, GraphicsContextFlags.Default);

            WindowContext.MakeCurrent(WindowInfo);
            WindowContext.LoadAll();

            WindowContext.SwapInterval = 1;
            GL.Viewport(0, 0, renderPanel.Width, renderPanel.Height);

            GL.Enable(EnableCap.DepthTest);

            GL.Disable(EnableCap.CullFace);
            GL.ClearColor(backColor);

            GL.Light(LightName.Light0, LightParameter.Ambient, new Color4(0.15f, 0.15f, 0.15f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Diffuse, new Color4(0.85f, 0.85f, 0.85f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Specular, new Color4(0.0f, 0.0f, 0.0f, 0.0f));
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(0.5f, 1.0f, 0.5f, 0.0f));

            GL.Enable(EnableCap.Light0);

            //GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new Color4(0.2f, 0.2f, 0.2f, 1.0f));
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, new Color4(0.0f, 0.0f, 0.0f, 1.0f));
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, 127.0f);


            MouseWheel += Form1_MouseWheel;

            setCurrentDirectory(currentFile);

            Console.WriteLine("form load end " + perfCount.ElapsedMilliseconds);
        }

        private void setCurrentDirectory(string path)
        {
            currentIndex = -1;

            if (path != "")
            {
                basePath = Path.GetDirectoryName(Path.GetFullPath(path));
                currentFile = Path.GetFileName(path);
                if (basePath != null)
                {
                    basePath += "\\";
                    var dc = new DirectoryInfo(basePath);
                    var df = dc.GetFiles("*.stl");
                    for (var i = 0; i < df.Length; ++i)
                    {
                        dirList.Add(df[i].Name);
                        if (df[i].Name == currentFile) currentIndex = i;
                    }

                    Console.WriteLine("Base path " + basePath);
                    Console.WriteLine("Current index " + currentIndex);
                }


                if (File.Exists(basePath + currentFile)) loadModel();
            }
        }



        private void loadModel()
        {
            Console.WriteLine("loading " + currentFile);
            loadStart = perfCount.ElapsedMilliseconds;
            Text = Path.GetFullPath(basePath + currentFile);
            CancelBGW();
            outlineMode = 1;

            loader.loadFile(basePath + currentFile, STL_Loader.NormalsRecalcMode.always);
            bbData = loader.getBoundingBox();

            // center model on platform
            var modelPos = new Vector3((bbData.maxX + bbData.minX) / -2.0f,
                -bbData.minY,
                (bbData.maxZ + bbData.minZ) / -2.0f);

            // center model
            for (var ti = 0; ti < loader.Triangles.Count; ++ti)
            {
                loader.Triangles[ti].V1 += modelPos;
                loader.Triangles[ti].V2 += modelPos;
                loader.Triangles[ti].V3 += modelPos;
            }


            // update bounding box data
            bbData.minX += modelPos.X;
            bbData.maxX += modelPos.X;

            bbData.minY += modelPos.Y;
            bbData.maxY += modelPos.Y;

            bbData.minZ += modelPos.Z;
            bbData.maxZ += modelPos.Z;

            // center view on model
            setPerspective(_fov, (float)ClientSize.Width / ClientSize.Height, 0.1f, 4096.0f);
            pivot = new Vector3(0.0f, 0.0f, 0.0f);
            pz = Math.Max(bbData.maxX - bbData.minX, bbData.maxY - bbData.minY);
            pz = Math.Max(bbData.maxZ - bbData.minZ, pz);
            pz = Math.Max(2.5f * pz / (float)Math.Tan(_fov), 75.0f);

            py = (bbData.maxY - bbData.minY) / -2.0f;
            px = 0;

            // prepare model data
            if (loader?.Triangles.Count > 0)
            {
                if (colList != -1) GL.DeleteLists(colList, 1);
                if (defList != -1) GL.DeleteLists(defList, 1);
                if (compList != -1) GL.DeleteLists(compList, 1);
                if (outlineList != -1) GL.DeleteLists(outlineList, 1);
                compList = -1;
                defList = -1;
                compList = -1;
                outlineList = -1;

                colList = GL.GenLists(1);
                GL.NewList(colList, ListMode.Compile);
                drawModel(!loader.Colored, (int)loader.Triangles.Count, loader.Triangles);
                GL.EndList();
                Console.WriteLine("Gen default model list data yields " + GL.GetError());

                if (!loader.Colored) defList = colList;
                else
                {
                    defList = GL.GenLists(1);
                    GL.NewList(defList, ListMode.Compile);
                    drawModel(true, (int)loader.Triangles.Count, loader.Triangles);
                    GL.EndList();
                    Console.WriteLine("Gen colored model list data yields " + GL.GetError());
                }
                status = currentFile + " " + loader.Triangles.Count + " triangles (" + loader.Type + ") ";
                label1.Text = status;

            }

            originalColors = loader.Colored;
            wiremode = false;
            Console.WriteLine("model loaded in " + (perfCount.ElapsedMilliseconds - loadStart));
            compCtrlPanel.Hide();
            holeCompPanel.Hide();

            if (loader?.Triangles.Count > 0)
            {
                Console.WriteLine("num vertex " + loader.Triangles.Count * 3);
                // prep data for comp
                faceIndices = new indiceStruct[loader.Triangles.Count];
                uniqueVertices.Clear();
                UniqueVerticesReady = false;
                // start checking for unique vertices
                _BGW_UV_resetEvent.Reset();
                backgroundWorker_UniqueVertex.RunWorkerAsync();

                ReDraw();
            }  
        }

        private void drawModel(bool overrideColors, int nbTriangles, List<FaceData> data)
        {
            GL.Begin(PrimitiveType.Triangles);
            for (var i = 0; i < nbTriangles; i++)
            {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, (overrideColors) ? defaultColor : data[i].Color);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, (overrideColors) ? defaultColor : data[i].Color);
                GL.Normal3(data[i].Normal);
                GL.Vertex3(data[i].V1);

                GL.Normal3(data[i].Normal);
                GL.Vertex3(data[i].V2);

                GL.Normal3(data[i].Normal);
                GL.Vertex3(data[i].V3);
            }
            GL.End();
        }

        private void resetSizes()
        {
            renderPanel.Size = ClientSize;
            if (WindowInfo != null)
            {
                setPerspective(_fov, (float)ClientSize.Width / ClientSize.Height, 0.1f, 4096.0f);
                ReDraw();
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            pz += e.Delta > 0 ? mouseWheelSpeed : -mouseWheelSpeed;
            ReDraw();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("form paint");
            resetSizes();
        }

        private void ReDraw()
        {
            if (WindowInfo == null) return;

            // update pivot
            var mrx = Matrix3.CreateRotationX(-rx * 0.01745f);
            var mry = Matrix3.CreateRotationY(-ry * 0.01745f);

            var offset = new Vector3(px, py, 0.0f);
            offset = Vector3.Transform(mrx, offset);
            pivot += Vector3.Transform(mry, offset); // ang in radians

            // update light
            offset = new Vector3(0.5f, 0.5f, 1.0f);
            offset.Normalize();
            offset = Vector3.Transform(mrx, offset);
            offset = Vector3.Transform(mry, offset); // ang in radians

            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(offset, 0.0f));

            px = 0.0f;
            py = 0.0f;

            // redraw
            WindowContext.MakeCurrent(WindowInfo);
            GL.Viewport(0, 0, renderPanel.Width, renderPanel.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(pmat);
            GL.Translate(0.0f, 0.0f, -pz);
            GL.Rotate(rx, 1.0f, 0.0f, 0.0f); // ang in degrees
            GL.Rotate(ry, 0.0f, 1.0f, 0.0f);
            GL.Translate(pivot);


            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // build platform box
            GL.Disable(EnableCap.Lighting);
            drawBuildVolume();
            GL.Enable(EnableCap.Lighting);

            if (loader?.Triangles.Count > 0)
            {
                if ((compList > 0) && (compCtrlPanel.Visible || holeCompPanel.Visible))
                {
                    if (wiremode)
                    {
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.CallList(compList);
                    }
                    else
                    {
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        GL.CallList(compList);
                        GL.Disable(EnableCap.DepthTest);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        GL.CallList((originalColors) ? colList : defList);
                        if (holeCompPanel.Visible)
                        {
                            GL.LineWidth(3.0f);
                            GL.Disable(EnableCap.Lighting);
                            GL.CallList(holeList);
                            GL.Enable(EnableCap.Lighting);
                            GL.LineWidth(1.0f);
                        }
                        GL.Enable(EnableCap.DepthTest);

                        if (OutlineReady)
                        {
                            GL.LineWidth(3.0f);
                            GL.CallList(outlineList);
                            GL.LineWidth(1.0f);
                        }
                    }
                }
                else
                {
                    // 1 outline and mesh, 2 mesh only, 3 outline only
                    if (outlineMode != 2)
                    {
                        if (OutlineReady)
                        {
                            GL.LineWidth(3.0f);
                            GL.CallList(outlineList);
                            GL.LineWidth(1.0f);
                        }
                    }
                    if (outlineMode != 3)
                    {                      
                        GL.PolygonMode(MaterialFace.FrontAndBack, wiremode ? PolygonMode.Line : PolygonMode.Fill);
                        GL.CallList((originalColors) ? colList : defList);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    }

                }
            }   

            WindowContext.SwapBuffers();
        }

        private void setPerspective(float FOV, float AR, float Near, float Far)
        {
            var f = (float)(1.0 / Math.Tan(FOV / 2.0));
            var nf = (float)(1.0 / (Near - Far));

            pmat[0] = f / AR;
            pmat[1] = 0.0f;
            pmat[2] = 0.0f;
            pmat[3] = 0.0f;
            pmat[4] = 0.0f;
            pmat[5] = f;
            pmat[6] = 0.0f;
            pmat[7] = 0.0f;
            pmat[8] = 0.0f;
            pmat[9] = 0.0f;
            pmat[10] = (Far + Near) * nf;
            pmat[11] = -1.0f;
            pmat[12] = 0.0f;
            pmat[13] = 0.0f;
            pmat[14] = 2.0f * Far * Near * nf;
            pmat[15] = 0.0f;
        }

        // using Shell32;
        public static Shell shell = new Shell();
        public static Folder RecyclingBin = shell.NameSpace(ShellSpecialFolderConstants.ssfBITBUCKET);

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1) // Toggle Help label
            {
                label2.Visible = !label2.Visible;
            }

            if (e.KeyCode == Keys.F12) // Toggle Surface Compensation
            {
                holeCompPanel.Visible = false;
                if (!compCtrlPanel.Visible && UniqueVerticesReady)
                {
                    compCtrlPanel.Visible = true;
                }
                else compCtrlPanel.Visible = false;
            }
            if (e.KeyCode == Keys.F11) // Toggle Hole Compensation
            {
                compCtrlPanel.Visible = false;
                if (!holeCompPanel.Visible && UniqueVerticesReady)
                {
                    holeCompPanel.Visible = true;
                }
                else holeCompPanel.Visible = false;
            }
            if ((e.KeyCode == Keys.S) && e.Control) // Save Compensated model
            {
                if (compList != -1)
                {
                    var stlw = new STL_Writer(loader.Colored);
                    for (int i = 0; i < loader.Triangles.Count; i++)
                    {
                        stlw.addFace(newData[i]);
                    }
                    try
                    {
                        var fn = Path.GetFileNameWithoutExtension(currentFile) + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + " " + modString + ".stl";
                        label1.Text = "Writing to " + fn;
                        stlw.writeToFile(fn, true);
                    }
                    catch
                    {
                        Console.WriteLine("File Already Exists");
                        label1.Text = "Failed to write to file";
                    }
                }
            }

            if (e.KeyCode == Keys.C) // Toggle model colors
            {
                originalColors = !originalColors;
            }
            if (e.KeyCode == Keys.O) // Toggle model colors
            {
                ++outlineMode;
                if (outlineMode > 3) outlineMode = 1;
            }
            if (e.KeyCode == Keys.W) // Toggle wireframe
            {
                wiremode = !wiremode;
            }
            if (e.KeyCode == Keys.Right) // Next model in folder
            {
                if ((compCtrlPanel.Visible) || (holeCompPanel.Visible)) return;

                currentIndex++;
                if (currentIndex >= dirList.Count) currentIndex = dirList.Count - 1;
                else if (dirList.Count > 0)
                {
                    currentFile = dirList[currentIndex];
                    loadModel();
                }
            }

            if (e.KeyCode == Keys.Left) // Previous model in folder
            {
                if ((compCtrlPanel.Visible) || (holeCompPanel.Visible)) return;

                currentIndex--;
                if (currentIndex < 0) currentIndex = 0;
                else if (dirList.Count > 0)
                {
                    currentFile = dirList[currentIndex];
                    loadModel();
                }
            }

            if (e.KeyCode == Keys.Delete)  // Delete model file (after confirmation dialog)
            {
                compCtrlPanel.Hide();
                holeCompPanel.Hide();
                if (MessageBox.Show(this, "Delete current file ?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    RecyclingBin.MoveHere(basePath + currentFile);

                    dirList.RemoveAt(currentIndex);

                    if (dirList.Count == 0)
                    {
                        loader = null;
                        currentIndex = -1;
                    }
                    else if (dirList.Count == 1)
                    {
                        currentIndex = 0;
                        currentFile = dirList[currentIndex];
                        loadModel();
                    }
                    else
                    {
                        if (currentIndex >= dirList.Count) currentIndex = dirList.Count - 1;
                        currentFile = dirList[currentIndex];
                        loadModel();
                    }
                }
            }
            if ((e.KeyCode == Keys.R) && e.Control && (currentIndex != -1)) // Rename file
            {
                var oldName = currentFile;

                var res = renameDialog_form.ShowDialog(oldName, basePath);
                if (res == DialogResult.OK)
                {
                    // update current list and file
                    currentFile = renameDialog_form.outputString;
                    dirList[currentIndex] = renameDialog_form.outputString;

                    // update UI
                    label1.Text = $"Renamed {oldName} to {currentFile}";
                }
                else Console.WriteLine($"Rename cancelled");
             }
            ReDraw();
        }

        private void drawBuildVolume()
        {
            // back
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, 0.0f, -buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, 0.0f, buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, 0.0f, buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, 0.0f, -buildVolumeDepthDiv2);
            GL.End();

            // front
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, buildVolumeHeight, -buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, buildVolumeHeight, buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, buildVolumeHeight, buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, buildVolumeHeight, -buildVolumeDepthDiv2);
            GL.End();

            // sides
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, 0.0f, -buildVolumeDepthDiv2);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, buildVolumeHeight, -buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, 0.0f, buildVolumeDepthDiv2);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(-buildVolumeWidthDiv2, buildVolumeHeight, buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, 0.0f, -buildVolumeDepthDiv2);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, buildVolumeHeight, -buildVolumeDepthDiv2);

            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, 0.0f, buildVolumeDepthDiv2);
            GL.Color4(0.8f, 0.8f, 0.8f, 1.0f);
            GL.Vertex3(buildVolumeWidthDiv2, buildVolumeHeight, buildVolumeDepthDiv2);
            GL.End();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_btn = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            mouse_btn = false;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_btn = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_btn)
            {
                var d_x = e.X - mouse_x;
                var d_y = e.Y - mouse_y;

                if (e.Button == MouseButtons.Left)
                {
                    rx += d_y * mouseSpeed;
                    ry += d_x * mouseSpeed;
                }
                if (e.Button == MouseButtons.Middle)
                {
                    px += d_x * mouseSpeed;
                    py -= d_y * mouseSpeed;
                }

                mouse_x = e.X;
                mouse_y = e.Y;
                ReDraw();
            }
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            resetSizes();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            var fna = (string[])e.Data.GetData("FileNameW");
            var newName = "";
            if ((fna != null) && (fna.Length > 0)) newName = fna[0];
            if (Path.GetExtension(newName) == ".stl") e.Effect = DragDropEffects.Copy;
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            var fna = (string[])e.Data.GetData("FileNameW");
            var newName = "";
            if ((fna != null) && (fna.Length > 0)) newName = fna[0];
            if (Path.GetExtension(newName) == ".stl")
            {
                setCurrentDirectory(newName);
                ReDraw();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelBGW();
            WindowInfo = null;
            GL.DeleteLists(colList, 1);
            GL.DeleteLists(defList, 1);
            GL.DeleteLists(compList, 1);
            GL.DeleteLists(outlineList, 1);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (!compCtrlPanel.Visible) return;

            // recomp with new value and reset all data and list
            var offsetX = trackBarX.Value / 40.0f / 2.0f; // half of overall error (applied on 2 sides)
            var offsetY = trackBarY.Value / 40.0f / 2.0f;
            var offsetZ = trackBarZ.Value / 40.0f / 2.0f;
            label1.Text = "Total compensation (mm) X:" + (2.0f* offsetX).ToString("F3") + " Y:" + (2.0f * offsetY).ToString("F3") + " Z:" + (2.0f * offsetZ).ToString("F3");
            modString = " x" + (100 * trackBarX.Value / 40).ToString("D3") + "y" + (100 * trackBarY.Value / 40).ToString("D3") + "z" + (100 * trackBarZ.Value / 40).ToString("D3");
            // Prep opt and compare data
            var uniqueVertexOffsets = new Vector3[uniqueVertices.Count];
            var uniqueVertexOffsetsNeg = new Vector3[uniqueVertices.Count];
            var uniqueVertexOffsetsPos = new Vector3[uniqueVertices.Count];

            // compensate for offset, find longest normal in offset direction
            for (var i = 0; i < loader.Triangles.Count; ++i)
            {
                // X
                if (loader.Triangles[i].Normal.X > 0.0f)
                {
                    if (loader.Triangles[i].Normal.X > uniqueVertexOffsetsPos[faceIndices[i].I1].X) uniqueVertexOffsetsPos[faceIndices[i].I1].X = loader.Triangles[i].Normal.X;
                    if (loader.Triangles[i].Normal.X > uniqueVertexOffsetsPos[faceIndices[i].I2].X) uniqueVertexOffsetsPos[faceIndices[i].I2].X = loader.Triangles[i].Normal.X;
                    if (loader.Triangles[i].Normal.X > uniqueVertexOffsetsPos[faceIndices[i].I3].X) uniqueVertexOffsetsPos[faceIndices[i].I3].X = loader.Triangles[i].Normal.X;
                }
                else
                {
                    if (loader.Triangles[i].Normal.X < uniqueVertexOffsetsNeg[faceIndices[i].I1].X) uniqueVertexOffsetsNeg[faceIndices[i].I1].X = loader.Triangles[i].Normal.X;
                    if (loader.Triangles[i].Normal.X < uniqueVertexOffsetsNeg[faceIndices[i].I2].X) uniqueVertexOffsetsNeg[faceIndices[i].I2].X = loader.Triangles[i].Normal.X;
                    if (loader.Triangles[i].Normal.X < uniqueVertexOffsetsNeg[faceIndices[i].I3].X) uniqueVertexOffsetsNeg[faceIndices[i].I3].X = loader.Triangles[i].Normal.X;
                }

                // Y
                if (loader.Triangles[i].Normal.Y > 0.0f)
                {
                    if (loader.Triangles[i].Normal.Y > uniqueVertexOffsetsPos[faceIndices[i].I1].Y) uniqueVertexOffsetsPos[faceIndices[i].I1].Y = loader.Triangles[i].Normal.Y;
                    if (loader.Triangles[i].Normal.Y > uniqueVertexOffsetsPos[faceIndices[i].I2].Y) uniqueVertexOffsetsPos[faceIndices[i].I2].Y = loader.Triangles[i].Normal.Y;
                    if (loader.Triangles[i].Normal.Y > uniqueVertexOffsetsPos[faceIndices[i].I3].Y) uniqueVertexOffsetsPos[faceIndices[i].I3].Y = loader.Triangles[i].Normal.Y;
                }
                else
                {
                    if (loader.Triangles[i].Normal.Y < uniqueVertexOffsetsNeg[faceIndices[i].I1].Y) uniqueVertexOffsetsNeg[faceIndices[i].I1].Y = loader.Triangles[i].Normal.Y;
                    if (loader.Triangles[i].Normal.Y < uniqueVertexOffsetsNeg[faceIndices[i].I2].Y) uniqueVertexOffsetsNeg[faceIndices[i].I2].Y = loader.Triangles[i].Normal.Y;
                    if (loader.Triangles[i].Normal.Y < uniqueVertexOffsetsNeg[faceIndices[i].I3].Y) uniqueVertexOffsetsNeg[faceIndices[i].I3].Y = loader.Triangles[i].Normal.Y;
                }

                // Z
                if (loader.Triangles[i].Normal.Z > 0.0f)
                {
                    if (loader.Triangles[i].Normal.Z > uniqueVertexOffsetsPos[faceIndices[i].I1].Z) uniqueVertexOffsetsPos[faceIndices[i].I1].Z = loader.Triangles[i].Normal.Z;
                    if (loader.Triangles[i].Normal.Z > uniqueVertexOffsetsPos[faceIndices[i].I2].Z) uniqueVertexOffsetsPos[faceIndices[i].I2].Z = loader.Triangles[i].Normal.Z;
                    if (loader.Triangles[i].Normal.Z > uniqueVertexOffsetsPos[faceIndices[i].I3].Z) uniqueVertexOffsetsPos[faceIndices[i].I3].Z = loader.Triangles[i].Normal.Z;
                }
                else
                {
                    if (loader.Triangles[i].Normal.Z < uniqueVertexOffsetsNeg[faceIndices[i].I1].Z) uniqueVertexOffsetsNeg[faceIndices[i].I1].Z = loader.Triangles[i].Normal.Z;
                    if (loader.Triangles[i].Normal.Z < uniqueVertexOffsetsNeg[faceIndices[i].I2].Z) uniqueVertexOffsetsNeg[faceIndices[i].I2].Z = loader.Triangles[i].Normal.Z;
                    if (loader.Triangles[i].Normal.Z < uniqueVertexOffsetsNeg[faceIndices[i].I3].Z) uniqueVertexOffsetsNeg[faceIndices[i].I3].Z = loader.Triangles[i].Normal.Z;
                }
            }

            // select final offset              
            for (var i = 0; i < uniqueVertices.Count; ++i)
            {
                uniqueVertexOffsets[i].X = offsetX * (uniqueVertexOffsetsPos[i].X + uniqueVertexOffsetsNeg[i].X);
                uniqueVertexOffsets[i].Y = offsetY * (uniqueVertexOffsetsPos[i].Y + uniqueVertexOffsetsNeg[i].Y);
                uniqueVertexOffsets[i].Z = offsetZ * (uniqueVertexOffsetsPos[i].Z + uniqueVertexOffsetsNeg[i].Z);
            }

            // apply offset to data
            newData = new List<FaceData>(loader.Triangles.Count);
            for (var i = 0; i < loader.Triangles.Count; ++i)
            {
                newData.Add(new FaceData
                {
                    V1 = loader.Triangles[i].V1 + uniqueVertexOffsets[faceIndices[i].I1],
                    V2 = loader.Triangles[i].V2 + uniqueVertexOffsets[faceIndices[i].I2],
                    V3 = loader.Triangles[i].V3 + uniqueVertexOffsets[faceIndices[i].I3],
                    Normal = loader.Triangles[i].Normal,
                    Color = loader.Triangles[i].Color
                });
            }

            // create render list
            GL.DeleteLists(compList, 1);

            compList = GL.GenLists(1);
            GL.NewList(compList, ListMode.Compile);
            drawModel(!false, loader.Triangles.Count, newData);
            GL.EndList();
            //Console.WriteLine("Gen model compList data yields " + GL.GetError());

            ReDraw();
        }


        private void genOutlineList()
        {
            GL.DeleteLists(outlineList, 1);

            outlineList = GL.GenLists(1);
            GL.NewList(outlineList, ListMode.Compile);


            GL.Begin(PrimitiveType.Lines);
            for (var i = 0; i < edges.Count; i++)
            {
                if (Math.Abs(Vector3.Dot( loader.Triangles[edges[i].T2].Normal, loader.Triangles[edges[i].T1].Normal)) < 0.8f)
                {
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, edgeColor);
                    GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, edgeColor);

                    var offset = (loader.Triangles[edges[i].T2].Normal + loader.Triangles[edges[i].T1].Normal);
                    offset = offset * 0.005f;

                    GL.Vertex3(uniqueVertices[edges[i].I1] + offset);
                    GL.Vertex3(uniqueVertices[edges[i].I2] + offset);

                }
            }

            GL.End();
            GL.EndList();

            Console.WriteLine("Gen outlines list data yields " + GL.GetError());
        }

        private void backgroundWorker_Uniques_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label1.Text = status + " Analyze Canceled";
                UniqueVerticesReady = false;
            }
            else if (e.Error != null)
            {
                label1.Text = status + " Vertex analyze Error: " + e.Error.Message;
                UniqueVerticesReady = false;
            }
            else
            {
                label1.Text = status + $"Found {uniqueVertices.Count} unique vertex out of {loader.Triangles.Count * 3}";
                Console.WriteLine("num unique vertex " + uniqueVertices.Count);
                Console.WriteLine("model analysed in " + (perfCount.ElapsedMilliseconds - loadStart));
                UniqueVerticesReady = true;
                ReDraw();

                // start edge finding
                edges.Clear();
                OutlineReady = false;
                _BGW_EF_resetEvent.Reset();
                backgroundWorker_Outline.RunWorkerAsync();
            }
        }

        private void backgroundWorker_Uniques_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = status + $"Analysing model {e.ProgressPercentage}%";
        }

        private static bool v3eq(Vector3 a, Vector3 b)
        {
            return (Math.Abs(a.X - b.X) < epsilon) && (Math.Abs(a.Y - b.Y) < epsilon) && (Math.Abs(a.Z - b.Z) < epsilon);
        }
        private void backgroundWorker_Uniques_DoWork(object sender, DoWorkEventArgs e)
        {
            // setup
            var slice = 16384;
            BackgroundWorker thisWorker = sender as BackgroundWorker;
            loadStart = perfCount.ElapsedMilliseconds;

            // prepare axis
            var bbdata = loader.getBoundingBox();
            Vector3 axis = new Vector3(bbdata.maxX - bbdata.minX, bbdata.maxY - bbdata.minY, bbdata.maxZ - bbdata.minZ);
            if (axis.Length < 0.01) axis = new Vector3(0.57735f);
            axis = Vector3.Normalize(axis);

            // 
            var stepTime0 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: bb and axis done in " + (stepTime0 - loadStart));
            uniqueVertices.Clear();
            uniqueIndices = new List<int>(loader.Triangles.Count * 3);
            uniqueBoundTriangles = new List<List<int>>();

            // unpack data
            var data = new List<vertexPackedData>();
            var index = 0;
            for (var i = 0; i < loader.Triangles.Count; ++i)
            {
                // V1
                var vpd = new vertexPackedData
                {
                    pos = loader.Triangles[i].V1,
                    distance = Vector3.Dot(axis, loader.Triangles[i].V1),
                    originalVertexIndex = index++
                };
                data.Add(vpd);

                // V2
                vpd = new vertexPackedData
                {
                    pos = loader.Triangles[i].V2,
                    distance = Vector3.Dot(axis, loader.Triangles[i].V2),
                    originalVertexIndex = index++
                };
                data.Add(vpd);

                // V3
                vpd = new vertexPackedData
                {
                    pos = loader.Triangles[i].V3,
                    distance = Vector3.Dot(axis, loader.Triangles[i].V3),
                    originalVertexIndex = index++
                };
                data.Add(vpd);


                // Report Progress
                if ((loader.Triangles.Count > slice) && ((i % slice) == 0))
                {
                    thisWorker.ReportProgress(0 + (20 * i) / loader.Triangles.Count);

                    // Check for cancellation
                    if (thisWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        _BGW_UV_resetEvent.Set();
                        return;
                    }
                }
            }

            var stepTime1 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: data unpacked in " + (stepTime1 - stepTime0));

            // sort along axis (using precalculated .distance)
            data.Sort();

            stepTime0 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: data sorted along axis in " + (stepTime0 - stepTime1));

            // Report Progress
            thisWorker.ReportProgress(40);
            // Check for cancellation
            if (thisWorker.CancellationPending == true)
            {
                e.Cancel = true;
                _BGW_UV_resetEvent.Set();
                return;
            }

            // walk the array and flag vertex that are not unique anymore
            for (int i = 0; i < data.Count; ++i) if (data[i].isUnique)
                {
                    data[i].sortedUniqueIndex = i;
                    var j = i + 1;
                    while ((j < data.Count) && (Math.Abs(data[i].distance - data[j].distance) < (epsilon * 2f)))
                    {
                        if (v3eq(data[i].pos, data[j].pos))
                        {
                            data[j].isUnique = false;
                            data[j].sortedUniqueIndex = i;
                        }
                        ++j;
                    }

                    // Report Progress
                    if ((data.Count > slice) && ((i % slice) == 0))
                    {
                        thisWorker.ReportProgress(40 + (20 * i) / data.Count);

                        // Check for cancellation
                        if (thisWorker.CancellationPending == true)
                        {
                            e.Cancel = true;
                            _BGW_UV_resetEvent.Set();
                            return;
                        }
                    }
                }

            stepTime1 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: walked for uniques in " + (stepTime1 - stepTime0));

            // assign indices
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i].isUnique)
                {
                    uniqueVertices.Add(new Vector3(data[i].pos));
                    data[i].extractedUniqueIndex = uniqueVertices.Count - 1;

                    uniqueBoundTriangles.Add(new List<int>());
                    uniqueBoundTriangles[uniqueBoundTriangles.Count - 1].Add(data[i].originalVertexIndex / 3);
                }
                else
                {
                    uniqueBoundTriangles[data[data[i].sortedUniqueIndex].extractedUniqueIndex].Add(data[i].originalVertexIndex / 3);
                }

                uniqueIndices.Add(-1);

                // Report Progress
                if ((data.Count > slice) && ((i % slice) == 0))
                {
                    thisWorker.ReportProgress(60 + (20 * i) / data.Count);

                    // Check for cancellation
                    if (thisWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        _BGW_UV_resetEvent.Set();
                        return;
                    }
                }
            }

            stepTime0 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: indices assigned in " + (stepTime0 - stepTime1));

            // extract indices
            for (int i = 0; i < data.Count; ++i)
            { 
                uniqueIndices[data[i].originalVertexIndex] = data[data[i].sortedUniqueIndex].extractedUniqueIndex;
                var tri = data[i].originalVertexIndex / 3;
                var ver = data[i].originalVertexIndex % 3;
                if (ver == 0) faceIndices[tri].I1 = data[data[i].sortedUniqueIndex].extractedUniqueIndex;
                if (ver == 1) faceIndices[tri].I2 = data[data[i].sortedUniqueIndex].extractedUniqueIndex;
                if (ver == 2) faceIndices[tri].I3 = data[data[i].sortedUniqueIndex].extractedUniqueIndex;

                // Report Progress
                if ((data.Count > slice) && ((i % slice) == 0))
                {
                    thisWorker.ReportProgress(80 + (20 * i) / data.Count);

                    // Check for cancellation
                    if (thisWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        _BGW_UV_resetEvent.Set();
                        return;
                    }
                }
            }

            stepTime1 = perfCount.ElapsedMilliseconds;
            Console.WriteLine("BGW-UV: extracted indices in " + (stepTime1 - stepTime0));

            // Report Progress
            thisWorker.ReportProgress(100);

            _BGW_UV_resetEvent.Set();
        }

        private List<int> findIn2Array(List<int> a, List<int> b)
        {
            return a.AsParallel().Where( elem => b.Contains(elem)).ToList(); 
        }
        private List<int> findIn2ArrayExcept_par(List<int> a, List<int> b, int exception)
        {
            return a.AsParallel().Where(elem => ((elem != exception) && b.Contains(elem))).ToList();
        }
        private List<int> findIn2ArrayExcept(List<int> a, List<int> b, int exception)
        {
            return a.FindAll(elem => ((elem != exception) && b.Contains(elem)));
        }

        private void backgroundWorker_Outline_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker thisWorker = sender as BackgroundWorker;
            var slice = 16384;
            loadStart = perfCount.ElapsedMilliseconds;

            var data = new List<trianglePackedData>();
           

            // prepare data
            for (var i = 0; i < uniqueIndices.Count; i += 3)
            {
                var tpd = new trianglePackedData();
                tpd.indiceList.Add(uniqueIndices[i + 0]);
                tpd.indiceList.Add(uniqueIndices[i + 1]);
                tpd.indiceList.Add(uniqueIndices[i + 2]);
                tpd.triangleIndex = i / 3;
                tpd.ti1 = findIn2ArrayExcept(uniqueBoundTriangles[tpd.indiceList[0]], uniqueBoundTriangles[tpd.indiceList[1]], i / 3);
                tpd.ti2 = findIn2ArrayExcept(uniqueBoundTriangles[tpd.indiceList[1]], uniqueBoundTriangles[tpd.indiceList[2]], i / 3);
                tpd.ti3 = findIn2ArrayExcept(uniqueBoundTriangles[tpd.indiceList[2]], uniqueBoundTriangles[tpd.indiceList[0]], i / 3);
                data.Add(tpd);

                // Report Progress
                if ((uniqueIndices.Count > slice) && ((i % slice) == 0))
                {
                    thisWorker.ReportProgress((50 * i) / uniqueIndices.Count);

                    // Check for cancellation
                    if (thisWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        _BGW_EF_resetEvent.Set();
                        return;
                    }
                }
            }

            // fill edge data ignoring orphan edges (would imply an open hull not suitable for 3D printing)
            for (var i = 0; i < data.Count; ++i)
            {
                // shared edge with t1, not checked before
                if ((data[i].ti1.Count > 0) && (data[i].ti1[0] > i)) 
                {
                    var edgesIndices = findIn2Array(data[i].indiceList, data[data[i].ti1[0]].indiceList);
                    var nes = new edgeStruct
                    {
                        I1 = edgesIndices[0],
                        I2 = edgesIndices[1],
                        T1 = data[i].triangleIndex,
                        T2 = data[data[i].ti1[0]].triangleIndex
                    };
                    edges.Add(nes);

                }
                // shared edge with t2, not checked before
                if ((data[i].ti2.Count > 0) && (data[i].ti2[0] > i))
                {
                    var edgesIndices = findIn2Array(data[i].indiceList, data[data[i].ti2[0]].indiceList);
                    var nes = new edgeStruct
                    {
                        I1 = edgesIndices[0],
                        I2 = edgesIndices[1],
                        T1 = data[i].triangleIndex,
                        T2 = data[data[i].ti2[0]].triangleIndex
                    };
                    edges.Add(nes);
                }
                // shared edge with t3, not checked before
                if ((data[i].ti3.Count > 0) && (data[i].ti3[0] > i))
                {
                    var edgesIndices = findIn2Array(data[i].indiceList, data[data[i].ti3[0]].indiceList);
                    var nes = new edgeStruct
                    {
                        I1 = edgesIndices[0],
                        I2 = edgesIndices[1],
                        T1 = data[i].triangleIndex,
                        T2 = data[data[i].ti3[0]].triangleIndex
                    };
                    edges.Add(nes);
                }

                // Report Progress
                if ((data.Count > slice) && ((i % slice) == 0))
                {
                    thisWorker.ReportProgress(50 + (50 * i) / data.Count);

                    // Check for cancellation
                    if (thisWorker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        _BGW_EF_resetEvent.Set();
                        return;
                    }
                }
            }

            _BGW_EF_resetEvent.Set();
        }

        private void backgroundWorker_Outline_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label1.Text = status + "Outline Canceled!";
                OutlineReady = false;
            }
            else if (e.Error != null)
            {
                label1.Text = status + "Outline Error: " + e.Error.Message;
                OutlineReady = false;
            }
            else
            {
                Console.WriteLine("num edges " + edges.Count);
                Console.WriteLine("edge found in " + (perfCount.ElapsedMilliseconds - loadStart));
                label1.Text = status;

                // generate outline list
                genOutlineList();
                OutlineReady = true;

                ReDraw();
            }
        }

        private void backgroundWorker_Outline_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label1.Text = status + $"Generating outline {e.ProgressPercentage}%";
        }


        private void CancelBGW()
        {
            if ( (backgroundWorker_Outline.IsBusy) && (backgroundWorker_Outline.WorkerSupportsCancellation == true))
            {
                backgroundWorker_Outline.CancelAsync();
                _BGW_EF_resetEvent.WaitOne();
                OutlineReady = false;
                Application.DoEvents();
            }
            if ( (backgroundWorker_UniqueVertex.IsBusy) && (backgroundWorker_UniqueVertex.WorkerSupportsCancellation == true))
            {
                backgroundWorker_UniqueVertex.CancelAsync();
                _BGW_UV_resetEvent.WaitOne();
                UniqueVerticesReady = false;
                Application.DoEvents();
            }
        }

        private void renderPanel_Paint(object sender, PaintEventArgs e)
        {
            ReDraw();
        }

        private float sqlenSelector(Vector3 v)
        {
            if (holeAxisRadioButtonX.Checked) return STL_Loader.sqrlenYZ(v);
            if (holeAxisRadioButtonY.Checked) return STL_Loader.sqrlenXZ(v);
            if (holeAxisRadioButtonZ.Checked) return STL_Loader.sqrlenXY(v);
            return 0.0f;
        }
        private void centerTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (!holeCompPanel.Visible) return;

            // recomp with new value and reset all data and list
            var limit = centerLimitTrackBar.Value / 4.0f;
            var comp = centerCompTrackBar.Value / 200.0f;//+/- 40 steps for +/- 0.20

            var axisString = "Y";
            if (holeAxisRadioButtonX.Checked) axisString = "X";
            if (holeAxisRadioButtonY.Checked) axisString = "Y";
            if (holeAxisRadioButtonZ.Checked) axisString = "Z";

            label1.Text = "Center hole radius limit:" + limit.ToString("F3") + " " + axisString + " Comp:" + comp.ToString("F3");
            modString = axisString + " L" + (100 * centerLimitTrackBar.Value / 4).ToString("D3") + " C" + (1000 * centerCompTrackBar.Value / 200).ToString("D4");

            var limitsq = limit * limit; // srq
            Vector3 scale = new Vector3(1.0f + comp, 1.0f + comp, 1.0f + comp);
            if (holeAxisRadioButtonX.Checked) scale.X = 1.0f;
            if (holeAxisRadioButtonY.Checked) scale.Y = 1.0f;
            if (holeAxisRadioButtonZ.Checked) scale.Z = 1.0f;

            // apply offset to data
            newData = new List<FaceData>(loader.Triangles.Count);
            for (var i = 0; i < loader.Triangles.Count; ++i)
            {
                newData.Add(new FaceData
                {
                    V1 = (sqlenSelector(loader.Triangles[i].V1) > limitsq) ? loader.Triangles[i].V1 : (loader.Triangles[i].V1 * scale),
                    V2 = (sqlenSelector(loader.Triangles[i].V2) > limitsq) ? loader.Triangles[i].V2 : (loader.Triangles[i].V2 * scale),
                    V3 = (sqlenSelector(loader.Triangles[i].V3) > limitsq) ? loader.Triangles[i].V3 : (loader.Triangles[i].V3 * scale),
                    Normal = loader.Triangles[i].Normal,
                    Color = loader.Triangles[i].Color
                });
            }

            // create render list
            GL.DeleteLists(compList, 1);
            compList = GL.GenLists(1);
            GL.NewList(compList, ListMode.Compile);
            drawModel(!false, loader.Triangles.Count, newData);
            GL.EndList();
            //Console.WriteLine("Gen model compList data yields " + GL.GetError());


            GL.DeleteLists(holeList, 1);
            holeList = GL.GenLists(1);
            GL.NewList(holeList, ListMode.Compile);
            GL.Begin(PrimitiveType.Lines);

           // GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, holeColor);
           // GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, holeColor);
            var maxOffset = bbData.maxY + 0.001f;
            var midOffset = bbData.maxY / 2.0f;
            var lowOffset = -0.001f;

            for (var i = 0; i < 64; i++)
            {
                var s1 = limit * Math.Sin(2 * Math.PI * i / 64);
                var c1 = limit * Math.Cos(2 * Math.PI * i / 64);

                var s2 = limit * Math.Sin(2 * Math.PI * (i+1) / 64);
                var c2 = limit * Math.Cos(2 * Math.PI * (i+1) / 64);

                if (holeAxisRadioButtonX.Checked)
                {
                    GL.Color4(holeColor);
                    GL.Vertex3(lowOffset, s1, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(lowOffset, s2, c2);

                    GL.Color4(holeColor);
                    GL.Vertex3(midOffset, s1, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(midOffset, s2, c2);

                    GL.Color4(holeColor);
                    GL.Vertex3(maxOffset, s1, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(maxOffset, s2, c2);
                }
                if (holeAxisRadioButtonY.Checked)
                {
                    GL.Color4(holeColor);
                    GL.Vertex3(s1, lowOffset, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, lowOffset, c2);

                    GL.Color4(holeColor);
                    GL.Vertex3(s1, midOffset, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, midOffset, c2);

                    GL.Color4(holeColor);
                    GL.Vertex3(s1, maxOffset, c1);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, maxOffset, c2);
                }
                if (holeAxisRadioButtonZ.Checked)
                {
                    GL.Color4(holeColor);
                    GL.Vertex3(s1, c1, lowOffset);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, c2, lowOffset);

                    GL.Color4(holeColor);
                    GL.Vertex3(s1, c1, midOffset);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, c2, midOffset);

                    GL.Color4(holeColor);
                    GL.Vertex3(s1, c1, maxOffset);
                    GL.Color4(holeColor);
                    GL.Vertex3(s2, c2, maxOffset);
                }

            }
            GL.End();
            GL.EndList();

            ReDraw();
        }
    }
}
