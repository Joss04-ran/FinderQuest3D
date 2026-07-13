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
    }
}