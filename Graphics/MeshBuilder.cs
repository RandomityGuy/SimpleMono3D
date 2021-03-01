using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SimpleMono3D.Graphics.Geometry;
using SimpleMono3D.Graphics.Importers;
using SimpleMono3D.Graphics.Materials;


namespace SimpleMono3D.Graphics
{

    public class MeshBuilder
    {
        List<Vector3> Positions;
        List<FaceGroup> FaceGroups;
        List<int> NetIndices;
        List<Vector3> Normals;
        List<Vector2> UV;

        public MeshBuilder()
        {
            Positions = new List<Vector3>();
            FaceGroups = new List<FaceGroup>();
            NetIndices = new List<int>();
            Normals = new List<Vector3>();
            UV = new List<Vector2>();
        }

        public void AddTriangle(Vector3 v1,Vector3 v2,Vector3 v3,Vector2 uv1,Vector2 uv2,Vector2 uv3,Material m)
        {
            var a = v1 - v2;
            var b = v3 - v2;
            var norm = Vector3.Cross(a,b);
            norm.Normalize();
            Normals.Add(norm);
            Normals.Add(norm);
            Normals.Add(norm);
            Positions.Add(v1);
            Positions.Add(v2);
            Positions.Add(v3);
            UV.Add(uv1);
            UV.Add(uv2);
            UV.Add(uv3);

            var facegroup = FaceGroups.Find(x => x.Material == m);
            if (facegroup != null)
            {
                if (facegroup.Indices == null)
                    facegroup.Indices = new List<int>();

                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Material = m;
            }
            else
            {
                facegroup = new FaceGroup();
                facegroup.Indices = new List<int>();
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Material = m;
                FaceGroups.Add(facegroup);
            }
        }

        public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3, Material m)
        {
            var a = v1 - v2;
            var b = v3 - v2;
            var norm = Vector3.Cross(a, b);
            norm.Normalize();
            Normals.Add(norm);
            Normals.Add(norm);
            Normals.Add(norm);
            Positions.Add(v1);
            Positions.Add(v2);
            Positions.Add(v3);
            UV.Add(Vector2.Zero);
            UV.Add(Vector2.Zero);
            UV.Add(Vector2.Zero);
            var facegroup = FaceGroups.Find(x => x.Material == m);
            if (facegroup != null)
            {
                if (facegroup.Indices == null)
                    facegroup.Indices = new List<int>();

                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Material = m;
            }
            else
            {
                facegroup = new FaceGroup();
                facegroup.Indices = new List<int>();
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Indices.Add(NetIndices.Count);
                NetIndices.Add(NetIndices.Count);
                facegroup.Material = m;
                FaceGroups.Add(facegroup);
            }
        }

        public void AddCube(Vector3 centre, Vector3 size)
        {
            var uv1 = new Vector2(0, 0);
            var uv2 = new Vector2(0, 1);
            var uv3 = new Vector2(1, 0);
            var uv4 = new Vector2(1, 1);

            //BackFace
            var v1 = new Vector3(centre.X - (size.X / 2), centre.Y - (size.Y / 2), centre.Z - (size.Z / 2)); //Bottom left back
            var v2 = new Vector3(centre.X - (size.X / 2), centre.Y + (size.Y / 2), centre.Z - (size.Z / 2)); //Top left back
            var v3 = new Vector3(centre.X + (size.X / 2), centre.Y - (size.Y / 2), centre.Z - (size.Z / 2)); //Bottom right back
            var v4 = new Vector3(centre.X + (size.X / 2), centre.Y + (size.Y / 2), centre.Z - (size.Z / 2)); //Top right back
            AddTriangle(v1, v2, v3, uv3, uv4, uv1,ColorMaterials.White);
            AddTriangle(v3, v2, v4,uv1, uv4, uv2, ColorMaterials.White);

            //Frontface
            var v5 = new Vector3(centre.X - (size.X / 2), centre.Y - (size.Y / 2), centre.Z + (size.Z / 2)); //Bottom left front
            var v6 = new Vector3(centre.X - (size.X / 2), centre.Y + (size.Y / 2), centre.Z + (size.Z / 2)); //Top left front
            var v7 = new Vector3(centre.X + (size.X / 2), centre.Y - (size.Y / 2), centre.Z + (size.Z / 2)); //Bottom right front
            var v8 = new Vector3(centre.X + (size.X / 2), centre.Y + (size.Y / 2), centre.Z + (size.Z / 2)); //Top right front
            AddTriangle(v7, v6, v5,uv3,uv2,uv1, ColorMaterials.White);
            AddTriangle(v6, v7, v8,uv2,uv3,uv4, ColorMaterials.White);

            //Leftface
            AddTriangle(v1, v5, v2,uv3,uv1,uv4, ColorMaterials.White);
            AddTriangle(v6, v2, v5,uv2,uv3,uv1, ColorMaterials.White);

            //Rightface
            AddTriangle(v4, v8, v7, uv3, uv1, uv4, ColorMaterials.White);
            AddTriangle(v7, v3, v4, uv2, uv3, uv1, ColorMaterials.White);

            //Topface
            AddTriangle(v6, v8, v2,uv1,uv3,uv2, ColorMaterials.White);
            AddTriangle(v8, v4, v2,uv3,uv4,uv2, ColorMaterials.White);

            //Bottomface
            AddTriangle(v7, v5, v1, uv1, uv3, uv2, ColorMaterials.White);
            AddTriangle(v1, v3, v7,uv1,uv4,uv2, ColorMaterials.White);
        }

        public void AddObject(Obj o)
        {
            var texmats = new List<Material>();

            foreach (var face in o.Faces)
            {
                var tex = texmats.Find(a => a.Name == face.material);
                if (tex == null)
                {
                    if (face.material != null)
                        tex = TextureMaterial.Load(face.material);
                    else
                        tex = ColorMaterials.White;
                    texmats.Add(tex);
                }


                AddTriangle(o.Points[face.v1], o.Points[face.v2], o.Points[face.v3], o.UV[face.uv1], o.UV[face.uv2], o.UV[face.uv3],tex);
            }
        }

        public MeshGeometry Build()
        {
            var geo = new Geometry.MeshGeometry();
            geo.Positions = Positions;
            geo.FaceGroups = FaceGroups;
            geo.Normals = Normals;
            //so.Normals.ForEach(a => a *= -1);
            geo.UV = UV;
            
            //geo.Material = Materials.ColorMaterial.FromColor(Color.Blue);

            return geo;
        }
    }
}
