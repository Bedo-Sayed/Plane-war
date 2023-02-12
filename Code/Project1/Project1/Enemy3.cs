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
    public class Enemy3 : Enemy
    {
        Friend target;
        int cnt , rotateCnt;
        public Enemy3(Texture2D texture, Vector2 pos, int width, int height, float ofUp, float ofDw, float ofLf,
            float ofRt, int hl) : base(texture, pos, width, height, ofUp, ofDw, ofLf, ofRt)
        {
            frame = rotateCnt = 0;
            cnt = 300;
            health = 1;
            target = null;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        {
            if (health <= 0)
            {
                SoundEffect snd = Globals.con.Load<SoundEffect>("sounds/explosion_sound");
                Globals.enemyCnt--;
                KillMe(sprites, bullets, ref idx, snd);
                return;
            }

            FixAngle();

            int r = Globals.GetRand(100000);
            if (r % 120 == 0)
            {
                GenerateBullet("bullet2", "lunch_rocket", bullets, 100, 30, 1, 0);
            }

            if (idle == true)
            {
                cnt = 300;
                Friend frn = GetRandFriend(sprites);

                if (frn != null)
                {
                    target = frn;
                    float ang = DiffAngle(this, target);
                    rotVel = ang / 40f;
                }
                else rotVel = 0;

                idle = false;
            }
            else
            {
                frame++;

                if (rotateCnt > 0)
                {
                    rotateCnt--;
                    rotRad += rotVel;
                    RotatePoly(rotVel, false);
                }
                else
                {
                    vel = 5;
                    if (target != null && target.health <= 0)
                        target = null;

                    if (target != null && target.health > 0 && frame % 40 == 0)
                    {
                        float ang = DiffAngle(this, target);
                        rotVel = ang / 40f;
                    }

                    if (target != null)
                    {
                        Vector2 tmp = ChangePos(1);
                        if (Globals.DsSq(tmp, target.pos) < 20000)
                        {
                            if (Globals.DsSq(pos, target.pos) < Globals.DsSq(tmp, target.pos)) { }
                            else
                            {
                                vel = 0;
                                rotateCnt = 30;
                                float ang = DiffAngle(this, (Plane)target);
                                rotVel = ang / 30f;
                            }
                        }
                    }

                    rotRad += rotVel;
                    RotatePoly(rotVel, false);
                }

                cnt--;
                if (cnt == 0)
                {
                    idle = true;
                }
            }

            for(int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i] is Friend && Collision(this, sprites[i]))
                {
                    Globals.enemyCnt--;
                    KillMe(sprites, bullets, ref idx, Globals.exp);
                    return;
                }
            }

            pos = ChangePos(1);
            UpdatePoly(1);

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol = false)
        {
            Rectangle rec2 = new Rectangle(0, 0, texture.Width, texture.Height); //mloosh lazmla

            spriteBatch.Draw(texture, rec, rec2, color, rotRad, org, SpriteEffects.None, 0f);

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
