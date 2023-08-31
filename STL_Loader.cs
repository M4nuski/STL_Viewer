using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace STLViewer
{
    public class FaceData
    {
        public Vector3 V1, V2, V3; 
        public Vector3 Normal;
        public Vector4 Color;
    }

    public struct BoundingBoxData
    {
        public float maxX, maxY, maxZ;
        public float minX, minY, minZ;
    }

    class STL_Loader
    {
        public List<FaceData> Triangles = new List<FaceData>();
        public bool Colored;
        public string Type;
        public enum NormalsRecalcMode { never, asNeeded, always };

        private static Vector3 ReadVector3(BinaryReader reader)
        {
            var x = reader.ReadSingle();
            var z = -reader.ReadSingle();
            var y = reader.ReadSingle();
            return new Vector3(
                x,
                y,
                z
                );
        }

        private static FaceData ReadFaceData(BinaryReader reader)
        {
            return new FaceData {
                Normal = ReadVector3(reader),
                V1 = ReadVector3(reader),
                V2 = ReadVector3(reader),
                V3 = ReadVector3(reader),
                Color = ConvertColors(reader.ReadUInt16())
            };
        }

        private static Vector4 ConvertColors(UInt16 ucolor)
        {
            return new Vector4(
                (ucolor & 0x001F)/31f,
                ((ucolor & 0x03E0) >> 5)/31f,
                ((ucolor & 0x7C00) >> 10)/31f,
                1.0f);
        }

        private static bool colorMarker(byte[] hrd)
        {
            var result  = (hrd[0] == 'C');
                result &= (hrd[1] == 'O');
                result &= (hrd[2] == 'L');
                result &= (hrd[3] == 'O');
                result &= (hrd[4] == 'R');
                result &= (hrd[5] == '=');

            return result;
        }

        private static bool textMarker(byte[] hrd)
        {
            var result  = (hrd[0] == 's');
                result &= (hrd[1] == 'o');
                result &= (hrd[2] == 'l');
                result &= (hrd[3] == 'i');
                result &= (hrd[4] == 'd');
                result &= (hrd[5] == ' ');

            return result;
        }

        private static float convertFloatString(string fs)
        {
            try
            {
                return float.Parse(fs, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            catch (Exception)
            {
                Console.WriteLine("error converting string " + fs);
                return 0.0f;
            }
        }

        public STL_Loader()
        {
            Colored = false;
            Type = "";
        }

        public void loadFile(string fileName, NormalsRecalcMode recalcNormals = NormalsRecalcMode.asNeeded )
        {
            char[] spaceSeperator = { ' ' };
            Colored = false;
            Type = "";
            Triangles.Clear();

            if (!File.Exists(fileName)) return;

            FileStream fileStream = null;
            BinaryReader binaryReader = null;
            StreamReader textStream = null;
            try
            {
                fileStream = File.OpenRead(fileName);
                binaryReader = new BinaryReader(fileStream);

                var header = new byte[80];
                binaryReader.Read(header, 0, 80);

                Colored = colorMarker(header);
                if (Colored) Console.WriteLine("Color header found");
                var normalsRecalculated = false;

                var textMode = textMarker(header);
                if (textMode)
                {
                    Console.WriteLine("Potential ASCII STL file");
                    textStream = File.OpenText(fileName);
                    var s = textStream.ReadLine()?.Trim();
                    if (s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[0].Trim() != "solid")
                        throw new Exception("Malformed ASCII STL (expected \"solid\")");
                    s = textStream.ReadLine()?.Trim() ?? "";
                    var ss = s.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries);
                    if (ss[0].Trim() != "facet")
                    {
                        textMode = false;
                        Console.WriteLine("Hybrid STL or malformed ASCII STL");
                        Type = "Hybrid";
                    }
                    else
                    {
                        Type = "ASCII";
                    }
                    textStream.Close();
                }
                else
                {
                    Type = "Binary";
                }

                if (textMode)
                {
                    textStream = File.OpenText(fileName);
                    Console.WriteLine("ASCII STL file");
                    Triangles.Clear();

                    var s = textStream.ReadLine()?.Trim(); 
                    // solid x
                    if (s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[0].Trim() != "solid") throw new Exception("Malformed ASCII STL (expected \"solid\")");
                    if (s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[1] != null) Console.WriteLine("solid name " + s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[1]); 

                    s = textStream.ReadLine()?.Trim();

                    while ((s != null) && (!textStream.EndOfStream) && (s.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[0].Trim() != "endsolid"))
                    {
                        var n = new Vector3(0.0f);
                        var v1 = new Vector3(0.0f);
                        var v2 = new Vector3(0.0f);
                        var v3 = new Vector3(0.0f);

                        // facet 
                        var ss = s.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries);
                        if (ss[0].Trim() != "facet") throw new Exception("Malformed ASCII STL (expected \"facet\") at triangle " + Triangles.Count);
                        // normal
                        if (ss[1].Trim() == "normal")
                        {
                            n.X = convertFloatString(ss[2].Trim());
                            n.Y = convertFloatString(ss[3].Trim());
                            n.Z = convertFloatString(ss[4].Trim());
                        }

                        // outer loop
                        s = textStream.ReadLine()?.Trim();
                        if (s?.Trim() != "outer loop") throw new Exception("Malformed ASCII STL (expected \"outer loop\") at triangle " + Triangles.Count);

                        // vertex 1
                        s = textStream.ReadLine()?.Trim(); 
                        ss = s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { "" };
                        if (ss[0].Trim() != "vertex")
                        {
                            throw new Exception("Malformed ASCII STL (expected \"vertex\") at triangle " + Triangles.Count);
                        }
                        else
                        {
                            v1.X = convertFloatString(ss[1].Trim());
                            v1.Y = convertFloatString(ss[2].Trim());
                            v1.Z = convertFloatString(ss[3].Trim());
                        }

                        // vertex 2
                        s = textStream.ReadLine()?.Trim();
                        ss = s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { "" };
                        if (ss[0].Trim() != "vertex")
                        {
                            throw new Exception("Malformed ASCII STL (expected \"vertex\") at triangle " + Triangles.Count);
                        }
                        else
                        {
                            v2.X = convertFloatString(ss[1].Trim());
                            v2.Y = convertFloatString(ss[2].Trim());
                            v2.Z = convertFloatString(ss[3].Trim());
                        }

                        // vertex 3
                        s = textStream.ReadLine()?.Trim();
                        ss = s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { "" };
                        if (ss[0].Trim() != "vertex")
                        {
                            throw new Exception("Malformed ASCII STL (expected \"vertex\") at triangle " + Triangles.Count);
                        }
                        else
                        {
                            v3.X = convertFloatString(ss[1].Trim());
                            v3.Y = convertFloatString(ss[2].Trim());
                            v3.Z = convertFloatString(ss[3].Trim());
                        }



                        // endloop
                        s = textStream.ReadLine()?.Trim();
                        if (s?.Trim() != "endloop") throw new Exception("Malformed ASCII STL (expected \"endloop\") at triangle " + Triangles.Count);

                        // endfacet
                        s = textStream.ReadLine()?.Trim();
                        if (s?.Trim() != "endfacet") throw new Exception("Malformed ASCII STL (expected \"endfacet\") at triangle " + Triangles.Count);


                        Triangles.Add( new FaceData()
                        {
                            Color = new Vector4(0.75f, 0.75f, 0.75f, 1.0f),
                            Normal = n,
                            V1 = v1,
                            V2 = v2,
                            V3 = v3
                        });




                        s = textStream.ReadLine()?.Trim();
                    } // end read line while
                    // endsolid x
                    if (s?.Split(spaceSeperator, StringSplitOptions.RemoveEmptyEntries)[0].Trim() != "endsolid") throw new Exception("Malformed ASCII STL (expected \"endsolid\"");



       
                    for (var i = 0; i < Triangles.Count; i++)
                    {

                        if ( (recalcNormals == NormalsRecalcMode.always) || ((Triangles[i].Normal.LengthSquared < 0.9f) && (recalcNormals == NormalsRecalcMode.asNeeded)) )
                        {
                            Triangles[i].Normal = getNormal(Triangles[i].V1, Triangles[i].V2, Triangles[i].V3);
                            normalsRecalculated = true;
                        }
                    }


                } // end textMode

                else
                {
                    Console.WriteLine("Binary STL file");
                    var NumTriangle = binaryReader.ReadUInt32();

                    for (var i = 0; i < NumTriangle; i++)
                    {
                        Triangles.Add(ReadFaceData(binaryReader));

                        if ((recalcNormals == NormalsRecalcMode.always) || ((Triangles[i].Normal.LengthSquared < 0.9f) && (recalcNormals == NormalsRecalcMode.asNeeded)))
                        {
                            Triangles[i].Normal = getNormal(Triangles[i].V1, Triangles[i].V2, Triangles[i].V3);
                            normalsRecalculated = true;
                        }
                    }
                        
                }

                if (normalsRecalculated) Console.WriteLine("Normals recalculated");

            }
            catch (Exception ex)
            {
                Console.WriteLine("STL_Loader Error: " + ex.Message);
                Triangles.Clear();
                Colored = false;
            }
            finally
            {
                binaryReader?.Close();
                fileStream?.Close();
                textStream?.Close();
            }

            // remove triangles whitout area
            /*var remCount = 0;
            for (int i = Triangles.Count-1; i >= 0; --i)
            {
                if ( (Vector3.DistanceSquared(Triangles[i].V1, Triangles[i].V2) < 0.0001) ||
                     (Vector3.DistanceSquared(Triangles[i].V2, Triangles[i].V3) < 0.0001) ||
                     (Vector3.DistanceSquared(Triangles[i].V3, Triangles[i].V1) < 0.0001) )
                {
                    Triangles.RemoveAt(i); 
                    remCount++;
                }
            }
            Console.WriteLine("Removed arealess triangles: " + remCount);
            */
        }

        public static Vector3 getNormal(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var v1 = new Vector3(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v2 = new Vector3(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
            var res = Vector3.Cross(v1, v2);

            res.Normalize();
            return res;
        }

        public static float sqrlenXY(Vector3 p)
        {
            return (p.X * p.X) + (p.Y * p.Y);
        }
        public static float sqrlenXZ(Vector3 p)
        {
            return (p.X * p.X) + (p.Z * p.Z);
        }
        public static float sqrlenYZ(Vector3 p)
        {
            return (p.Y * p.Y) + (p.Z * p.Z);
        }

        public static float lenXY(Vector3 p)
        {
            return (float)Math.Sqrt((p.X * p.X) + (p.Y * p.Y));
        }
        public static float lenXZ(Vector3 p)
        {
            return (float)Math.Sqrt((p.X * p.X) + (p.Z * p.Z));
        }
        public static float lenYZ(Vector3 p)
        {
            return (float)Math.Sqrt((p.Y * p.Y) + (p.Z * p.Z));
        }

        public BoundingBoxData getBoundingBox()
        {
            var res = new BoundingBoxData
            {
                maxX = float.NegativeInfinity,
                maxY = float.NegativeInfinity,
                maxZ = float.NegativeInfinity,
                minX = float.PositiveInfinity,
                minY = float.PositiveInfinity,
                minZ = float.PositiveInfinity
            };


            for (var i = 0; i < Triangles.Count; i++)
            {
                if (Triangles[i].V1.X > res.maxX) res.maxX = Triangles[i].V1.X;
                if (Triangles[i].V1.X < res.minX) res.minX = Triangles[i].V1.X;
                if (Triangles[i].V1.Y > res.maxY) res.maxY = Triangles[i].V1.Y;
                if (Triangles[i].V1.Y < res.minY) res.minY = Triangles[i].V1.Y;
                if (Triangles[i].V1.Z > res.maxZ) res.maxZ = Triangles[i].V1.Z;
                if (Triangles[i].V1.Z < res.minZ) res.minZ = Triangles[i].V1.Z;

                if (Triangles[i].V2.X > res.maxX) res.maxX = Triangles[i].V2.X;
                if (Triangles[i].V2.X < res.minX) res.minX = Triangles[i].V2.X;
                if (Triangles[i].V2.Y > res.maxY) res.maxY = Triangles[i].V2.Y;
                if (Triangles[i].V2.Y < res.minY) res.minY = Triangles[i].V2.Y;
                if (Triangles[i].V2.Z > res.maxZ) res.maxZ = Triangles[i].V2.Z;
                if (Triangles[i].V2.Z < res.minZ) res.minZ = Triangles[i].V2.Z;

                if (Triangles[i].V3.X > res.maxX) res.maxX = Triangles[i].V3.X;
                if (Triangles[i].V3.X < res.minX) res.minX = Triangles[i].V3.X;
                if (Triangles[i].V3.Y > res.maxY) res.maxY = Triangles[i].V3.Y;
                if (Triangles[i].V3.Y < res.minY) res.minY = Triangles[i].V3.Y;
                if (Triangles[i].V3.Z > res.maxZ) res.maxZ = Triangles[i].V3.Z;
                if (Triangles[i].V3.Z < res.minZ) res.minZ = Triangles[i].V3.Z;
            }


            return res;
        }

    }



}
