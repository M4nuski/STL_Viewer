using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;

namespace STLViewer
{
    class STL_Writer
    {
        public List<FaceData> Triangles;
        public UInt32 NumTriangle => (UInt32)Triangles.Count;
        public bool Colored = false;
        public Vector4 defaultColor = new Vector4(0.8f, 0.8f, 0.8f, 1.0f);

        public STL_Writer(bool useColors = false)
        {
            Triangles = new List<FaceData>();
            Colored = useColors;
        }

        private static void WriteVector3(BinaryWriter writer, Vector3 v)
        {
            writer.Write(v.X);
            writer.Write(-v.Y);
            writer.Write(v.Z);
        }

        private static void WriteVertexData(BinaryWriter writer, FaceData vd)
        {
            WriteVector3(writer, vd.Normal);
            WriteVector3(writer, vd.V1);
            WriteVector3(writer, vd.V2);
            WriteVector3(writer, vd.V3);
            writer.Write(ConvertColors(vd.Color));
        }

        private static UInt16 ConvertColors(Vector4 color)
        {
            var r =( (UInt16)(color.X * 31.0f) ) & 0x001F;
            var g =( (UInt16)(color.Y * 31.0f) ) & 0x001F;
            var b =( (UInt16)(color.Z * 31.0f) ) & 0x001F;

            return (UInt16)((r) + (g << 5) + (b << 10));
        }

        private static void WriteColorHeader(BinaryWriter writer)
        {
            writer.Write(Convert.ToByte('C'));
            writer.Write(Convert.ToByte('O'));
            writer.Write(Convert.ToByte('L'));
            writer.Write(Convert.ToByte('O'));
            writer.Write(Convert.ToByte('R'));
            writer.Write(Convert.ToByte('='));
            for (var i = 0; i < 80 - 6; i++)
            {
                writer.Write(' ');
            }
        }

        private static void WriteStandardHeader(BinaryWriter writer)
        {
            for (var i = 0; i < 80; i++)
            {
                writer.Write(Convert.ToByte(' '));
            }
        }

        public void addFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal, Vector4 color)
        {
            Triangles.Add(new FaceData() { V1 = v1, V2 = v2, V3 = v3, Normal = normal, Color = color} );
        }

        public void addFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 normal)
        {
            Triangles.Add(new FaceData() { V1 = v1, V2 = v2, V3 = v3, Normal = normal, Color = defaultColor });
        }

        public void addFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector4 color)
        {
            Triangles.Add(new FaceData() { V1 = v1, V2 = v2, V3 = v3, Normal = getNormal(v1, v2, v3), Color = color });
        }

        public void writeToFile(string fileName, bool recalcNormals = false)
        {
            if (File.Exists(fileName)) throw new Exception("File already exists");
            var fileStream = File.Create(fileName);
            var binaryWriter = new BinaryWriter(fileStream);

            // Write Header
            if (Colored)
            {
                WriteColorHeader(binaryWriter);
            }
            else
            {
                WriteStandardHeader(binaryWriter);
            }

            // Write Numtriangles
            binaryWriter.Write(NumTriangle);

            // Write Data
            for (var i = 0; i < NumTriangle; i++)
            {
                if (recalcNormals) Triangles[i].Normal = getNormal(Triangles[i].V1, Triangles[i].V2, Triangles[i].V3);
                WriteVertexData(binaryWriter, Triangles[i]);
            }

            // Flush and complete
            binaryWriter.Flush();
            fileStream.Flush();
            binaryWriter.Close();
            fileStream.Close();
        }

        public Vector3 getNormal(Vector3 p0, Vector3 p1, Vector3 p2)
        {

            var v1 = new Vector3(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            var v2 = new Vector3(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
            var res = Vector3.Cross(v1, v2);

            res.Normalize();
            return res;
        }

       





    }


}
