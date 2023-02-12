using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Threading;

namespace Project1
{
    public class HealthBar : Sprite
    {
        Plane parent;

        public HealthBar(Plane parent) 
        {
            this.parent = parent;
            texture = Globals.red;

            texture = new Texture2D( Globals.device , 1, 1);

            color = Color.Green;

            texture.SetData<Color>(new Color[] { color });

            width = (int)(0.8f * parent.width);
            height = 10;
            pos = parent.pol[0] + new Vector2(0 , -15);
            rotRad = parent.rotRad;

            org = new Vector2(0 , 0);

            base.Build();
        }

        public override void Update(GameTime gameTime , List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            float perp = parent.rotRad + 1.57f;
            var dir = new Vector2((float)Math.Cos(perp), (float)Math.Sin(perp)) * 15;
            pos = parent.pol[0] - dir;
            rotRad = parent.rotRad;
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
            float rat = (float)parent.health / (float)parent.startingHealth;

            rec.Width = (int) (width * rat);

            if (rat < 0.65f && rat >= 0.30f)
                color = Color.Yellow;
            else if (rat < 0.30f)
                color = Color.Red;

            texture.SetData<Color>(new Color[] { color });
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol = false)
        {
            spriteBatch.Draw(texture, rec, null, Color.White, rotRad, org, SpriteEffects.None, 0f);

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
