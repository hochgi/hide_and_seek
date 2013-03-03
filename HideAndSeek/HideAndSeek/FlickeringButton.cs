using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HideAndSeek
{
    class FlickeringButton
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Color alphaDisolver = new Color(255, 255, 255, 255);
        public Vector2 size;
        public FlickeringButton(Texture2D newTexture, GraphicsDevice graphics, int placeInLine)
        {
            int i = placeInLine - 2;
            texture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 4, graphics.Viewport.Height / 15);
            setPosition(new Vector2((graphics.Viewport.Width / 2) - (graphics.Viewport.Width / 8),
                                    (graphics.Viewport.Height / 2) + (i * (graphics.Viewport.Height / 15))));
        }
        bool down;
        public bool isClicked;
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectangle.Intersects(rectangle))
            {
                if (alphaDisolver.A == 255) down = false;
                if (alphaDisolver.A == 0) down = true;
                if (down) alphaDisolver.A += 3; else alphaDisolver.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if (alphaDisolver.A < 255)
            {
                alphaDisolver.A += 3;
                isClicked = false;
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rectangle, alphaDisolver);
        }
    }
}
