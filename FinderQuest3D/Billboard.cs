using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class Billboard : Mesh
    {
        private bool isStaticY;

        public Billboard(string name, int verticesCount, int facesCount, bool isStaticY)
            : base(name, verticesCount, facesCount)
        {
            this.IsStaticY = isStaticY;
        }

        public bool IsStaticY { get => isStaticY; set => isStaticY = value; }

        public void UpdateFacing(Camera camera)
        {
            if (camera == null) return;

            float dx = camera.Position.X - this.Position.X;
            float dz = camera.Position.Z - this.Position.Z;
            float angleY = (float)Math.Atan2(dx, dz);

            if (isStaticY)
            {
                this.Rotation = new Vector3(this.Rotation.X, angleY, this.Rotation.Z);
            }
            else
            {
                float dy = camera.Position.Y - this.Position.Y;
                float dist = (float)Math.Sqrt(dx * dx + dz * dz);
                float angleX = -(float)Math.Atan2(dy, dist);
                this.Rotation = new Vector3(angleX, angleY, this.Rotation.Z);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Billboard billboard &&
                   IsStaticY == billboard.IsStaticY &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return -157929914 + IsStaticY.GetHashCode() + base.GetHashCode();
        }
    }
}