using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Button : Sprite
    {
        SpriteFont font;
        public string s;
        public string description;
        bool isHover;

        public Button(Vector2 pos) : base()
        {
            description = "";
            this.font = Globals.font;
            color = Color.White;
            this.pos = pos;
            rec = new Rectangle((int)pos.X, (int)pos.Y, 150 , 50);
            texture = Globals.red;
        }

        void GoPlay()
        {
            Main.nxtState = new ChoosingMission(Globals.con, Globals.spriteBatch, Globals.graphics);
        }
        void ChangeVolume()
        {
            if (Globals.soundVol == 0)
            {
                Globals.soundVol = 0.5f;
                s = "    Mute";
                Main.btnList[1].s = "    Mute"; 
            }
            else
            {
                Globals.soundVol = 0f;
                s = "  Unmute";
                Main.btnList[1].s = "  Unmute";
            }
        }

        private string GetName()
        {
            string str = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ') continue;
                for (int j = i; j < s.Length; j++)
                {
                    str += s[j];
                }
                return str;
            }
            return "";
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            Rectangle mouseRec = new Rectangle(ms.X, ms.Y, 1, 1);
            if (mouseRec.Intersects(rec)) isHover = true;
            else isHover = false;

            string str = GetName();
            
            if (isHover && Globals.isRel && ms.LeftButton == ButtonState.Pressed)
            {
                Globals.isRel = false;

                if (str == "Play")
                {
                    GoPlay();
                }
                else if (str == "Mute" || str == "Unmute")
                {
                    ChangeVolume();
                }
                else if (str == "Exit")
                {
                    Game1.exitGame = true;
                }
                else if (str == "Mission 1")
                {
                    Main.nxtState = new Mission1(Globals.con, Globals.spriteBatch, Globals.graphics);
                }
                else if (str == "Mission 2")
                {
                    Main.nxtState = new Mission2(Globals.con, Globals.spriteBatch, Globals.graphics);
                }
                else if (str == "Mission 3")
                {
                    Main.nxtState = new Mission3(Globals.con, Globals.spriteBatch, Globals.graphics);
                }
                else if (str == "Resume")
                {
                    Main.pause = 0;
                    Main.keyRel = true;
                    if (Globals.backSnd.State != SoundState.Stopped)
                        Globals.backSnd.Resume();
                }
                else if (str == "Menu")
                {
                    Globals.backSnd.Stop();
                    Main.cnt = 0;
                    Main.nxtState = new MainMenu(Globals.con, Globals.spriteBatch, Globals.graphics);
                }
                else if (str == "Mission 4")
                {
                    Main.nxtState = new Mission4(Globals.con, Globals.spriteBatch, Globals.graphics);
                }
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                Globals.isRel = true;
            }

            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public override void Draw(SpriteBatch spriteBatch, bool drawPol = false)
        {
            if (isHover)
                color = Color.DarkRed;
            else
                color = Color.Red;
            spriteBatch.Draw(Globals.red , rec, color);
            spriteBatch.DrawString(font , s , new Vector2(rec.X, rec.Y+7) , Color.White);
            if (description.Length > 0)
            {
                spriteBatch.DrawString(font, description, new Vector2(rec.X, rec.Y + 7), Color.White , 0 , new Vector2(0,0) , 0.5f , SpriteEffects.None , 1);
            }
        }
    }
}
