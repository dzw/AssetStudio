using System;
using System.IO;
using System.Text;

namespace AssetStudio.Export.Exporters
{
    /// <summary>
    /// Exporter for Mesh assets.
    /// </summary>
    public class MeshExporter : ExporterBase
    {
        public override AssetType ExportType => AssetType.Model;

        public override bool CanExport(ClassIDType type)
        {
            return type == ClassIDType.Mesh;
        }

        public override string GetFileExtension(Object asset, ExportOptions options)
        {
            return options.ModelFormat == ModelFormat.Obj ? ".obj" : ".glb";
        }

        public override bool Export(Object asset, string exportPath, ExportOptions options)
        {
            var mesh = asset as Mesh;
            if (mesh == null)
            {
                return false;
            }

            if (mesh.m_VertexCount <= 0)
            {
                return false;
            }

            string extension = GetFileExtension(asset, options);
            string fileName = FixFileName(mesh.m_Name);
            string filePath = GetUniqueFilePath(exportPath, fileName, extension, asset.m_PathID.ToString());

            if (options.ModelFormat == ModelFormat.Obj)
            {
                return ExportAsObj(mesh, filePath);
            }
            else
            {
                // GLB export would require additional dependencies
                // Fall back to OBJ for now
                return ExportAsObj(mesh, filePath);
            }
        }

        private bool ExportAsObj(Mesh mesh, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("g " + mesh.m_Name);

            #region Vertices
            if (mesh.m_Vertices == null || mesh.m_Vertices.Length == 0)
            {
                return false;
            }

            int c = 3;
            if (mesh.m_Vertices.Length == mesh.m_VertexCount * 4)
            {
                c = 4;
            }

            for (int v = 0; v < mesh.m_VertexCount; v++)
            {
                sb.AppendFormat("v {0} {1} {2}\r\n", -mesh.m_Vertices[v * c], mesh.m_Vertices[v * c + 1], mesh.m_Vertices[v * c + 2]);
            }
            #endregion

            #region UV
            if (mesh.m_UV0?.Length > 0)
            {
                c = 4;
                if (mesh.m_UV0.Length == mesh.m_VertexCount * 2)
                {
                    c = 2;
                }
                else if (mesh.m_UV0.Length == mesh.m_VertexCount * 3)
                {
                    c = 3;
                }

                for (int v = 0; v < mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vt {0} {1}\r\n", mesh.m_UV0[v * c], mesh.m_UV0[v * c + 1]);
                }
            }
            #endregion

            #region Normals
            if (mesh.m_Normals?.Length > 0)
            {
                if (mesh.m_Normals.Length == mesh.m_VertexCount * 3)
                {
                    c = 3;
                }
                else if (mesh.m_Normals.Length == mesh.m_VertexCount * 4)
                {
                    c = 4;
                }

                for (int v = 0; v < mesh.m_VertexCount; v++)
                {
                    sb.AppendFormat("vn {0} {1} {2}\r\n", -mesh.m_Normals[v * c], mesh.m_Normals[v * c + 1], mesh.m_Normals[v * c + 2]);
                }
            }
            #endregion

            #region Face
            int sum = 0;
            for (var i = 0; i < mesh.m_SubMeshes.Length; i++)
            {
                sb.AppendLine($"g {mesh.m_Name}_{i}");
                int indexCount = (int)mesh.m_SubMeshes[i].indexCount;
                var end = sum + indexCount / 3;
                for (int f = sum; f < end; f++)
                {
                    sb.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\r\n",
                        mesh.m_Indices[f * 3 + 2] + 1,
                        mesh.m_Indices[f * 3 + 1] + 1,
                        mesh.m_Indices[f * 3] + 1);
                }
                sum = end;
            }
            #endregion

            sb.Replace("NaN", "0");
            File.WriteAllText(filePath, sb.ToString());
            return true;
        }
    }
}
