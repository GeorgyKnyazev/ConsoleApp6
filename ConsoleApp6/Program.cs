using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool programwork = true;
            int playerX;
            int playerY;

            char[,] map = ReadMap("map1", out playerX, out playerY); 

            while (programwork == true)
            {
                Console.Clear();
                Console.WriteLine("1 - Редактирование");
                Console.WriteLine("2 - игра");
                Console.WriteLine("3 выход");
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.D1:
                        ModifiMap(ref map, ref playerX, ref playerY);

                        break;
                    case ConsoleKey.D2:
                        PlayOnMap(ref map, playerX, playerY);
                        break;
                    case ConsoleKey.D3:
                        programwork = false;
                        break;
                }
            }

        }

        static char[,] ReadMap(string mapName, out int playerX, out int playerY)
        {
            playerX = 0;
            playerY = 0;

            string[] newFile = File.ReadAllLines($"maps/{mapName}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if (map[i, j] == '@')
                    {
                        playerX = i;
                        playerY = j;
                    }
                }
            }
            return map;
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void ModifiMap( ref char[,] map, ref int playerX, ref int playerY)
        {
            char wallSymbol = '1';
            char playerSymbol = '2';
            char deleteSymbol = '3';
            char escapeSymbol = '4';
            bool modifiMap = true;
            int cursorPositionX = 0;
            int cursorPositionY = 0;


            while (modifiMap == true)
            {
               Console.Clear();

                DrawMap(map);

                Console.SetCursorPosition(0, map.GetLength(0) + 1);
                Console.WriteLine("Для движения вверх  - стрелочка вверх");
                Console.WriteLine("Для движения вниз   - стрелочка вниз");
                Console.WriteLine("Для движения вправо - стрелочка вправо");
                Console.WriteLine("Для движения влево  - стрелочка влево");
                Console.WriteLine($"Нарисовать #        - Введите   {wallSymbol}");
                Console.WriteLine($"Нарисовать @        - Введите   {playerSymbol}");
                Console.WriteLine($"Стереть символ      - Введите   {deleteSymbol}");
                Console.WriteLine($"Для выхода          - Введите   {escapeSymbol}");
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.D1:
                        map[cursorPositionY, cursorPositionX] = '#'; 
                        break;
                    case ConsoleKey.D2:
                        map[cursorPositionY, cursorPositionX] = '@';
                        playerX = cursorPositionX;
                        playerY = cursorPositionY;
                        break;
                    case ConsoleKey.D3:
                        map[cursorPositionY, cursorPositionX] = ' ';
                        break;
                    case ConsoleKey.UpArrow:
                        if(cursorPositionY != 0)
                        {
                            cursorPositionY--;
                        }
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                        break;
                    case ConsoleKey.DownArrow:
                        if(cursorPositionY < map.GetLength(0) - 1)
                        {
                            cursorPositionY++;
                        }
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                        break;
                    case ConsoleKey.RightArrow:
                        if(cursorPositionX < map.GetLength(1) - 1)
                        {
                            cursorPositionX++;
                        }
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                        break;
                    case ConsoleKey.LeftArrow:
                        if(cursorPositionX != 0)
                        {
                            cursorPositionX--;
                        }
                        Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                        break;
                    case ConsoleKey.D4:
                        modifiMap = false;
                        break;
                }
            }
            Console.SetCursorPosition(0, 0);
            return;
        }
        static void PlayOnMap(ref char[,] map, int playerX, int playerY)
        {
            int playerMoveX = 0;
            int playerMoveY = 0;
            bool playOnMap = true;

            Console.Clear();
            DrawMap(map);

            Console.SetCursorPosition(0, map.GetLength(0) + 1);
            Console.WriteLine("Для выхода нажмите - 1");

            while (playOnMap == true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            playerMoveX = -1;
                            playerMoveY = 0;
                            break;
                        case ConsoleKey.DownArrow:
                            playerMoveX = 1;
                            playerMoveY = 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            playerMoveX = 0;
                            playerMoveY = -1;
                            break;
                        case ConsoleKey.RightArrow:
                            playerMoveX = 0;
                            playerMoveY = 1;
                            break;
                        case ConsoleKey.D1:
                            playOnMap = false;
                            break;
                    }

                    if (map[playerX + playerMoveX, playerY + playerMoveY] != '#')
                    {
                        Console.SetCursorPosition(playerY, playerX);
                        Console.Write(" ");

                        playerX += playerMoveX;
                        playerY += playerMoveY;

                        Console.SetCursorPosition(playerY, playerX);
                        Console.Write("@");
                    }
                }
            }
        }
    }   
}
