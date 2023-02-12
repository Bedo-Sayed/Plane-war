using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Net.Security;
//using System.Numerics;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Enemy1 : Enemy
    {
        public Enemy1(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf, float ofRt)
            : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            vel = 3;
            frame = 0;
            health = 1;
            lifeTime = 300;
            idle = true;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (lifeTime <= 0 || health <= 0)
            {
                Globals.enemyCnt--;
                KillMe(sprites, bullets, ref idx, Globals.exp);
                return;
            }

            FixAngle();

            int rr = Globals.GetRand(100000);
            if (rr % 120 == 0)
            {
                GenerateBullet("8", "lunch_rocket", bullets, 80, 60, 1, 0);
            }

            if (idle == true)
            {
                vel = 3;
                int r = Globals.rnd.Next(6);
                if (r <= 1)
                {
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        if (sprites[i] is Friend && !(sprites[i] is Player) )
                        {
                            float ang = DiffAngle(this, (Plane)sprites[i]);

                            rotVel = ang / 60f;
                            idle = false;
                            frame = 60;
                        }
                    }
                }
                else if (r <= 3)
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
                    if (Collision(this , (Plane)sprites[i]))
                    {
                        Globals.enemyCnt--;
                        KillMe(sprites, bullets, ref idx, Globals.exp);
                        return;
                    }

                    Vector2 tmp = ChangePos(1);
                    if (sprites[i] is Player && Globals.DsSq(tmp, sprites[i].pos) < 20000)
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
                    else if (sprites[i] is Friend && sprites[i] is not Player && Globals.DsSq(tmp, sprites[i].pos) < 40000)
                    {
                        if (Globals.DsSq(pos, sprites[i].pos) < Globals.DsSq(tmp, sprites[i].pos)) { }
                        else
                        {
                            idle = false;
                            frame = 60;
                            vel = -vel;
                            int r = Globals.GetRand(200) - 100;
                            float ang = MathHelper.ToRadians(r);
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
