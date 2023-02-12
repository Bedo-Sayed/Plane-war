using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Threading;

namespace Project1
{
    public class Sprite
    {
        public Texture2D texture;
        public Vector2 pos;
        Vector2 topLeft, topRight, botLeft, botRight;
        public List<Vector2> pol;
        public float vel, rotRad, rotVel, totRot;
        public int width, height , velRotDeg;
        public Rectangle rec;
        public Color color;
        public Vector2 org;
        public float opacity , offUp , offDown , offLeft , offRight;

        public void Build()
        {
            Vector2 tmp = new Vector2(offUp * width, offUp * height);
            topLeft = new Vector2(pos.X - width / 2f, pos.Y - height / 2f);

            tmp = new Vector2(-offRight * width, offUp * height);
            topRight = new Vector2(topLeft.X + width, topLeft.Y);
            topRight += tmp;

            tmp = new Vector2(offLeft * width, -offDown * height);
            botLeft = new Vector2(topLeft.X, topLeft.Y + height);
            botLeft += tmp;

            tmp = new Vector2(-offRight * width, -offDown * height);
            botRight = new Vector2(topLeft.X + width, topLeft.Y + height);
            botRight += tmp;

            tmp = new Vector2(offLeft * width, offUp * height);
            topLeft += tmp;

            pol = new List<Vector2>() { topLeft, botLeft, botRight, topRight };
            rec = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public Sprite()
        {

        }
        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            color = Color.White;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);
            opacity = 1f;

            offUp = offDown = offLeft = offRight = 0;
            Vector2 tmp = new Vector2(offUp * width, offUp * height);
            topLeft = new Vector2(pos.X - width / 2f, pos.Y - height / 2f);

            tmp = new Vector2(-offRight * width, offUp * height);
            topRight = new Vector2(topLeft.X + width, topLeft.Y);
            topRight += tmp;

            tmp = new Vector2(offLeft * width, -offDown * height);
            botLeft = new Vector2(topLeft.X, topLeft.Y + height);
            botLeft += tmp;

            tmp = new Vector2(-offRight * width, -offDown * height);
            botRight = new Vector2(topLeft.X + width, topLeft.Y + height);
            botRight += tmp;

            tmp = new Vector2(offLeft * width, offUp * height);
            topLeft += tmp;

            pol = new List<Vector2>() { topLeft, botLeft, botRight, topRight };
            rec = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public Sprite(Texture2D texture, Vector2 pos , int width , int height , float offUp , float offDown , float offLeft , float offRight)
        {
            opacity = 1;
            this.texture = texture;
            this.pos = pos;
            this.width = width;
            this.height = height;
            color = Color.White;
            org = new Vector2(texture.Width / 2f, texture.Height / 2f);

            Vector2 tmp = new Vector2(offUp * width, offUp * height);
            topLeft = new Vector2(pos.X - width/2f , pos.Y - height/2f);

            tmp = new Vector2(-offRight * width, offUp * height);
            topRight = new Vector2(topLeft.X + width, topLeft.Y);
            topRight += tmp;

            tmp = new Vector2(offLeft * width, -offDown * height);
            botLeft = new Vector2(topLeft.X, topLeft.Y + height);
            botLeft += tmp;

            tmp = new Vector2(-offRight * width, -offDown * height);
            botRight = new Vector2(topLeft.X + width, topLeft.Y + height);
            botRight += tmp;

            tmp = new Vector2(offLeft * width, offUp * height);
            topLeft += tmp;

            pol = new List<Vector2>() { topLeft, botLeft, botRight, topRight };
            rec = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }
        protected void FixAngle()
        {
            while (rotRad > Globals.pipi || Math.Abs(rotRad - Globals.pipi) < 0.00001f)
                rotRad -= (float)Globals.pipi;

            while (rotRad < 0)
                rotRad += (float)Globals.pipi;
        }
        protected float DiffAngle(Sprite pl, Sprite pl2)
        {
            Vector2 diff = pl2.pos - pl.pos;
            float ang = (float)Math.Atan2((float)diff.Y, (float)diff.X);
            ang -= pl.rotRad;
            ang = CloserAng(ang);
            return ang;
        }

        protected float CloserAng(float ang)
        {
            while (ang < 0) ang += (float)Globals.pipi;

            if (2 * ang > Globals.pipi)
                ang -= (float)Globals.pipi;
            return ang;
        }

        protected void GenerateBullet(string bullName, string bullSound, List<Bullet> bullets, int width, int height, int damage, float volLvl)
        {
            var textt = Globals.con.Load<Texture2D>("rockets/" + bullName);
            Bullet bl2 = new Bullet(Globals.con.Load<SoundEffect>("sounds/" + bullSound), textt, pos, width, height, 0, 0, 0, 0, volLvl);
            bl2.parent = this;
            bl2.rotRad = rotRad;
            bl2.damage = damage;

            for (int i = 0; i < 4; i++)
            {
                bl2.pol[i] = bl2.RotateAbout(bl2.pol[i], bl2.pos, bl2.rotRad, false);
            }

            bullets.Add(bl2);
        }

        protected bool Collision(Sprite pl, Sprite pl2)
        {
            if (Geometry.Intersect2(pl.pol, pl2.pol))
            {
                return true;
            }
            else if (Geometry.IsInsidePoly(pl.pos, pl2.pol) || Geometry.IsInsidePoly(pl2.pos, pl.pol))
            {
                return true;
            }
            return false;
        }

        public Vector2 RotateAbout(Vector2 point , Vector2 org , float an , bool deg = true)
        {
            Vector2 tmp =  point - org;

            float ang = an;
            if (deg)
                ang = MathHelper.ToRadians(an);

            point.X = (float)(Math.Cos(ang) * tmp.X - Math.Sin(ang) * tmp.Y);
            point.Y = (float)(Math.Sin(ang) * tmp.X + Math.Cos(ang) * tmp.Y);

            point += org;

            return point;
        }

        protected void RotatePoly(float rt , bool deg = true)
        {
            for (int i=0; i<pol.Count; i++)
            {
                pol[i] = RotateAbout(pol[i], pos, rt , deg);
            }
        }

        protected void UpdatePoly(int d)
        {
            var dir = new Vector2((float)Math.Cos(rotRad), (float)Math.Sin(rotRad));
            for (int i = 0; i < 4; i++)
            {
                pol[i] += dir * vel * d;
            }
        }
        protected Vector2 ChangePos(int d)
        {
            var dir = new Vector2((float)Math.Cos(rotRad), (float)Math.Sin(rotRad));

            Vector2 result = pos + dir * vel * d;

            return result;
        }

        public virtual void Update(GameTime gameTime)
        {
            pos = ChangePos(1);
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprites, List<Bullet> bullets, ref int idx)
        { 

        }

        public virtual void Draw(SpriteBatch spriteBatch , bool drawPol = false)
        {
            spriteBatch.Draw(texture, rec, null, color, rotRad, org , SpriteEffects.None, 0f);
        }
    }
}
