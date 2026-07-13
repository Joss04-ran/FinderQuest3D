using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace FinderQuest3D
{
    // Class for parent on every area, like WalkArea or TalkArea
    public abstract class Map
    {
        // 0 : Grass, 1 : Road, 2 : Trees, 3 : Player, 5 : Fence 
        // 6 : Persons, 7 : Air (to move into next map)
        private int[,] grid;
        // Composition : Map has Mesh. Map cannot exist without Mesh
        private Mesh mesh;
        private float tileSize;
        private int width;
        private int height;

        protected Map()
        {
            this.TileSize = 16.0f;
        }

        public Map(int[,] grid)
        {
            AddMap(grid);
        }

        public void AddMap(int[,] grid)
        {
            this.Grid = grid;
            this.Mesh = BuildMapMesh();
        }


        public int Width => grid.GetLength(1);
        public int Height => grid.GetLength(0);
        public float TileSize { get => tileSize; set => tileSize = value; }

        public int[,] Grid { get => grid; private set => grid = value; }
        public Mesh Mesh { get => mesh; private set => mesh = value; }

        public override bool Equals(object obj)
        {
            return obj is Map map &&
                   EqualityComparer<int[,]>.Default.Equals(Grid, map.Grid) &&
                   EqualityComparer<Mesh>.Default.Equals(Mesh, map.Mesh);
        }

        public override int GetHashCode()
        {
            int hashCode = -526703371;
            hashCode = hashCode * -1521134295 + EqualityComparer<int[,]>.Default.GetHashCode(Grid);
            hashCode = hashCode * -1521134295 + EqualityComparer<Mesh>.Default.GetHashCode(Mesh);
            return hashCode;
        }

        public int GetCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return -1;
            return grid[y, x];
        }

        private bool IsWalkable(int x, int z)
        {
            int cell = GetCell(x, z);
            // 0: Grass, 1: Road, 3: Player, 6: Person, 7: Air are walkable.
            return cell == 0 || cell == 1 || cell == 3 || cell == 6 || cell == 7;
        }

        public bool IsTransitionArea(Vector3 position)
        {
            Vector3 cellPos = position / tileSize;
            int x = (int)Math.Floor(cellPos.X);
            int z = (int)Math.Floor(cellPos.Z);
            return GetCell(x, z) == 7;
        }

        public Vector3 GetPlayerSpawnPosition()
        {
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    if (GetCell(c, r) == 3)
                    {
                        return new Vector3(c * tileSize + tileSize / 2, 4.0f, r * tileSize + tileSize / 2);
                    }
                }
            }
            return new Vector3(150.0f, 4.0f, 150.0f); // Default if not found
        }

        // Method for procedurally generated maps 
        // I use virtual for used in WalkArea or TalkArea
        protected virtual Mesh BuildMapMesh()
        {
            Texture atlasTexture = CreateTextureAtlas();
            List<Vertex> vertices = new List<Vertex>();
            List<Faces> faces = new List<Faces>();

            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    int cell = GetCell(c, r);
                    float xStart = c * tileSize;
                    float zStart = r * tileSize;

                    // By default, render a ground quad.
                    // If it is road, render road texture. Otherwise, grass.
                    bool isRoad = false;
                    if (cell == 1 || cell == 3)
                        isRoad = true;
                    Vector2 uvMin;
                    if (isRoad)
                    {
                        uvMin = new Vector2(0.25f, 0.0f);
                    }
                    else
                    {
                        uvMin = new Vector2(0.0f, 0.0f);
                    }
                    Vector2 uvMax;
                    if (isRoad)
                    {
                        uvMax = new Vector2(0.5f, 1.0f);
                    }
                    else
                    {
                        uvMax = new Vector2(0.25f, 1.0f);
                    }

                    AddQuad(vertices, faces,
                        new Vector3(xStart, 0.0f, zStart),
                        new Vector3(xStart + tileSize, 0.0f, zStart),
                        new Vector3(xStart + tileSize, 0.0f, zStart + tileSize),
                        new Vector3(xStart, 0.0f, zStart + tileSize),
                        new Vector2(uvMin.X, uvMax.Y),
                        new Vector2(uvMax.X, uvMax.Y),
                        new Vector2(uvMax.X, uvMin.Y),
                        new Vector2(uvMin.X, uvMin.Y),
                        new Vector3(0, 1, 0));

                    if (cell == 5)
                    {
                        float height = 8.0f; 
                        Vector2 fenceUvMin = new Vector2(0.5f, 0.0f);
                        Vector2 fenceUvMax = new Vector2(0.75f, 1.0f);

                        // Check neighbors to determine orientation
                        bool connectZ = GetCell(c, r - 1) == 5 || GetCell(c, r + 1) == 5;
                        bool connectX = GetCell(c - 1, r) == 5 || GetCell(c + 1, r) == 5;

                        if (connectZ && !connectX)
                        {
                            // Z-aligned
                            AddQuad(vertices, faces,
                                new Vector3(xStart + tileSize / 2, 0.0f, zStart),
                                new Vector3(xStart + tileSize / 2, height, zStart),
                                new Vector3(xStart + tileSize / 2, height, zStart + tileSize),
                                new Vector3(xStart + tileSize / 2, 0.0f, zStart + tileSize),
                                new Vector2(fenceUvMin.X, fenceUvMax.Y),
                                new Vector2(fenceUvMin.X, fenceUvMin.Y),
                                new Vector2(fenceUvMax.X, fenceUvMin.Y),
                                new Vector2(fenceUvMax.X, fenceUvMax.Y),
                                new Vector3(1, 0, 0));
                        }
                        else
                        {
                            // X-aligned
                            AddQuad(vertices, faces,
                                new Vector3(xStart, 0.0f, zStart + tileSize / 2),
                                new Vector3(xStart, height, zStart + tileSize / 2),
                                new Vector3(xStart + tileSize, height, zStart + tileSize / 2),
                                new Vector3(xStart + tileSize, 0.0f, zStart + tileSize / 2),
                                new Vector2(fenceUvMin.X, fenceUvMax.Y),
                                new Vector2(fenceUvMin.X, fenceUvMin.Y),
                                new Vector2(fenceUvMax.X, fenceUvMin.Y),
                                new Vector2(fenceUvMax.X, fenceUvMax.Y),
                                new Vector3(0, 0, 1));
                        }
                    }
                }
            }

            Mesh resultMesh = new Mesh("ProceduralMap", vertices.Count, faces.Count);
            for (int i = 0; i < vertices.Count; i++)
            {
                resultMesh.ArrayVertices[i] = vertices[i];
            }
            for (int i = 0; i < faces.Count; i++)
            {
                resultMesh.ArrayFaces[i] = faces[i];
            }
            resultMesh.Texture = atlasTexture;
            return resultMesh;
        }

        protected void AddQuad(List<Vertex> vertices, List<Faces> faces,
            Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3,
            Vector2 uv0, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector3 normal)
        {
            int baseIdx = vertices.Count;
            vertices.Add(new Vertex { Coordinates = p0, TextureCoordinates = uv0, Normal = normal });
            vertices.Add(new Vertex { Coordinates = p1, TextureCoordinates = uv1, Normal = normal });
            vertices.Add(new Vertex { Coordinates = p2, TextureCoordinates = uv2, Normal = normal });
            vertices.Add(new Vertex { Coordinates = p3, TextureCoordinates = uv3, Normal = normal });

            faces.Add(new Faces { A = baseIdx + 0, B = baseIdx + 2, C = baseIdx + 1 });
            faces.Add(new Faces { A = baseIdx + 0, B = baseIdx + 3, C = baseIdx + 2 });
        }

        protected Texture CreateTextureAtlas()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(baseDir, "..", ".."));

            string grassPath = Path.Combine(projectPath, "Resources", "Tileset", "Grass.png");
            string roadPath = Path.Combine(projectPath, "Resources", "Tileset", "Gravel.png");
            string fencePath = Path.Combine(projectPath, "Resources", "Tileset", "Fence.png");
            string treePath = Path.Combine(projectPath, "Resources", "Tileset", "Tree.png");

            // Fallback checking in debug folder
            if (!File.Exists(grassPath))
            {
                grassPath = Path.Combine(baseDir, "Resources", "Tileset", "Grass.png");
                roadPath = Path.Combine(baseDir, "Resources", "Tileset", "Gravel.png");
                fencePath = Path.Combine(baseDir, "Resources", "Tileset", "Fence.png");
                treePath = Path.Combine(baseDir, "Resources", "Tileset", "Tree.png");
            }

            int tileW = 16;
            int tileH = 16;

            using (var atlas = new Bitmap(tileW * 4, tileH))
            using (var g = Graphics.FromImage(atlas))
            {
                g.Clear(System.Drawing.Color.Transparent);

                void DrawTile(string path, int index, System.Drawing.Color fallbackColor)
                {
                    if (File.Exists(path))
                    {
                        using (var img = new Bitmap(path))
                        {
                            g.DrawImage(img, new System.Drawing.Rectangle(index * tileW, 0, tileW, tileH));
                        }
                    }
                    else
                    {
                        using (var brush = new SolidBrush(fallbackColor))
                        {
                            g.FillRectangle(brush, index * tileW, 0, tileW, tileH);
                        }
                    }
                }

                DrawTile(grassPath, 0, System.Drawing.Color.Green);
                DrawTile(roadPath, 1, System.Drawing.Color.Gray);
                DrawTile(fencePath, 2, System.Drawing.Color.SaddleBrown);
                DrawTile(treePath, 3, System.Drawing.Color.DarkGreen);

                var rect = new System.Drawing.Rectangle(0, 0, atlas.Width, atlas.Height);
                var bmpData = atlas.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                int bytes = atlas.Width * atlas.Height * 4;
                byte[] rgbValues = new byte[bytes];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, rgbValues, 0, bytes);
                atlas.UnlockBits(bmpData);

                return new Texture(rgbValues, atlas.Width, atlas.Height);
            }
        }

        public bool isCollision(Vector3 position, float radius)
        {
            float cellRadius = radius / tileSize;
            Vector3 cellPos = position / tileSize;

            int minX = (int)Math.Floor(cellPos.X - cellRadius);
            int maxX = (int)Math.Floor(cellPos.X + cellRadius);
            int minZ = (int)Math.Floor(cellPos.Z - cellRadius);
            int maxZ = (int)Math.Floor(cellPos.Z + cellRadius);

            for (int z = minZ; z <= maxZ; z++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (!IsWalkable(x, z))
                    {
                        float closestX = Math.Max(x, Math.Min(cellPos.X, x + 1.0f));
                        float closestZ = Math.Max(z, Math.Min(cellPos.Z, z + 1.0f));
                        float diffX = cellPos.X - closestX;
                        float diffZ = cellPos.Z - closestZ;
                        float distSq = (diffX * diffX) + (diffZ * diffZ);

                        if (distSq < cellRadius * cellRadius)
                        {
                            return true; 
                        }
                    }
                }
            }
            return false; 
        }
    }
}