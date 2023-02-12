using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Explosion2 : Sprite
    {
        private Rectangle dist;
        private int frames = 0;
        public bool idle;
        int cntWidth = 8, cntHeight = 6;

        public Explosion2(Vector2 pos, int width, int height)
        {
            SoundEffect snd = Globals.con.Load<SoundEffect>("sounds/long-explosion_edited");
            snd.Play(Globals.soundVol , 0f , 0f);

            texture = Globals.con.Load<Texture2D>("long_explosion");
            this.width = width;
            this.height = height;
            this.pos = pos;
            Build();
            dist = new Rectangle(0, 0, 0, 0);
            org = new Vector2((texture.Width / cntWidth) / 2, (texture.Height / cntHeight) / 2);
            idle = false;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            if (idle)
            {
                dist = new Rectangle(130 * 4, 130 * 4, 130, 130);
                frames = 0;
                sprites.RemoveAt(idx);
                idx--;
                return;
            }

            frames++;

            if (frames >= 180)
            {
                idle = true;
                return;
            }
            if (frames%4 == 0)
            {
                int cnt = (frames - 0) / 4;
                int xx = cnt % cntWidth, yy = cnt / cntWidth;
                dist = new Rectangle(xx * 256, yy * 256, 256, 256);
            }

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol)
        {
            rec = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            spriteBatch.Draw(texture, rec, dist, Color.White, 0f, org, SpriteEffects.None, 0f);

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
