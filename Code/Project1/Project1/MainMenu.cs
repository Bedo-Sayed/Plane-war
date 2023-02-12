using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class MainMenu : GameState
    {
        SpriteBatch spriteBatch;
        List<Button> btnList;
        Texture2D background;

        private void Pre(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            this.spriteBatch = spriteBatch;
            var text = Globals.font;
            btnList = new List<Button>();

            Button btn = new Button(new Vector2(525, 350));
            btn.s = "    Play";
            Button btn2 = new Button(new Vector2(525, 420));
            if (Globals.soundVol == 0f)
                btn2.s = "  Unmute";
            else
                btn2.s = "    Mute";
            Button btn3 = new Button(new Vector2(525, 490));
            btn3.s = "    Exit";

            btnList.Add(btn);
            btnList.Add(btn2);
            btnList.Add(btn3);

            background = content.Load<Texture2D>("back_main_menu");
        }

        public MainMenu(ContentManager content, SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            Pre(content, spriteBatch, graphics);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button btn in btnList)
            {
                btn.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
           
            spriteBatch.Draw(background, new Rectangle(0, 0, 1200, 680) , Color.White);
            foreach (Button btn in btnList)
            {
                btn.Draw(spriteBatch);
            }
           spriteBatch.End();
        }
    }
}
