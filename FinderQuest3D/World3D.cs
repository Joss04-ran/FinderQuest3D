using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    public class World3D
    {
        private Mesh floor;
        private List<Billboard> billboards;
        private Mesh sky;

        public World3D()
        {
            this.Floor = null;
            this.Billboards = new List<Billboard>();
            this.Sky = null;
        }

        public Mesh Floor { get => floor; private set => floor = value; }
        public List<Billboard> Billboards { get => billboards; private set => billboards = value; }
        public Mesh Sky { get => sky; private set => sky = value; }

        public void GenerateFloor(int widthInTiles, 
            int heightInTiles, float tileSize, 
            string texturePath,
            System.Drawing.Rectangle? sourceRect = null)
        {
            int verticesCount = widthInTiles * heightInTiles * 4;
            int facesCount = widthInTiles * heightInTiles * 2;

            floor = new Mesh("Floor", verticesCount, facesCount);

            int vertexIdx = 0;
            int faceIdx = 0;

            for (int r = 0; r < heightInTiles; r++)
            {
                for (int c = 0; c < widthInTiles; c++)
                {
                    float xStart = c * tileSize;
                    float zStart = r * tileSize;

                    // Vertices
                    floor.ArrayVertices[vertexIdx + 0] = new Vertex
                    {
                        Coordinates = new Vector3(xStart, 0.0f, zStart),
                        TextureCoordinates = new Vector2(0.0f, 1.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f)
                    };
                    floor.ArrayVertices[vertexIdx + 1] = new Vertex
                    {
                        Coordinates = new Vector3(xStart + tileSize, 0.0f, zStart),
                        TextureCoordinates = new Vector2(1.0f, 1.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f)
                    };
                    floor.ArrayVertices[vertexIdx + 2] = new Vertex
                    {
                        Coordinates = new Vector3(xStart + tileSize, 0.0f, zStart + tileSize),
                        TextureCoordinates = new Vector2(1.0f, 0.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f)
                    };
                    floor.ArrayVertices[vertexIdx + 3] = new Vertex
                    {
                        Coordinates = new Vector3(xStart, 0.0f, zStart + tileSize),
                        TextureCoordinates = new Vector2(0.0f, 0.0f),
                        Normal = new Vector3(0.0f, 1.0f, 0.0f)
                    };

                    // Faces (2 triangles)
                    floor.ArrayFaces[faceIdx + 0] = new Faces
                    {
                        A = vertexIdx + 0,
                        B = vertexIdx + 2,
                        C = vertexIdx + 1
                    };
                    floor.ArrayFaces[faceIdx + 1] = new Faces
                    {
                        A = vertexIdx + 0,
                        B = vertexIdx + 3,
                        C = vertexIdx + 2
                    };

                    vertexIdx += 4;
                    faceIdx += 2;
                }
            }

            if (File.Exists(texturePath))
            {
                using (var fullBitmap = new System.Drawing.Bitmap(texturePath))
                {
                    System.Drawing.Bitmap bitmapToUse = fullBitmap;
                    bool shouldDispose = false;

                    if (sourceRect.HasValue)
                    {
                        var rect = sourceRect.Value;
                        if (rect.X >= 0 && rect.Y >= 0 && rect.Width > 0 && rect.Height > 0 &&
                            rect.X + rect.Width <= fullBitmap.Width && rect.Y + rect.Height <= fullBitmap.Height)
                        {
                            bitmapToUse = new System.Drawing.Bitmap(rect.Width, rect.Height);
                            using (var g = System.Drawing.Graphics.FromImage(bitmapToUse))
                            {
                                g.DrawImage(fullBitmap, new System.Drawing.Rectangle(0, 0, rect.Width, rect.Height), rect, System.Drawing.GraphicsUnit.Pixel);
                            }
                            shouldDispose = true;
                        }
                    }

                    var lockRect = new System.Drawing.Rectangle(0, 0, bitmapToUse.Width, bitmapToUse.Height);
                    var bmpData = bitmapToUse.LockBits(lockRect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int bytes = bitmapToUse.Width * bitmapToUse.Height * 4;
                    byte[] rgbValues = new byte[bytes];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    bitmapToUse.UnlockBits(bmpData);

                    floor.Texture = new Texture(rgbValues, bitmapToUse.Width, bitmapToUse.Height);

                    if (shouldDispose)
                    {
                        bitmapToUse.Dispose();
                    }
                }
            }
        }

        public void AddBillboard(string texturePath, Vector3 position, Vector3 scale)
        {
            var billboard = new Billboard("Tree", 4, 2, true);
            billboard.Position = position;

            float halfWidth = scale.X;
            float height = scale.Y;

            billboard.ArrayVertices[0] = new Vertex
            {
                Coordinates = new Vector3(-halfWidth, 0.0f, 0.0f),
                TextureCoordinates = new Vector2(0.0f, 1.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[1] = new Vertex
            {
                Coordinates = new Vector3(halfWidth, 0.0f, 0.0f),
                TextureCoordinates = new Vector2(1.0f, 1.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[2] = new Vertex
            {
                Coordinates = new Vector3(halfWidth, height, 0.0f),
                TextureCoordinates = new Vector2(1.0f, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[3] = new Vertex
            {
                Coordinates = new Vector3(-halfWidth, height, 0.0f),
                TextureCoordinates = new Vector2(0.0f, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };

            billboard.ArrayFaces[0] = new Faces { A = 0, B = 2, C = 1 };
            billboard.ArrayFaces[1] = new Faces { A = 0, B = 3, C = 2 };

            if (File.Exists(texturePath))
            {
                using (var bitmap = new System.Drawing.Bitmap(texturePath))
                {
                    var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int bytes = bitmap.Width * bitmap.Height * 4;
                    byte[] rgbValues = new byte[bytes];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    bitmap.UnlockBits(bmpData);
                    billboard.Texture = new Texture(rgbValues, bitmap.Width, bitmap.Height);
                }
            }

            billboards.Add(billboard);
        }

        public void AddBillboard(System.Drawing.Image image, Vector3 position, Vector3 scale)
        {
            var billboard = new Billboard("Person", 4, 2, true);
            billboard.Position = position;

            float halfWidth = scale.X;
            float height = scale.Y;

            billboard.ArrayVertices[0] = new Vertex
            {
                Coordinates = new Vector3(-halfWidth, 0.0f, 0.0f),
                TextureCoordinates = new Vector2(0.0f, 1.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[1] = new Vertex
            {
                Coordinates = new Vector3(halfWidth, 0.0f, 0.0f),
                TextureCoordinates = new Vector2(1.0f, 1.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[2] = new Vertex
            {
                Coordinates = new Vector3(halfWidth, height, 0.0f),
                TextureCoordinates = new Vector2(1.0f, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };
            billboard.ArrayVertices[3] = new Vertex
            {
                Coordinates = new Vector3(-halfWidth, height, 0.0f),
                TextureCoordinates = new Vector2(0.0f, 0.0f),
                Normal = new Vector3(0.0f, 0.0f, -1.0f)
            };

            billboard.ArrayFaces[0] = new Faces { A = 0, B = 2, C = 1 };
            billboard.ArrayFaces[1] = new Faces { A = 0, B = 3, C = 2 };

            if (image != null)
            {
                using (var bitmap = new System.Drawing.Bitmap(image))
                {
                    var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int bytes = bitmap.Width * bitmap.Height * 4;
                    byte[] rgbValues = new byte[bytes];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    bitmap.UnlockBits(bmpData);
                    billboard.Texture = new Texture(rgbValues, bitmap.Width, bitmap.Height);
                }
            }

            billboards.Add(billboard);
        }

        public void GenerateSky(string texturePath)
        {
            sky = new Mesh("Sky", 4, 2);

            float halfW = 80.0f;
            float halfH = 45.0f;

            // Large quad facing the camera. Normal has Y=0 so lighting treats it as a billboard.
            sky.ArrayVertices[0] = new Vertex { Coordinates = new Vector3(-halfW,  halfH, 0.0f), TextureCoordinates = new Vector2(0.0f, 0.0f), Normal = new Vector3(0.0f, 0.0f, -1.0f) };
            sky.ArrayVertices[1] = new Vertex { Coordinates = new Vector3( halfW,  halfH, 0.0f), TextureCoordinates = new Vector2(1.0f, 0.0f), Normal = new Vector3(0.0f, 0.0f, -1.0f) };
            sky.ArrayVertices[2] = new Vertex { Coordinates = new Vector3( halfW, -halfH, 0.0f), TextureCoordinates = new Vector2(1.0f, 1.0f), Normal = new Vector3(0.0f, 0.0f, -1.0f) };
            sky.ArrayVertices[3] = new Vertex { Coordinates = new Vector3(-halfW, -halfH, 0.0f), TextureCoordinates = new Vector2(0.0f, 1.0f), Normal = new Vector3(0.0f, 0.0f, -1.0f) };

            sky.ArrayFaces[0] = new Faces { A = 0, B = 2, C = 1 };
            sky.ArrayFaces[1] = new Faces { A = 0, B = 3, C = 2 };

            if (File.Exists(texturePath))
            {
                using (var bitmap = new System.Drawing.Bitmap(texturePath))
                {
                    var rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                    var bmpData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int bytes = bitmap.Width * bitmap.Height * 4;
                    byte[] rgbValues = new byte[bytes];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                    bitmap.UnlockBits(bmpData);
                    sky.Texture = new Texture(rgbValues, bitmap.Width, bitmap.Height);
                }
            }
        }

        public void Update(Camera camera)
        {
            if (sky != null && camera != null)
            {
                // Position the sky plane 90 units in front of the camera, centered at camera height
                sky.Position = camera.Position + camera.Forward * 90.0f;
                sky.Rotation = new Vector3(0.0f, camera.Yaw, 0.0f);

                // Pan texture coordinate U based on yaw angle
                float uOffset = camera.Yaw / (2.0f * (float)Math.PI);
                sky.ArrayVertices[0].TextureCoordinates = new Vector2(uOffset, 0.0f);
                sky.ArrayVertices[1].TextureCoordinates = new Vector2(uOffset + 1.0f, 0.0f);
                sky.ArrayVertices[2].TextureCoordinates = new Vector2(uOffset + 1.0f, 1.0f);
                sky.ArrayVertices[3].TextureCoordinates = new Vector2(uOffset, 1.0f);

                // Force reconstruction of the VertexBuffer to upload new texture coordinates
                if (sky.VertexBuffer != null)
                {
                    sky.VertexBuffer.Dispose();
                    sky.VertexBuffer = null;
                }
            }

            if (billboards == null) return;
            foreach (var billboard in billboards)
            {
                billboard.UpdateFacing(camera);
            }
        }

        public void InitializeFromMap(Map map, string treeTexturePath, string personTexturePath)
        {
            this.Floor = map.Mesh;
            this.Billboards.Clear();

            WalkAreas walkArea = map as WalkAreas;
            int personIndex = 0;

            float size = map.TileSize;
            for (int r = 0; r < map.Height; r++)
            {
                for (int c = 0; c < map.Width; c++)
                {
                    int cell = map.GetCell(c, r);
                    float x = c * size + size / 2.0f;
                    float z = r * size + size / 2.0f;

                    if (cell == 2)
                    {
                        AddBillboard(treeTexturePath, new Vector3(x, 0.0f, z), new Vector3(8.0f, 16.0f, -2.0f));
                    }
                    else if (cell == 3 || cell == 6)
                    {
                        Vector3 personPos = new Vector3(x, 0.0f, z);
                        if (walkArea != null && walkArea.ListPersons != null && personIndex < walkArea.ListPersons.Count)
                        {
                            var person = walkArea.ListPersons[personIndex];
                            person.Position = personPos; // Set the 3D position
                            if (person.Picture != null && person.Picture.Image != null)
                            {
                                AddBillboard(person.Picture.Image, personPos, new Vector3(6.0f, 12.0f, -1.5f));
                            }
                            else
                            {
                                AddBillboard(personTexturePath, personPos, new Vector3(6.0f, 12.0f, -1.5f));
                            }
                            personIndex++;
                        }
                        else
                        {
                            AddBillboard(personTexturePath, personPos, new Vector3(6.0f, 12.0f, -1.5f));
                        }
                    }
                }
            }
        }

        public void Render(Device device, Camera camera)
        {
            if (device == null || camera == null) return;

            // Render Sky Background first
            if (sky != null)
            {
                device.Render(sky, camera);
            }

            // Render Floor
            if (floor != null)
            {
                device.Render(floor, camera);
            }

            // Render Billboards
            if (billboards != null)
            {
                foreach (var billboard in billboards)
                {
                    device.Render(billboard, camera);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is World3D d &&
                   EqualityComparer<Mesh>.Default.Equals(Floor, d.Floor) &&
                   EqualityComparer<List<Billboard>>.Default.Equals(Billboards, d.Billboards);
        }

        public override int GetHashCode()
        {
            int hashCode = -1415363622;
            hashCode = hashCode * -1521134295 + EqualityComparer<Mesh>.Default.GetHashCode(Floor);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Billboard>>.Default.GetHashCode(Billboards);
            return hashCode;
        }
    }
}