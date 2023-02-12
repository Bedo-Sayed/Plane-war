using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project1
{
    public class Explosion : Sprite
    {
        private Rectangle dist;
        private int frames = 0;
        public bool idle;

        public Explosion(SoundEffect exp , Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt,
            int cntWidth , int cntHeight)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            exp.Play(Globals.soundVol , 0 , 0);

            dist = new Rectangle(0, 0, texture.Width/cntWidth, texture.Height/cntHeight);
            org = new Vector2( (texture.Width / cntWidth) / 2, (texture.Height / cntHeight)/2 );
            idle = false;
        }

        public override void Update(GameTime gameTime , List<Sprite> sprites ,List<Bullet> bullets , ref int idx)
        {
            if (idle)
            {
                dist = new Rectangle(130*4, 130*4, 130, 130);
                frames = 0;
                sprites.RemoveAt(idx);
                idx--;
                return;
            }

            frames++;
            if (frames >= 75)
            {
                idle = true;
                return;
            }

            if (frames%3 == 0)
            {
                int cnt = frames / 3;
                int xx = cnt % 5, yy = cnt / 5;
                dist = new Rectangle(xx * 130, yy * 130, 130, 130);
            }

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch , bool drawPol)
        {
            Rectangle rec = new Rectangle((int)pos.X , (int)pos.Y , width, height);
            spriteBatch.Draw(texture, rec , dist , Color.White , 0f , org , SpriteEffects.None , 0f);

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
