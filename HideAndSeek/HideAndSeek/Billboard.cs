using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HideAndSeek
{
    interface Billboard
    {
        /**
         * returns the 4 vertices making the billboard.
         * the order is important, starting from upper
         * left corner, and moving on in clockwise order.
         */
        Vector3[] getQuadBillboard(Vector3 up, Vector3 position);
        Vector3 getPosition();
        Texture2D getTexture();
    }

    class BillboardSystem
    {
        /**
         * adds a new Billboard to the BillboardSystem.
         * return true on success, or false if maximum
         * billboards amount have already been reached.
         */
        public bool addBillboard(Billboard bb)
        {
            if (maxIndex == billboards.Length)
            {
                return false;
            }
            else
            {
                billboards[maxIndex] = bb;
                maxIndex++;
                return true;
            }
        }

        public void drawBillboards(Vector3 cameraLocation, Effect effect, Vector3 upVector)
        {
            sortBillboards(billboards, cameraLocation);

            Vector3 up;
            if(upVector == null)
            {
                up = defaultUp;
            }
            else
            {
                up = upVector;
            }

            for (int i = 0; i < maxIndex; i++)
            {
                Billboard bb = billboards[i];
                Texture2D t = bb.getTexture();
                Vector3[] q = bb.getQuadBillboard(up, cameraLocation);
                Vector3 normal = Vector3.Subtract(cameraLocation, bb.getPosition());

                //effect.Parameters["ParticleTexture"].SetValue(t);
                //effect.Parameters["View"].SetValue(view);
                //effect.Parameters["Projection"].SetValue(projection);
                //effect.CurrentTechnique.Passes[0].Apply();

                vertices[0].Position = q[0];
                vertices[1].Position = q[1];
                vertices[2].Position = q[2];
                vertices[3].Position = q[3];

                vertices[0].Normal = normal;
                vertices[1].Normal = normal;
                vertices[2].Normal = normal;
                vertices[3].Normal = normal;
            }

            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, 4, indexes, 0, 2);
        }

        //private section:

        private int maxIndex = 0;
        private GraphicsDevice graphicsDevice;
        private Billboard[] billboards;
        private Effect effect;
        private Vector3 defaultUp = new Vector3(0f, 1f, 0f);
        private VertexPositionNormalTexture[] vertices;
        private int[] indexes;

        private BillboardSystem(int numberOfBillboards, GraphicsDevice graphicsDevice, Effect effect)
        {
            billboards = new Billboard[numberOfBillboards];
            this.graphicsDevice = graphicsDevice;
            this.effect = effect;
            vertices = new VertexPositionNormalTexture[4];
            vertices[0] = new VertexPositionNormalTexture();
            vertices[1] = new VertexPositionNormalTexture();
            vertices[2] = new VertexPositionNormalTexture();
            vertices[3] = new VertexPositionNormalTexture();
            vertices[0].TextureCoordinate = new Vector2(0f, 0f);
            vertices[1].TextureCoordinate = new Vector2(1f, 0f);
            vertices[2].TextureCoordinate = new Vector2(1f, 1f);
            vertices[3].TextureCoordinate = new Vector2(0f, 1f);

            indexes = new int[6];
            indexes[0] = 0;
            indexes[0] = 1;
            indexes[0] = 2;
            indexes[0] = 2;
            indexes[0] = 1;
            indexes[0] = 3;
        }

        //static section:

        private static BillboardSystem bbs = null;

        public static BillboardSystem factory(int maxNumberOfBillboards, GraphicsDevice graphicsDevice, Effect effect)
        {
            if (bbs != null)
            {
                throw new AlreadyInitializedException("cannot create more than one singleton Billboard system.");
            }

            Effect l_effect;
            if (effect == null)
            {
                l_effect = new BasicEffect(graphicsDevice);
            }
            else
            {
                l_effect = effect;
            }

            bbs = new BillboardSystem(maxNumberOfBillboards, graphicsDevice, l_effect);
            return bbs;
        }

        public static BillboardSystem getBillboardSystem()
        {
            if (bbs != null)
            {
                return bbs;
            }

            else throw new NotInitializedException("use \'Billboard.factory(numOfBillboards);\' before trying to retrieve it.");
        }

        private static Billboard[] sortBillboards(Billboard[] billboards, Vector3 cameraLocation)
        {
            Array.Sort<Billboard>(billboards, new Comparison<Billboard>(
                (Billboard a, Billboard b) => 
                {
                    if(a == b == null) return 0;
                    else if(a == null) return 1;
                    else if(b == null) return -1;
                    else return (int)(Vector3.DistanceSquared(cameraLocation, a.getPosition()) - Vector3.DistanceSquared(cameraLocation, b.getPosition()));
                })
            );

            return billboards;
        }
    }
}
