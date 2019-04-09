using System;
using SFML.Graphics;
using SFML.Window;

namespace Quadrapassel
{
    public abstract class Scene
    {
        protected RenderWindow Window;
        protected Color ClearColor;

        protected bool ReturnState;

        protected Scene(uint width, uint height, string name, Color clearColor)
        {
            Window = new RenderWindow(new VideoMode(width, height), name, Styles.Default);
            ClearColor = clearColor;

            Window.Closed += OnClosed;
            Window.KeyPressed += WindowKeyPressed;
            Window.KeyReleased += WindowKeyReleased;
            Window.MouseButtonPressed += WindowMouseButtonPressed;
            Window.MouseButtonReleased += WindowMouseButtonReleased;
            Window.MouseMoved += WindowMouseMoved;
            Window.MouseWheelScrolled += WindowMouseScrolled;
        }

        protected abstract void CheckCollide(MouseMoveEventArgs e);
        protected abstract void CheckClick(MouseButtonEventArgs e);
        protected abstract void CheckUnClick(MouseButtonEventArgs e);
        protected abstract void CheckKeyPressed(KeyEventArgs e);
        protected abstract void CheckKeyReleased(KeyEventArgs e);

        protected abstract void LoadContent();
        protected abstract void Initialize();

        protected abstract void Tick();
        protected abstract void Render();

        public bool Run()
        {
            LoadContent();
            Initialize();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                Tick();

                Window.Clear(ClearColor);
                Render();
                Window.Display();
            }

            return ReturnState;
        }

        private void WindowMouseScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            //Console.WriteLine(e.Delta);
        }

        private void WindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            //Console.WriteLine("X: {0} Y: {1}", e.X, e.Y);
            CheckCollide(e);
        }

        private void WindowMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            CheckUnClick(e);
        }

        private void WindowMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine(e.Button);
            CheckClick(e);
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            // Console.WriteLine(e.Code);
            CheckKeyPressed(e);
        }

        private void WindowKeyReleased(object sender, KeyEventArgs e)
        {
            CheckKeyReleased(e);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            ReturnState = false;
            Window.Close();
        }
    }
}
