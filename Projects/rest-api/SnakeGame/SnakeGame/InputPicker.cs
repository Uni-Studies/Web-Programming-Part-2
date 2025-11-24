using SnakeGame.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class InputPicker
    {
        public Action<DirectionEnum> OnDirectionChanged = null;
        private Thread thread;
        private void PickInput()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true); // true displayed the selected button in the console
                switch(keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            OnDirectionChanged(DirectionEnum.Up);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            OnDirectionChanged(DirectionEnum.Down);
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            OnDirectionChanged(DirectionEnum.Left);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            OnDirectionChanged(DirectionEnum.Right);
                            break;
                        }
                }
            }
        }

        public void Abort()
        {
            // bad practice
            try
            {
                this.thread.Abort(); // terminates the thread
            }
            catch
            {

            }
        }

        public void Run()
        {
            ThreadStart threadStartdlg = new ThreadStart(PickInput);
            thread = new Thread(threadStartdlg);
            thread.Start();
            //thread = new Thread(new ThreadStart(PickInput));
        }
    }
}
