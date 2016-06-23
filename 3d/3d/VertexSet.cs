using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Graphics;

namespace _3d
{
    class Primitive
    {

        public VertexBuffer VertexBuffer { get; private set; }
        public IndexBuffer IndexBuffer { get; private set; }

        public void Init(GraphicsDevice gfx)
        {
            var pts = GetPoints();
            VertexBuffer = new VertexBuffer(gfx, typeof(VertexPositionColor), pts.Length, BufferUsage.None);
            VertexBuffer.SetData(pts);

            var indicies = GetIndicies();
            IndexBuffer = new IndexBuffer(gfx, typeof (short), indicies.Length, BufferUsage.None);
            IndexBuffer.SetData(indicies);
        }

        protected virtual VertexPositionColor[] GetPoints()
        {
            // fill in points. 
            return new VertexPositionColor[]{};
        }

        protected virtual short[] GetIndicies()
        {
            return new short[]{};
        }

        public void Draw(GraphicsDevice gfx)
        {
            gfx.SetVertexBuffer(VertexBuffer);
            gfx.Indices = IndexBuffer;
            gfx.RasterizerState = RasterizerState.CullNone;
            gfx.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, VertexBuffer.VertexCount, 0, IndexBuffer.IndexCount / 3);

        }

    }

    class CubeVertexSet : Primitive
    {


        protected override VertexPositionColor[] GetPoints()
        {
            return new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(0, 0, 0), Color.Red), 
                new VertexPositionColor(new Vector3(1, 0, 0), Color.Red), 
                new VertexPositionColor(new Vector3(1, 1, 0), Color.Green), 
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Green), 
                new VertexPositionColor(new Vector3(0, 0, 1), Color.Blue), 
                new VertexPositionColor(new Vector3(1, 0, 1), Color.Blue), 
                new VertexPositionColor(new Vector3(1, 1, 1), Color.White), 
                new VertexPositionColor(new Vector3(0, 1, 1), Color.White), 
            }.ToList().Select(v => new VertexPositionColor(v.Position -= new Vector3(.5f), v.Color)).ToArray();
        }

        protected override short[] GetIndicies()
        {
            return new short[]
            {
                0, 1, 2,
                0, 2, 3,

                0, 1, 5,
                0, 5, 4,

                0, 4, 7,
                0, 7, 3,

                1, 5, 6,
                1, 6, 2,

                5, 6, 4,
                4, 6, 7,

                2, 6, 7,
                2, 3, 7,
            };
        }
    }
}
