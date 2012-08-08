using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HideAndSeek
{
    class MyDrawable : DrawableGameComponent
    {
        public enum Direction { Forward, Backward, Left, Right, LF, LB, RF, RB, Up, Down };

        BasicEffect m_effect;
        public VertexPositionColor[] m_vertices;
        public int m_startIdx
        {
            get { return m_startIdx; }
            set
            {
                if (value < 0 || value >= m_vertices.Length)
                    throw new Exception("index out of bound");
                m_startIdx = value;
            }
        }
        public int m_numOfTriangles
        {
            get { return m_numOfTriangles; }
            set
            {
                if (value < 1 || value > m_vertices.Length - 2)
                    throw new Exception("wrong number of triangles");
                m_numOfTriangles = value;
            }
        }

        public void move(Direction dir)
        {
            Vector3 movement = new Vector3(0, 0, 0);
            switch ((Direction)dir)
            {
                case Direction.Forward:
                    movement.Z = -1;
                    break;
                case Direction.Backward:
                    movement.Z = +1;
                    break;
                case Direction.Left:
                    movement.X = -1;
                    break;
                case Direction.Right:
                    movement.X = +1;
                    break;
                case Direction.LF:
                    movement.Z = -1;
                    movement.X = -1;
                    break;
                case Direction.LB:
                    movement.Z = +1;
                    movement.X = -1;
                    break;
                case Direction.RF:
                    movement.Z = -1;
                    movement.X = +1;
                    break;
                case Direction.RB:
                    movement.Z = +1;
                    movement.X = +1;
                    break;
                case Direction.Up:
                    movement.Y = +1;
                    break;
                case Direction.Down:
                    movement.Y = -1;
                    break;
            }

            for (int i = 0; i < m_vertices.Length; ++i)
            {
                m_vertices[i].Position += movement;
            }

        }

        public override void Draw(GameTime gameTime)
        {
            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                m_effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleStrip, m_vertices, m_startIdx, m_numOfTriangles);
            }
            base.Draw(gameTime);
        }

    }
}
