using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class Camera
    {
        private Vector3 position;
        private float yaw;
        private Vector3 forward;
        private Vector3 sideWalk;
        private Vector3 target;

        public Camera(Vector3 position, float yaw)
        {
            this.Position = position;
            this.Yaw = yaw;
            this.Forward = new Vector3((float)Math.Sin(Yaw), 0.0f, (float)Math.Cos(this.Yaw));
            this.SideWalk = new Vector3((float)Math.Cos(Yaw), 0.0f, -(float)Math.Sin(Yaw));
            this.Target = this.Forward + this.SideWalk;
        }

        public Vector3 Position { get => position; set => position = value; }
        public float Yaw 
        { 
            get => yaw; 
            set 
            { 
                yaw = value; 
                this.Forward = new Vector3((float)Math.Sin(yaw), 0.0f, (float)Math.Cos(yaw));
                this.SideWalk = new Vector3((float)Math.Cos(yaw), 0.0f, -(float)Math.Sin(yaw));
                this.Target = this.Forward + this.SideWalk;
            } 
        }
        public Vector3 Forward { get => forward; private set => forward = value; }
        public Vector3 SideWalk { get => sideWalk; private set => sideWalk = value; }
        public Vector3 Target { get => target; private set => target = value; }

        public override bool Equals(object obj)
        {
            return obj is Camera camera &&
                   Position.Equals(camera.Position) &&
                   Yaw == camera.Yaw &&
                   Forward.Equals(camera.Forward) &&
                   SideWalk.Equals(camera.SideWalk) &&
                   Target.Equals(camera.Target);
        }

        public override int GetHashCode()
        {
            int hashCode = -331022445;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Yaw.GetHashCode();
            hashCode = hashCode * -1521134295 + Forward.GetHashCode();
            hashCode = hashCode * -1521134295 + SideWalk.GetHashCode();
            hashCode = hashCode * -1521134295 + Target.GetHashCode();
            return hashCode;
        }
    }
}