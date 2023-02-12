using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;

namespace Project1
{
    public abstract class Globals
    {
        public static ContentManager con;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static Texture2D red;
        public static SpriteFont font;
        public static string s = "Hi";
        public static int w;
        public static int h;
        public static int cnt = 0;
        public static int enemyCnt = 0;
        public static int friendCnt = 0;
        public static float soundVol;
        public static Texture2D expText;
        public static Vector2 topLeft, botRight;
        public static Matrix tran = Matrix.CreateTranslation(0, 0, 0f);
        public static float camX, camY;
        public static bool drawPoly; 
        public static Random rnd;
        public static SoundEffect exp;
        public static GraphicsDevice device;
        public static double pipi = Math.Acos(-1) * 2;
        public static bool isRel = true;
        public static SoundEffect playerShot;
        public static SoundEffect backgroundSound;
        public static SoundEffect win, lose , stateSnd;
        public static SoundEffectInstance backSnd;
        public static List<Sprite> toBeAdded;

        public static void Init(ContentManager content , SpriteBatch sp , GraphicsDeviceManager gr , GraphicsDevice dv)
        {
            con = content;
            spriteBatch = sp;
            graphics = gr;
            font = con.Load<SpriteFont>("galleryFont");
            red = con.Load<Texture2D>("red");
            soundVol = 0.5f;
            drawPoly = true;
            exp = con.Load<SoundEffect>("sounds/explosion_sound");
            expText = con.Load<Texture2D>("pngegg");
            device = dv;
            rnd = new Random();
            win = con.Load<SoundEffect>("sounds/tadaa");
            lose = con.Load<SoundEffect>("sounds/game_over");
        }

        public static int GetRand(int x)
        {
            return rnd.Next(x);
        }

        public static float DsSq(Vector2 pos , Vector2 pos2)
        {
            return (pos.X - pos2.X) * (pos.X - pos2.X) + (pos.Y - pos2.Y) * (pos.Y - pos2.Y);
        }
    }
}
