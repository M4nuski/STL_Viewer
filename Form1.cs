using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;

using System.IO;
using Shell32;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace STLViewer
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
        private float[] pmap; // perspective matrix
        private const float _fov = 45.0f;
        // TODO private float smooth = 0.1f;

        //Directory and file content
        private List<string> dirList = new List<string>();
        private string basePath;
        private int currentIndex = -1;

        // current file stl data
        private string currentFile;
        private STL_Loader loader;
        private BoundingBoxData bbData;
        private Vector3 modelPos;
        private int colList, defList = -1;

        private List<FaceData> data;

        // user inputs
        private float px;// = 0.0f;
        private float py;// = 0.0f;
        private float pz;// = 300.0f;

        private float rx;// = 0.0f;
        private float ry;// = 0.0f;

        private int mouse_x;// = 0;
        private int mouse_y;// = 0;
        private bool mouse_btn;
        private const float mouseSpeed = 0.15f;
        private const float mouseWheelSpeed = 10.0f;

        private Vector3 pivot;

        private bool originalColors = true;
        private Vector4 defaultColor;

        private bool wiremode = false;

        public Form1()
        {
            perfCount.Start();
            InitializeComponent();
            pmap = new float[16];
            try
            {
                defaultColor = new Vector4(
                    (float)Properties.Settings.Default["defaultColor_R"],
                    (float)Properties.Settings.Default["defaultColor_G"],
                    (float)Properties.Settings.Default["defaultColor_B"],
                    (float)Properties.Settings.Default["defaultColor_A"]
                );
            }
            catch (Exception ex)
            {
                
                defaultColor = new Vector4( 0.75f, 0.75f, 0.75f, 1.0f);
                Console.WriteLine("error reading default color assembly properties: " + ex.Message);
            } 

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
            WindowInfo = Utilities.CreateWindowsWindowInfo(panel1.Handle);
            var WindowMode = new GraphicsMode(32, 24, 0, 0, 0, 2);
            WindowContext = new GraphicsContext(WindowMode, WindowInfo, 2, 0, GraphicsContextFlags.Default);

            WindowContext.MakeCurrent(WindowInfo);
            WindowContext.LoadAll();

            WindowContext.SwapInterval = 1;
            GL.Viewport(0, 0, panel1.Width, panel1.Height);

            GL.Enable(EnableCap.DepthTest);
            
            GL.Disable(EnableCap.CullFace);
            GL.ClearColor(35.0f / 255.0f, 105.0f / 255.0f, 219.0f / 255.0f, 1.0f);

            GL.Light(LightName.Light0, LightParameter.Ambient, new Color4(0.15f, 0.15f, 0.15f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Diffuse, new Color4(0.85f, 0.85f, 0.85f, 1.0f));
            GL.Light(LightName.Light0, LightParameter.Specular, new Color4(0.0f, 0.0f, 0.0f, 0.0f));
            GL.Light(LightName.Light0, LightParameter.Position, new Vector4(0.5f, 1.0f, 0.5f, 0.0f));

            GL.Enable(EnableCap.Light0);

//            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, new Color4(0.2f, 0.2f, 0.2f, 1.0f));
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
            var loadStart = perfCount.ElapsedMilliseconds;
            Text = Path.GetFullPath(basePath + currentFile);


            loader = new STL_Loader(basePath + currentFile);
            bbData = loader.getBondingBox();

            // center model on platform
            modelPos = new Vector3((bbData.maxX + bbData.minX) / -2.0f,
                -bbData.minY,
                (bbData.maxZ + bbData.minZ) / -2.0f);

            // center view on model
            setPerspective(_fov, (float)ClientSize.Width / ClientSize.Height, 0.1f, 4096.0f);
            pivot = new Vector3(0.0f, 0.0f, 0.0f);
            pz = Math.Max(bbData.maxX - bbData.minX, bbData.maxY - bbData.minY);
            pz = Math.Max(bbData.maxZ - bbData.minZ, pz);
            pz = Math.Max(2.5f * pz / (float)Math.Tan(_fov), 75.0f);

            py = (bbData.maxY - bbData.minY) / -2.0f;
            px = 0;
           // rx = 0;
           // ry = 0;

            // prepare model data
            if (loader?.NumTriangle > 0)
            {
                GL.DeleteLists(colList, 1);
                GL.DeleteLists(defList, 1);

                colList = GL.GenLists(1);                   
                GL.NewList(colList, ListMode.Compile);
                    drawModel(!loader.Colored);
                GL.EndList();
                Console.WriteLine(GL.GetError());

                if (!loader.Colored) defList = colList;
                else
                {
                    defList = GL.GenLists(1);
                    GL.NewList(defList, ListMode.Compile);
                        drawModel(true);
                    GL.EndList();
                    Console.WriteLine(GL.GetError());
                }

                label1.Text = currentFile + " " + loader.NumTriangle + " triangles (" + loader.Type + ")";
            }

            originalColors = loader.Colored;
            wiremode = false;
            Console.WriteLine("model loaded in " + (perfCount.ElapsedMilliseconds - loadStart));

            if (data != null)
            {
                data.Clear();
            }
            else
            {
                data = new List<FaceData>();
            }
            if (loader?.NumTriangle > 0) for (var i = 0; i < loader?.NumTriangle; i++) data.Add(loader.Triangles[i]);
        }

        private void drawModel(bool overrideColors)
        {
            GL.Begin(PrimitiveType.Triangles);
            for (var i = 0; i < loader.NumTriangle; i++)
            {
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Diffuse, (overrideColors) ? defaultColor : loader.Triangles[i].Color);
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Ambient, (overrideColors) ? defaultColor : loader.Triangles[i].Color);
                GL.Normal3(loader.Triangles[i].Normal);
                GL.Vertex3(loader.Triangles[i].V1);

                GL.Normal3(loader.Triangles[i].Normal);
                GL.Vertex3(loader.Triangles[i].V2);

                GL.Normal3(loader.Triangles[i].Normal);
                GL.Vertex3(loader.Triangles[i].V3);
            }
            GL.End();
        }

        private void resetSizes()
        {
            panel1.Size = ClientSize;
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
            if (WindowInfo != null)
            {

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
                GL.Viewport(0, 0, panel1.Width, panel1.Height);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.PolygonMode(MaterialFace.FrontAndBack, wiremode ? PolygonMode.Line : PolygonMode.Fill);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(pmap);        
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

                // model
                GL.Translate(modelPos);
                if (loader?.NumTriangle > 0) GL.CallList((originalColors) ? colList : defList);

                WindowContext.SwapBuffers();
            }
        }

        private void setPerspective(float FOV, float AR, float Near, float Far)
        {
            var f = (float)(1.0 / Math.Tan(FOV / 2.0));
            var nf = (float)(1.0 / (Near - Far));
            
            pmap[0] = f / AR;
            pmap[1] = 0.0f;
            pmap[2] = 0.0f;
            pmap[3] = 0.0f;
            pmap[4] = 0.0f;
            pmap[5] = f;
            pmap[6] = 0.0f;
            pmap[7] = 0.0f;
            pmap[8] = 0.0f;
            pmap[9] = 0.0f;
            pmap[10] = (Far + Near) * nf;
            pmap[11] = -1.0f;
            pmap[12] = 0.0f;
            pmap[13] = 0.0f;
            pmap[14] = 2.0f * Far * Near * nf;
            pmap[15] = 0.0f;
        }

       // using Shell32;


        public static Shell shell = new Shell();
        public static Folder RecyclingBin = shell.NameSpace(ShellSpecialFolderConstants.ssfBITBUCKET);


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                label2.Visible = !label2.Visible;
                ReDraw();
            }

            if (e.KeyCode == Keys.F12)
            {
                var stlw = new STL_Writer(true);
                var offset = new Vector3(0.0f, 0.0f, -0.15f/2.0f);

                // re structure : list of vertex, each face have a normal and ref 3 vertex index
                var numVertex = data.Count * 3;
                Console.WriteLine("num vertex " + numVertex);
                var indices = new int[numVertex];
                var uniqueVertex = new List<Vector3>();


                for (var i = 0; i < data.Count; ++i)
                {
                    // V1
                    var unique = true;
                    var curVert = data[i].V1;
                    for (var j = 0; j < uniqueVertex.Count; ++j)
                    {
                        if ((unique) && (Vector3.Equals(uniqueVertex[j], curVert)))
                        {
                            unique = false;
                            indices[i*3] = j;
                        }
                    }
                    if (unique)
                    {
                        uniqueVertex.Add(curVert);
                        indices[i*3] = uniqueVertex.Count - 1;
                    }


                    // V2
                    unique = true;
                    curVert = data[i].V2;
                    for (var j = 0; j < uniqueVertex.Count; ++j)
                    {
                        if ((unique) && (Vector3.Equals(uniqueVertex[j], curVert)))
                        {
                            unique = false;
                            indices[i * 3] = j;
                        }
                    }
                    if (unique)
                    {
                        uniqueVertex.Add(curVert);
                        indices[i * 3] = uniqueVertex.Count - 1;
                    }

                    // V3
                    unique = true;
                    curVert = data[i].V3;
                    for (var j = 0; j < uniqueVertex.Count; ++j)
                    {
                        if ((unique) && (Vector3.Equals(uniqueVertex[j], curVert)))
                        {
                            unique = false;
                            indices[i * 3] = j;
                        }
                    }
                    if (unique)
                    {
                        uniqueVertex.Add(curVert);
                        indices[i * 3] = uniqueVertex.Count - 1;
                    }
                }

                Console.WriteLine("num unique vertex " + uniqueVertex.Count);
                // regroup vertex

                // compensate for offset max per vertex

                // inflate back to stl structure

                //var counts = new int[data.Count];
                //for (var i = 0; i < data.Count; i++) counts[i] = 0;



                //     stlw.addFace(new Vector3(-10.0f, -10.0f, -10.0f), new Vector3(-10.0f, 10.0f, -10.0f), new Vector3(10.0f, -10.0f, -10.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                //     stlw.addFace(new Vector3(-10.0f, 10.0f, -10.0f), new Vector3(10.0f, 10.0f, -10.0f), new Vector3(10.0f, -10.0f, -10.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f));
                //     stlw.addFace(new Vector3(10.0f, -10.0f, -10.0f), new Vector3(10.0f, 10.0f, -10.0f), new Vector3(-10.0f, -10.0f, -10.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
                //  stlw.addFace(new Vector3(-10.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                //  stlw.addFace(new Vector3(-10.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                //  stlw.addFace(new Vector3(-10.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                try
                {

                    var fn = "testFile" + DateTime.Now.ToString("yy-mm-dd HH-MM-ss") + ".stl";
                    label1.Text = "Writing to " + fn;
                    stlw.writeToFile(fn);

                }
                catch
                {
                    Console.WriteLine("File Already Exists");
                    label1.Text = "Failed to write to file";
                }



            }


            if (e.KeyCode == Keys.Return)
            {
                // switch to model color or default color
                originalColors = !originalColors;
                ReDraw();
            }
            if (e.KeyCode == Keys.Back)
            {
                wiremode = !wiremode;
                ReDraw();
            }
            if (e.KeyCode == Keys.Right) 
            {

                // Load next model in folder
                currentIndex ++;
                if (currentIndex >= dirList.Count) currentIndex = dirList.Count - 1; 
                else if (dirList.Count > 0)
                {
                    currentFile = dirList[currentIndex];
                    loadModel();
                    ReDraw();
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                // Load previous model in folder
                currentIndex--;
                if (currentIndex < 0) currentIndex = 0;
                else if (dirList.Count > 0)
                {
                    currentFile = dirList[currentIndex];
                    loadModel();
                    ReDraw();
                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                // Delete model file (after confirmation dialog)
                if (MessageBox.Show(this, "Delete current file ?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    RecyclingBin.MoveHere(basePath + currentFile);

                    dirList.RemoveAt(currentIndex);

                    if (dirList.Count == 0)
                    {
                        loader = null;
                        currentIndex = -1;
                        ReDraw();
                    }
                    else if (dirList.Count == 1)
                    {
                        currentIndex = 0;
                        currentFile = dirList[currentIndex];
                        loadModel();
                        ReDraw();
                    } else
                    {
                        if (currentIndex >= dirList.Count) currentIndex = dirList.Count - 1;
                        currentFile = dirList[currentIndex];
                        loadModel();
                        ReDraw();
                    }
                }
            }
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
            GL.DeleteLists(colList, 1);
            GL.DeleteLists(defList, 1);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            var fna = (string[])e.Data.GetData("FileNameW");
            var newName = "";
            if ((fna != null) && (fna.Length > 0)) newName = fna[0];
            if (Path.GetExtension(newName) == ".stl") e.Effect = DragDropEffects.Copy;
        }
    }
}
