using SnakeGame.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameEngine
    {
        InputPicker inputPicker = new InputPicker();

        //private int[,] map = new int[11, 11];
        private int mapSize = 11;
        private Tools.Point snake = new Tools.Point(5, 5);
        private DirectionEnum snakeDirection = DirectionEnum.Right;
       
        public void DirectionChange(DirectionEnum direction)
        {
            this.snakeDirection = direction;
            // Console.WriteLine(this.snakeDirection);
        }

        private void DrawMap()
        {
            for(int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if(snake.X == j && snake.Y == i)
                    {
                        Console.Write("*");
                    }
                    else
                        Console.Write("_");
                }
                Console.WriteLine();
            }
        }
        public void Run()
        {
            inputPicker.OnDirectionChanged = new Action<DirectionEnum>(DirectionChange);
            inputPicker.Run();

            while (true)
            {
                Console.Clear();
                switch (snakeDirection)
                {
                    case DirectionEnum.Up:
                        {
                            snake.Y--;
                            break;
                        }
                    case DirectionEnum.Down:
                        {
                            snake.Y++;
                            break;
                        }
                    case DirectionEnum.Left:
                        {
                            snake.X--;
                            break;
                        }
                    case DirectionEnum.Right:
                        {
                            snake.X++;
                            break;
                        }
                }
                if (snake.X >= mapSize || snake.Y >= mapSize || snake.X < 0 || snake.Y < 0)
                {
                    inputPicker.Abort();
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("Game Over!!!");
                    break;
                }
                DrawMap();
                Thread.Sleep(500); 
            }
        }
    }
}
