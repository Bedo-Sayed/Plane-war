using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace Project1
{
    public class Bullet : Sprite
    {
        public double lifeTime;
        public Sprite parent;
        public int damage;

        public Bullet(SoundEffect snd, Texture2D texture, Vector2 pos , int width , int height, float ofUp, float ofDw, float ofLf, float ofRt , float volLvl=1) 
            : base(texture, pos , width , height, ofUp, ofDw, ofLf, ofRt)
        {
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            lifeTime = 5f;
            vel = 8;
            damage = 1;
            snd.Play(volLvl * Globals.soundVol, 0.0f, 0.0f);
        }

        public override void Update(GameTime gameTime , List<Sprite> sprites , List<Bullet> bullets, ref int idx)
        {
            pos = ChangePos(1);
            UpdatePoly(1);
            lifeTime -= gameTime.ElapsedGameTime.TotalSeconds;
            if (lifeTime <= 0)
            {
                bullets.RemoveAt(idx);
                idx--;
                return;
            }

            for (int i=0; i<sprites.Count; i++)
            {
                if (this.parent is Friend && sprites[i] is Enemy && (Geometry.Intersect2(sprites[i].pol, pol) || Geometry.IsInsidePoly(pos , sprites[i].pol) ) )
                {
                    //continue;
                    ((Plane)sprites[i]).health -= damage;

                    if (((Plane)sprites[i]).health > 0)
                    {
                        Sprite exp;
                        Vector2 tmpPos = (bullets[idx].pol[3] + bullets[idx].pol[2]) / 2f;
                        var dir = new Vector2((float)Math.Cos(rotRad), (float)Math.Sin(rotRad));
                        tmpPos += dir * 50;
                        exp = new Explosion(Globals.exp, Globals.expText, tmpPos , 100, 100, 0.2f, 0.2f, 0.2f, 0.2f , 5 , 5);
                        Globals.toBeAdded.Add(exp);
                    }

                    bullets.RemoveAt(idx);
                    idx--;
                    return;
                }
                else if (this.parent is Enemy && sprites[i] is Friend && (Geometry.Intersect2(sprites[i].pol, pol) || Geometry.IsInsidePoly(pos, sprites[i].pol)) )
                {
                    //continue;
                    ((Friend)sprites[i]).health -= damage;
     
                    if (((Plane)sprites[i]).health > 0)
                    {
                        Sprite exp;
                        Vector2 tmpPos = (bullets[idx].pol[3] + bullets[idx].pol[2]) / 2f;
                        var dir = new Vector2((float)Math.Cos(rotRad), (float)Math.Sin(rotRad));
                        tmpPos += dir * 50;
                        exp = new Explosion(Globals.exp, Globals.expText, tmpPos, 100, 100, 0.2f, 0.2f, 0.2f, 0.2f , 5 , 5);
                        Globals.toBeAdded.Add(exp);
                    }

                    bullets.RemoveAt(idx);
                    idx--;
                    return;
                }
            }

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch , bool drawPol)
        {
            spriteBatch.Draw(texture, rec, null , color , rotRad , org, SpriteEffects.None, 0f);

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
