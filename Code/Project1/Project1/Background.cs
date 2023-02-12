using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Background : Sprite
    {
        public Rectangle recFromPic;
        public int stX, stY , takeX , takeY;

        public Rectangle RecFromPic { get => recFromPic; set => recFromPic = value; }

        public Background(Texture2D texture) : base(texture)
        {
            opacity = 1;
            offUp = offDown = offLeft = offRight = stX = stY = takeX = takeY = 0;
            org = new Vector2(0, 0);
            recFromPic = new Rectangle(0, 0, texture.Width, texture.Height);
            color = Color.White;
        }

        public Background(Texture2D texture, Vector2 pos, int width, int height, float offUp, float offDown, float offLeft, float offRight) 
            : base(texture, pos, width, height, offUp, offDown, offLeft, offRight)
        {
            org = new Vector2(0, 0);
            color = Color.White;
        }

        public Background(Texture2D texture, Vector2 pos, int width, int height, float offUp, float offDown, float offLeft, float offRight,
            int stX , int stY)
            : base(texture, pos, width, height, offUp, offDown, offLeft, offRight)
        {
            org = new Vector2(0, 0);
            color = Color.White;
            recFromPic = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public override void Update(GameTime gameTime , List<Sprite> sprites , List<Bullet> bullets , ref int idx)
        {
            pos = ChangePos(1);
            UpdatePoly(1);
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch , bool drawPol)
        {
            rec.Width = width;
            rec.Height = height;
            spriteBatch.Draw(texture, rec, recFromPic, color * opacity, 0f, org, SpriteEffects.None, 0f);

            if (drawPol == true)
            {
                for (int i = 0; i < pol.Count; i++)
                {
                    Rectangle recc = new Rectangle((int)pol[i].X, (int)pol[i].Y, 5, 5);
                    spriteBatch.Draw(Globals.red, recc, Color.White);
                }
            }
        }
    }
}
