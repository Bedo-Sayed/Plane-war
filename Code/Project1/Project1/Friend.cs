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
    public class Friend : Plane
    {
        public Friend(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt, int hl)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            health = startingHealth = hl;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            idle = true;
        }

        protected Enemy GetRandEnemy(List<Sprite> sprites)
        {
            int cnt = 0;
            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Enemy)
                    cnt++;
            }

            if (cnt == 0)
                return null;

            int r = Globals.GetRand(cnt);

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Enemy)
                {
                    if (r == 0)
                    {
                        return (Enemy)sprites[i];
                    }
                    else r--;
                }
            }
            return null;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            frame++;
            if (health <= 0 && Main.cnt == 0)
            {
                Main.cnt = 600;
                Globals.stateSnd = Globals.lose;
                SoundEffect snd = Globals.con.Load<SoundEffect>("sounds/great_explosion");
                Mission1.c.health = 0;
                Globals.backSnd.Stop();
                KillMe(sprites , bullets , ref idx, snd);
                return;
            }
            else if (Collision(this , Mission1.earth) && Main.cnt == 0)
            {
                Main.cnt = 600;
                Globals.stateSnd = Globals.win;
                Globals.backSnd.Stop();
                for (int i=0; i<sprites.Count; i++)
                {
                    if (sprites[i] is Enemy)
                        ((Enemy)sprites[i]).health = 0;
                }
                return;
            }

            FixAngle();

            if (frame % 2400 == 0)
            {
                vel = 1;
                idle = true;
            }
            else if (frame % 2400 == 1200)
            {
                vel = 0;
                idle = false;
            }

            if (idle)
            {
                if (frame % 2400 == 0)
                {
                    float ang = DiffAngle(this, Mission1.earth);
                    rotVel = ang / 60f;
                }
                else if (frame % 2400 == 60) rotVel = 0;
            }
            else
            {
                if (frame % 100 == 0)
                {
                    Enemy enem = GetRandEnemy(sprites);
                    if (enem != null)
                    {
                        float ang = DiffAngle(this, enem);
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
                        string name = "6", sound = "great_gun_shot";
                        GenerateBullet(name, sound, bullets, 150, 50, 10, 1.5f);
                    }
                }

            }

            rotRad += rotVel;

            RotatePoly(rotVel, false);
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
                    spriteBatch.Draw(Globals.red, recc, Color.White);
                }
            }
        }
    }
}