using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class Texture
    {
        private byte[] internalBuffer;
        private int width;
        private int height;

        public Texture(byte[] internalBuffer, int width, int height)
        {
            this.InternalBuffer = internalBuffer;
            this.Width = width;
            this.Height = height;
        }

        public byte[] InternalBuffer { get => internalBuffer; set => internalBuffer = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public override bool Equals(object obj)
        {
            return obj is Texture texture &&
                   EqualityComparer<byte[]>.Default.Equals(InternalBuffer, texture.InternalBuffer) &&
                   Width == texture.Width &&
                   Height == texture.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = -1634753485;
            hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(InternalBuffer);
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }
    }
}