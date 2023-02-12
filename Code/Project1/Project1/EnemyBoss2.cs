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
    public class EnemyBoss2 : Enemy
    {
        public int rotDeg;
        public EnemyBoss2(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt, int hl)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            health = startingHealth = hl;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            frame = 0;
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
                Globals.backSnd.Stop();
                Globals.stateSnd = Globals.win;
                KillMe(sprites, bullets, ref idx , snd);
                for (int i = 0; i < sprites.Count; i++)
                {
                    if (sprites[i] is Enemy)
                        ((Enemy)sprites[i]).health = 0;
                }
                return;
            }
            else if (Collision(this , Mission2.flag) && Main.cnt == 0)
            {
                Main.cnt = 600;
                Globals.backSnd.Stop();
                Globals.stateSnd = Globals.lose;
                Mission2.c.health = 0;
                return;
            }

            FixAngle();

            if (frame%2400 == 0)
            {
                vel = 1;
                idle = true;
            }
            else if (frame%1200 == 0)
            {
                vel = 0;
                idle = false;
            }

            if (idle)
            {
                if (frame % 2400 == 0)
                {
                    float ang = DiffAngle(this, Mission2.flag);
                    rotVel = ang / 60f;
                }
                else if (frame % 2400 == 60) rotVel = 0;
            }
            else
            {
                if (frame % 100 == 0)
                {
                    Player player = GetPlayer(sprites);
                    if (player != null)
                    {
                        float ang = DiffAngle(this, player);
                        rotVel = ang / 60f;
                    }
                }
                else if (frame % 100 == 60)
                {
                    rotVel = 0;
                }
                else
                {
                    if (Globals.GetRand(100000) % 120 == 0)
                    {
                        string name = "7", sound = "great_gun_shot";
                        GenerateBullet(name, sound, bullets, 150, 50 , 10 , 1.5f);
                    }
                }
            }

            rotRad += rotVel;
            RotatePoly(rotVel , false);
            pos = ChangePos(1);
            UpdatePoly(1);

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol = false)
        {
            Rectangle rec2 = new Rectangle(0, 0, texture.Width, texture.Height); //mloosh lazmla
            spriteBatch.Draw(texture, rec, rec2, Color.White, rotRad, org, SpriteEffects.None, 0f);

            if (drawPol == true)
            {
                for (int i = 0; i < pol.Count; i++)
                {
                    Rectangle recc = new Rectangle((int)pol[i].X, (int)pol[i].Y, 5, 5);
                    spriteBatch.Draw(Globals.red , recc, Color.White);
                }
            }
        }
    }
}