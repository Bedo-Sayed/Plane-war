using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Project1
{
    public class Main
    {
        //GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        //ContentManager content;
        public static GameState state , nxtState;
        public string s = "Normal";
        public static int pause = 0 , cnt = 0;
        public static bool keyRel;
        public static List<Button> btnList;

        public Main(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            btnList = new List<Button>();

            Button btn = new Button(new Vector2(525, 350));
            btn.s = "  Resume";
            Button btn2 = new Button(new Vector2(525, 420));
            if (Globals.soundVol == 0f)
                btn2.s = "  Unmute";
            else
                btn2.s = "    Mute";
            Button btn3 = new Button(new Vector2(525, 490));
            btn3.s = "    Menu";

            btnList.Add(btn);
            btnList.Add(btn2);
            btnList.Add(btn3);

            keyRel = true;
            if (state == null)
                state = new MainMenu(content, spriteBatch, graphics);
            nxtState = state;
        }

        public void Update(GameTime gameTime)
        {
            if (nxtState != null)
                state = nxtState;
            
            nxtState = null;

            if (cnt != 0 && cnt <= 300)
            {
                if (cnt == 1) nxtState = new MainMenu(Globals.con, Globals.spriteBatch, Globals.graphics);
                else
                {
                    if (cnt == 300) Globals.stateSnd.Play(Globals.soundVol, 0f, 0f);
                }
                cnt--;
                return;
            }
            else if (cnt != 0) cnt--;

            if (state is not Mission1 && state is not Mission2 && state is not Mission3 && state is not Mission4)
            {
                pause = 0;
                keyRel = true;
            }
            else
            {
                KeyboardState st = Keyboard.GetState();
                if (st.IsKeyDown(Keys.Space) && keyRel)
                {
                    keyRel = false;
                    pause ^= 1;
                    if (Globals.backSnd.State != SoundState.Stopped)
                    {
                        if (pause == 1)
                            Globals.backSnd.Pause();
                        else
                            Globals.backSnd.Resume();
                    }
                }
                else if (st.IsKeyUp(Keys.Space))
                    keyRel = true;
            }
            

            if (pause == 0) 
                state.Update(gameTime);
            else
            {
                for(int i = 0; i < btnList.Count; i++)
                {
                    btnList[i].Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            state.Draw(gameTime);

            if (pause == 1)
            {
                Globals.spriteBatch.Begin();
                for (int i = 0; i < btnList.Count; i++)
                {
                    btnList[i].Draw(Globals.spriteBatch);
                }
                Globals.spriteBatch.End();
            }
        }
    }
}
