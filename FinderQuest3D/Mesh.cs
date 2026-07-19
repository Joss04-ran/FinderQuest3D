using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class Mesh : IDisposable
    {
        private string name;
        private Vertex[] arrayVertices;
        private Faces[] arrayFaces;
        private Vector3 position;
        private Vector3 rotation;
        private Texture texture;

        public Mesh(string name, int verticesCount, int facesCount)
        {
            this.ArrayVertices = new Vertex[verticesCount];
            this.ArrayFaces = new Faces[facesCount];
            this.Name = name;
        }

        public string Name { get => name; set => name = value; }
        public Vertex[] ArrayVertices { get => arrayVertices; private set => arrayVertices = value; }
        public Faces[] ArrayFaces { get => arrayFaces; set => arrayFaces = value; }
        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Rotation { get => rotation; set => rotation = value; }
        public Texture Texture { get => texture; set => texture = value; }

        // Cached Direct3D resources
        public SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        public SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        public ShaderResourceView TextureView { get; set; }

        public void Dispose()
        {
            VertexBuffer?.Dispose();
            IndexBuffer?.Dispose();
            TextureView?.Dispose();
        }

        public override bool Equals(object obj)
        {
            return obj is Mesh mesh &&
                   name == mesh.name &&
                   EqualityComparer<Vertex[]>.Default.Equals(arrayVertices, mesh.arrayVertices) &&
                   EqualityComparer<Faces[]>.Default.Equals(arrayFaces, mesh.arrayFaces) &&
                   position.Equals(mesh.position) &&
                   rotation.Equals(mesh.rotation) &&
                   EqualityComparer<Texture>.Default.Equals(texture, mesh.texture);
        }

        public override int GetHashCode()
        {
            int hashCode = 350619652;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex[]>.Default.GetHashCode(arrayVertices);
            hashCode = hashCode * -1521134295 + EqualityComparer<Faces[]>.Default.GetHashCode(arrayFaces);
            hashCode = hashCode * -1521134295 + position.GetHashCode();
            hashCode = hashCode * -1521134295 + rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Texture>.Default.GetHashCode(texture);
            return hashCode;
        }
    }
}