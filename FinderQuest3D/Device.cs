using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FinderQuest3D
{
    public class Device
    {
        private SharpDX.Direct3D11.Device d3dDevice;
        private DeviceContext context;
        private SwapChain swapChain;
        private RenderTargetView renderView;
        private DepthStencilView depthView;
        private Texture2D depthBuffer;

        private VertexShader vertexShader;
        private PixelShader pixelShader;
        private InputLayout layout;
        private SamplerState samplerState;
        private SharpDX.Direct3D11.Buffer constantBuffer;
        private RasterizerState rasterizerState;
        private BlendState blendState;
        private bool isDisposed;

        private int width;
        private int height;

        public Device(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.IsDisposed = false;
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public bool IsDisposed { get => isDisposed; private set => isDisposed = value; }

        public Device(IntPtr windowHandle, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.IsDisposed = false;
            // Setup SwapChain description pointing to window handle
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(width, height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = windowHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Initialize Direct3D 11 Device and SwapChain
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out d3dDevice, out swapChain);
            context = d3dDevice.ImmediateContext;

            // Create RenderTargetView from SwapChain backbuffer
            using (var backBuffer = swapChain.GetBackBuffer<Texture2D>(0))
            {
                renderView = new RenderTargetView(d3dDevice, backBuffer);
            }

            // Create DepthBuffer (Z-Buffer) and DepthStencilView
            var depthDesc = new Texture2DDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = width,
                Height = height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };
            depthBuffer = new Texture2D(d3dDevice, depthDesc);
            depthView = new DepthStencilView(d3dDevice, depthBuffer);

            // Set viewport
            context.Rasterizer.SetViewport(new ViewportF(0, 0, width, height, 0.0f, 1.0f));

            // Load and compile shaders from shaders.hlsl
            string shaderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shaders.hlsl");

            var vsBytecode = ShaderBytecode.CompileFromFile(shaderPath, "VS", "vs_4_0");
            vertexShader = new VertexShader(d3dDevice, vsBytecode);

            var psBytecode = ShaderBytecode.CompileFromFile(shaderPath, "PS", "ps_4_0");
            pixelShader = new PixelShader(d3dDevice, psBytecode);

            // Create Input Layout for Vertex struct binding
            layout = new InputLayout(d3dDevice, ShaderSignature.GetInputSignature(vsBytecode), new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0),
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 20, 0)
            });

            // Create Texture Sampler State (wrapping/repeating mode)
            var samplerDesc = SamplerStateDescription.Default();
            samplerDesc.Filter = SharpDX.Direct3D11.Filter.MinMagMipPoint; // pick nearest colour without blending
            samplerDesc.AddressU = TextureAddressMode.Wrap;
            samplerDesc.AddressV = TextureAddressMode.Wrap;
            samplerDesc.AddressW = TextureAddressMode.Wrap;
            samplerState = new SamplerState(d3dDevice, samplerDesc);

            var rasterizerDesc = new RasterizerStateDescription()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = false,
                IsDepthClipEnabled = true
            };
            rasterizerState = new RasterizerState(d3dDevice, rasterizerDesc);
            context.Rasterizer.State = rasterizerState;

            var blendDesc = new BlendStateDescription();
            blendDesc.RenderTarget[0].IsBlendEnabled = true;
            blendDesc.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            blendDesc.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            blendDesc.RenderTarget[0].BlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].SourceAlphaBlend = BlendOption.One;
            blendDesc.RenderTarget[0].DestinationAlphaBlend = BlendOption.Zero;
            blendDesc.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            blendDesc.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;

            blendState = new BlendState(d3dDevice, blendDesc);
            context.OutputMerger.SetBlendState(blendState);

            // Create Constant Buffer for Matrix transformations and Light Pos
            constantBuffer = new SharpDX.Direct3D11.Buffer(
                d3dDevice,
                Utilities.SizeOf<Data>(),
                ResourceUsage.Default,
                BindFlags.ConstantBuffer,
                CpuAccessFlags.None,
                ResourceOptionFlags.None,
                0
            );
        }

        public ShaderResourceView LoadTexture(string path)
        {
            if (!File.Exists(path))
                return null;

            using (var bitmap = new System.Drawing.Bitmap(path))
            {
                var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                var textureDesc = new Texture2DDescription()
                {
                    Width = bitmap.Width,
                    Height = bitmap.Height,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.B8G8R8A8_UNorm,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Immutable,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                var dataRect = new DataRectangle(bmpData.Scan0, bitmap.Width * 4);
                var tex2D = new Texture2D(d3dDevice, textureDesc, dataRect);
                var srv = new ShaderResourceView(d3dDevice, tex2D);

                bitmap.UnlockBits(bmpData);
                tex2D.Dispose();

                return srv;
            }
        }

        public ShaderResourceView CreateTextureSRV(Texture texture)
        {
            if (texture == null || texture.InternalBuffer == null)
                return null;

            try
            {
                using (var ms = new MemoryStream(texture.InternalBuffer))
                using (var bitmap = new System.Drawing.Bitmap(ms))
                {
                    var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    var textureDesc = new Texture2DDescription()
                    {
                        Width = bitmap.Width,
                        Height = bitmap.Height,
                        MipLevels = 1,
                        ArraySize = 1,
                        Format = Format.B8G8R8A8_UNorm,
                        SampleDescription = new SampleDescription(1, 0),
                        Usage = ResourceUsage.Immutable,
                        BindFlags = BindFlags.ShaderResource,
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None
                    };

                    var dataRect = new DataRectangle(bmpData.Scan0, bitmap.Width * 4);
                    var tex2D = new Texture2D(d3dDevice, textureDesc, dataRect);
                    var srv = new ShaderResourceView(d3dDevice, tex2D);

                    bitmap.UnlockBits(bmpData);
                    tex2D.Dispose();

                    return srv;
                }
            }
            catch
            {
                var textureDesc = new Texture2DDescription()
                {
                    Width = texture.Width,
                    Height = texture.Height,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.B8G8R8A8_UNorm,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Immutable,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                using (var stream = DataStream.Create(texture.InternalBuffer, true, false))
                {
                    var dataRect = new DataRectangle(stream.DataPointer, texture.Width * 4);
                    var tex2D = new Texture2D(d3dDevice, textureDesc, dataRect);
                    var srv = new ShaderResourceView(d3dDevice, tex2D);
                    tex2D.Dispose();
                    return srv;
                }
            }
        }

        public void Clear(Color4 color)
        {
            if (this.IsDisposed || context == null || renderView == null || depthView == null) return;
            context.ClearRenderTargetView(renderView, color);
            context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
        }

        public void Present()
        {
            if (this.IsDisposed || swapChain == null) return;
            swapChain.Present(1, PresentFlags.None);
        }

        public void Render(Mesh mesh, Camera camera)
        {
            if (this.IsDisposed || context == null || mesh == null || camera == null)
                return;

            if (mesh.VertexBuffer == null)
            {
                mesh.VertexBuffer = SharpDX.Direct3D11.Buffer.Create(d3dDevice, BindFlags.VertexBuffer, mesh.ArrayVertices);
            }
            if (mesh.IndexBuffer == null)
            {
                int[] indices = new int[mesh.ArrayFaces.Length * 3];
                for (int i = 0; i < mesh.ArrayFaces.Length; i++)
                {
                    indices[i * 3 + 0] = mesh.ArrayFaces[i].A;
                    indices[i * 3 + 1] = mesh.ArrayFaces[i].B;
                    indices[i * 3 + 2] = mesh.ArrayFaces[i].C;
                }
                mesh.IndexBuffer = SharpDX.Direct3D11.Buffer.Create(d3dDevice, BindFlags.IndexBuffer, indices);
            }
            if (mesh.TextureView == null && mesh.Texture != null)
            {
                mesh.TextureView = CreateTextureSRV(mesh.Texture);
            }

            context.OutputMerger.SetTargets(depthView, renderView);

            context.InputAssembler.InputLayout = layout;
            context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            context.VertexShader.Set(vertexShader);
            context.PixelShader.Set(pixelShader);
            context.PixelShader.SetSampler(0, samplerState);

            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(mesh.VertexBuffer, Utilities.SizeOf<Vertex>(), 0));
            context.InputAssembler.SetIndexBuffer(mesh.IndexBuffer, Format.R32_UInt, 0);

            Matrix world = Matrix.RotationYawPitchRoll(mesh.Rotation.Y, mesh.Rotation.X, mesh.Rotation.Z) * Matrix.Translation(mesh.Position);
            Matrix view = Matrix.LookAtLH(camera.Position, camera.Position + camera.Forward, Vector3.Up);
            Matrix projection = Matrix.PerspectiveFovLH((float)Math.PI / 4.0f, (float)width / height, 0.1f, 100.0f);

            world.Transpose();
            view.Transpose();
            projection.Transpose();

            var data = new Data()
            {
                World = world,
                View = view,
                Projection = projection,
                LightPos = new Vector4(0.3f, -1.0f, 0.3f, 0.0f)
            };

            context.UpdateSubresource(ref data, constantBuffer);
            context.VertexShader.SetConstantBuffer(0, constantBuffer);
            context.PixelShader.SetConstantBuffer(0, constantBuffer);

            if (mesh.TextureView != null)
            {
                context.PixelShader.SetShaderResource(0, mesh.TextureView);
            }
            else
            {
                context.PixelShader.SetShaderResource(0, null);
            }

            context.DrawIndexed(mesh.ArrayFaces.Length * 3, 0, 0);
        }


        public override bool Equals(object obj)
        {
            return obj is Device device &&
                   width == device.width &&
                   height == device.height;
        }

        public override int GetHashCode()
        {
            int hashCode = 1263118649;
            hashCode = hashCode * -1521134295 + width.GetHashCode();
            hashCode = hashCode * -1521134295 + height.GetHashCode();
            return hashCode;
        }

        public void Dispose()
        {
            try
            {
                if (this.IsDisposed) return;
                this.IsDisposed = true;

                constantBuffer?.Dispose();
                samplerState?.Dispose();
                rasterizerState?.Dispose();
                blendState?.Dispose();
                layout?.Dispose();
                vertexShader?.Dispose();
                pixelShader?.Dispose();
                depthView?.Dispose();
                depthBuffer?.Dispose();
                renderView?.Dispose();
                context?.Dispose();
                d3dDevice?.Dispose();
                swapChain?.Dispose();

                constantBuffer = null;
                samplerState = null;
                rasterizerState = null;
                blendState = null;
                layout = null;
                vertexShader = null;
                pixelShader = null;
                depthView = null;
                depthBuffer = null;
                renderView = null;
                context = null;
                d3dDevice = null;
                swapChain = null;
            }
            catch { 
                Environment.Exit(0);
            }
        }
    }
}