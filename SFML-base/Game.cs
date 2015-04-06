using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using SFML.Audio;

namespace SFML_base
{
    class Game
    {
        Time TimePerFrame = Time.FromSeconds(1.0f / 60.0f);
        const uint _WIDTH = 800;
        const uint _HEIGHT = 600;

        RenderWindow rw;
        View mainView;

        public Game()
        {
            rw = new RenderWindow(new VideoMode(_WIDTH, _HEIGHT), "SFML-program", Styles.Close, new ContextSettings(32, 32, 4, 1, 0));
            rw.Closed += rw_Closed;

            mainView = new View(rw.GetView());

            fpsText = new Text();
            fpsText.Position = new Vector2f(15, 15);
            fpsText.Font = new Font("Roboto-Regular.ttf");
            fpsText.Scale = new Vector2f(0.45f, 0.45f);
            fpsClock = new Clock();
        }

        void rw_Closed(object sender, EventArgs e)
        {
            rw.Close();
        }

        public void Run()
        {
            Clock clock = new Clock();
            Time timeSinceLastUpdate = Time.Zero; 

            while(rw.IsOpen)
            {
                
                rw.DispatchEvents();

                timeSinceLastUpdate += clock.Restart();

                while(timeSinceLastUpdate > TimePerFrame)
                {
                    Console.WriteLine(timeSinceLastUpdate.AsMilliseconds() + ", " + TimePerFrame.AsMilliseconds());
                    timeSinceLastUpdate -= TimePerFrame;
                    rw.DispatchEvents();

                    update(TimePerFrame);
                }
                
                render();

                updateFps(TimePerFrame);
            }
        }

        private void update(Time time)
        {
            rw.SetView(mainView);
        }

        private void render()
        {
            rw.Clear(Color.Black);

            rw.SetView(rw.GetView());
            rw.Draw(fpsText);
            rw.SetView(mainView);

            rw.Display();
        }

        Clock fpsClock;
        float lastTime = 0;
        Time fpsUpdateTime;
        Text fpsText;

        private void updateFps(Time time)
        {
            fpsUpdateTime += time;
            float currentTime = fpsClock.Restart().AsSeconds();
            float fps = 1 / currentTime;
            lastTime = currentTime;

            if(fpsUpdateTime >= Time.FromSeconds(1))
            {
                if(fps >= 60)
                {
                    fpsText.Color = Color.Green;
                }
                else if(fps < 25)
                {
                    fpsText.Color = Color.Red;
                }
                else if(fps < 60)
                {
                    fpsText.Color = Color.Yellow;
                }

                string fpsString = fps.ToString();
                fpsString = fpsString.Substring(0, fpsString.IndexOf(','));

                fpsText.DisplayedString = "FPS: " + fpsString + "\n";
                fpsUpdateTime -= Time.FromSeconds(1);
            }
        }
    }
}
