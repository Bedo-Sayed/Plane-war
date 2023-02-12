using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Xna.Framework.Audio;

namespace Project1
{
    public class EnemyBoss3 : Enemy
    {
        public EnemyBoss3(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt, int hl)
         : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            health = startingHealth = hl;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            frame = 0;
            vel = 0;
            idle = true;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            frame++;
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if ( (lifeTime <= 0 || health <= 0) && Main.cnt == 0)
            {
                SoundEffect snd = Globals.con.Load<SoundEffect>("sounds/great_explosion");
                Globals.enemyCnt--;
                Main.cnt = 600;
                Globals.stateSnd = Globals.win;
                Globals.backSnd.Stop();
                KillMe(sprites, bullets, ref idx, snd);
                for (int i = 0; i < sprites.Count; i++)
                {
                    if (sprites[i] is Enemy)
                        ((Enemy)sprites[i]).health = 0;
                }
                return;
            }
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
