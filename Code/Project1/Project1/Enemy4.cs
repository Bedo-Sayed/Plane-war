using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Enemy4 : Enemy
    {
        public Enemy4(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            health = startingHealth = 1;
            vel = 0;

        }
        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (lifeTime <= 0 || health <= 0)
            {
                Globals.enemyCnt--;
                Mission4.killedEnem++;
                KillMe(sprites, bullets, ref idx, Globals.exp);
                return;
            }

            int rr = Globals.GetRand(100000);
            if (rr % 120 == 0)
            {
                GenerateBullet("bullet2", "lunch_rocket", bullets, 100, 30, 1, 0);
            }

            FixAngle();
            if (idle == true)
            {
                vel = 3;
                int r = Globals.rnd.Next(4);
                if (r <= 1)
                {
                    Player player = GetPlayer(sprites);
                    if (player != null)
                    {
                        float ang = DiffAngle(this, player);
                        rotVel = ang / 60f;
                        idle = false;
                        frame = 60;
                    }
                }
                else
                {
                    r = Globals.GetRand(200) - 100;
                    float ang = MathHelper.ToRadians(r);
                    rotVel = ang / 60f;
                    idle = false;
                    frame = 60;
                }
            }
            else
            {
                frame--;
                rotRad += rotVel;
                RotatePoly(rotVel, false);

                if (frame == 0)
                    idle = true;
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Friend)
                {
                    if (Collision(this, sprites[i]))
                    {
                        Globals.enemyCnt--;
                        KillMe(sprites, bullets, ref idx, Globals.exp);
                        return;
                    }

                    Vector2 tmp = ChangePos(1);
                    if (Globals.DsSq(tmp, sprites[i].pos) < 20000)
                    {
                        if (Globals.DsSq(pos, sprites[i].pos) < Globals.DsSq(tmp, sprites[i].pos)) { }
                        else
                        {
                            idle = false;
                            frame = 60;
                            vel = 0;
                            float ang = DiffAngle(this, (Plane)sprites[i]);
                            rotVel = ang / 60f;
                        }
                    }
                }
            }

            pos = ChangePos(1);
            UpdatePoly(1);
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol)
        {
            spriteBatch.Draw(texture, rec, null, color, rotRad, org, SpriteEffects.None, 0f);

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
