using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace FinderQuest3D
{
    // Structured buffer data for HLSL constant buffer 
    [StructLayout(LayoutKind.Sequential)]
    public struct Data
    {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;
        public Vector4 LightPos; 
    }
}