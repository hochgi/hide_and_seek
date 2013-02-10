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
    //i moved this enum to outside the class because I wanted to use it somewhere else (tamar)
    public enum Direction { Forward, Backward, Left, Right, LF, LB, RF, RB, Up, Down };

    public class MyDrawable : DrawableGameComponent
    {

        public MyDrawable(Game game, Color color, Vector3 shapePosition, Vector3 shapeSize) : base(game) 
        {
            Game.Components.Add(this);
            this.color = color;
            this.shapePosition = shapePosition;
            this.shapeSize = shapeSize;
        }

        //BasicEffect m_effect;//not sure!
        public VertexPositionColor[] m_vertices;
        public Color color;
        Vector3 shapePosition;
        Vector3 shapeSize;

        public int m_startIdx;
        //{
        //    get { return m_startIdx; }
        //    set
        //    {
        //        if (value < 0 || value >= m_vertices.Length)
        //            throw new Exception("index out of bound");
        //        m_startIdx = value;
        //    }
        //}
        public int m_numOfTriangles;
        //{
        //    get { return m_numOfTriangles; }
        //    set
        //    {
        //        if (value < 1 || value > m_vertices.Length - 2)
        //        {
        //            Console.WriteLine("numOFTriangles Error!");
        //            throw new Exception("wrong number of triangles");
        //        }
        //        m_numOfTriangles = value;
        //    }
        //}

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

        public override void Initialize()
        {
            m_vertices = new VertexPositionColor[36];
            m_numOfTriangles = 12;
            m_startIdx = 0;

            Vector3 topLeftFront = shapePosition +
                new Vector3(-1.0f, 1.0f, -1.0f) * shapeSize;
            Vector3 bottomLeftFront = shapePosition +
                new Vector3(-1.0f, -1.0f, -1.0f) * shapeSize;
            Vector3 topRightFront = shapePosition +
                new Vector3(1.0f, 1.0f, -1.0f) * shapeSize;
            Vector3 bottomRightFront = shapePosition +
                new Vector3(1.0f, -1.0f, -1.0f) * shapeSize;
            Vector3 topLeftBack = shapePosition +
                new Vector3(-1.0f, 1.0f, 1.0f) * shapeSize;
            Vector3 topRightBack = shapePosition +
                new Vector3(1.0f, 1.0f, 1.0f) * shapeSize;
            Vector3 bottomLeftBack = shapePosition +
                new Vector3(-1.0f, -1.0f, 1.0f) * shapeSize;
            Vector3 bottomRightBack = shapePosition +
                new Vector3(1.0f, -1.0f, 1.0f) * shapeSize;

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f) * shapeSize;
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f) * shapeSize;
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f) * shapeSize;
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f) * shapeSize;
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f) * shapeSize;
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f) * shapeSize;

            Vector2 textureTopLeft = new Vector2(0.5f * shapeSize.X, 0.0f * shapeSize.Y);
            Vector2 textureTopRight = new Vector2(0.0f * shapeSize.X, 0.0f * shapeSize.Y);
            Vector2 textureBottomLeft = new Vector2(0.5f * shapeSize.X, 0.5f * shapeSize.Y);
            Vector2 textureBottomRight = new Vector2(0.0f * shapeSize.X, 0.5f * shapeSize.Y);

            // Front face.
            m_vertices[0] = new VertexPositionColor(topLeftFront, color);
            m_vertices[1] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[2] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[3] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[4] = new VertexPositionColor(
                bottomRightFront, color);
            m_vertices[5] = new VertexPositionColor(
                topRightFront, color);

            // Back face.
            m_vertices[6] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[7] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[8] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[9] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[10] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[11] = new VertexPositionColor(
                bottomRightBack, color);

            // Top face.
            m_vertices[12] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[13] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[14] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[15] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[16] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[17] = new VertexPositionColor(
                topRightBack, color);

            // Bottom face. 
            m_vertices[18] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[19] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[20] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[21] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[22] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[23] = new VertexPositionColor(
                bottomRightFront, color);

            // Left face.
            m_vertices[24] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[25] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[26] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[27] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[28] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[29] = new VertexPositionColor(
                topLeftFront, color);

            // Right face. 
            m_vertices[30] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[31] = new VertexPositionColor(
                bottomRightFront, color);
            m_vertices[32] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[33] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[34] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[35] = new VertexPositionColor(
                bottomRightBack, color);

            base.Initialize();
        }

        public void updateLocation(Vector3 location)
        {
            shapePosition = location;

            Vector3 topLeftFront = shapePosition +
                new Vector3(-1.0f, 1.0f, -1.0f) * shapeSize;
            Vector3 bottomLeftFront = shapePosition +
                new Vector3(-1.0f, -1.0f, -1.0f) * shapeSize;
            Vector3 topRightFront = shapePosition +
                new Vector3(1.0f, 1.0f, -1.0f) * shapeSize;
            Vector3 bottomRightFront = shapePosition +
                new Vector3(1.0f, -1.0f, -1.0f) * shapeSize;
            Vector3 topLeftBack = shapePosition +
                new Vector3(-1.0f, 1.0f, 1.0f) * shapeSize;
            Vector3 topRightBack = shapePosition +
                new Vector3(1.0f, 1.0f, 1.0f) * shapeSize;
            Vector3 bottomLeftBack = shapePosition +
                new Vector3(-1.0f, -1.0f, 1.0f) * shapeSize;
            Vector3 bottomRightBack = shapePosition +
                new Vector3(1.0f, -1.0f, 1.0f) * shapeSize;

            Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f) * shapeSize;
            Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f) * shapeSize;
            Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f) * shapeSize;
            Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f) * shapeSize;
            Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f) * shapeSize;
            Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f) * shapeSize;

            Vector2 textureTopLeft = new Vector2(0.5f * shapeSize.X, 0.0f * shapeSize.Y);
            Vector2 textureTopRight = new Vector2(0.0f * shapeSize.X, 0.0f * shapeSize.Y);
            Vector2 textureBottomLeft = new Vector2(0.5f * shapeSize.X, 0.5f * shapeSize.Y);
            Vector2 textureBottomRight = new Vector2(0.0f * shapeSize.X, 0.5f * shapeSize.Y);

            // Front face.
            m_vertices[0] = new VertexPositionColor(topLeftFront, color);
            m_vertices[1] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[2] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[3] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[4] = new VertexPositionColor(
                bottomRightFront, color);
            m_vertices[5] = new VertexPositionColor(
                topRightFront, color);

            // Back face.
            m_vertices[6] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[7] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[8] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[9] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[10] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[11] = new VertexPositionColor(
                bottomRightBack, color);

            // Top face.
            m_vertices[12] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[13] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[14] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[15] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[16] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[17] = new VertexPositionColor(
                topRightBack, color);

            // Bottom face. 
            m_vertices[18] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[19] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[20] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[21] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[22] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[23] = new VertexPositionColor(
                bottomRightFront, color);

            // Left face.
            m_vertices[24] = new VertexPositionColor(
                topLeftFront, color);
            m_vertices[25] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[26] = new VertexPositionColor(
                bottomLeftFront, color);
            m_vertices[27] = new VertexPositionColor(
                topLeftBack, color);
            m_vertices[28] = new VertexPositionColor(
                bottomLeftBack, color);
            m_vertices[29] = new VertexPositionColor(
                topLeftFront, color);

            // Right face. 
            m_vertices[30] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[31] = new VertexPositionColor(
                bottomRightFront, color);
            m_vertices[32] = new VertexPositionColor(
                bottomRightBack, color);
            m_vertices[33] = new VertexPositionColor(
                topRightBack, color);
            m_vertices[34] = new VertexPositionColor(
                topRightFront, color);
            m_vertices[35] = new VertexPositionColor(
                bottomRightBack, color);
        }

        public override void Draw(GameTime gameTime)
        {
            BasicEffect m_effect = ((Game1)Game).m_effect;
            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                m_effect.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.TriangleList, m_vertices, m_startIdx, m_numOfTriangles);
            }
            base.Draw(gameTime);
        }

    }
}
