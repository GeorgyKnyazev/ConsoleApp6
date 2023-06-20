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
            const ConsoleKey editKeyInMenu = ConsoleKey.D1;
            const ConsoleKey gameKeyInMenu = ConsoleKey.D2;
            const ConsoleKey exitKeyInMenu = ConsoleKey.D3;
            bool isProgramWork = true;
            int playerX;
            int playerY;

            char[,] map = ReadMap("map1", out playerX, out playerY); 

            while (isProgramWork == true)
            {
                Console.Clear();
                Console.WriteLine($"{editKeyInMenu} - Редактирование");
                Console.WriteLine($"{gameKeyInMenu} - игра");
                Console.WriteLine($"{exitKeyInMenu} - выход");
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case editKeyInMenu:
                        ModifiMap(ref map, ref playerX, ref playerY);
                        break;

                    case gameKeyInMenu:
                        PlayOnMap(map, playerX, playerY);
                        break;

                    case exitKeyInMenu:
                        isProgramWork = false;
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
            string wallKeylInMenu  = "1";
            string playerKeyInMenu = "2";
            string deleteKeyInMenu = "3";
            string escapeKeyInMenu = "4";
            string upMoveKeyInMenu = "стрелка вверх";
            string downMoveKeyInMenu = "стрелка вниз";
            string rightMoveKeyInMenu = "стрелка вправо";
            string leftMoveKeyInMenu = "стрелка влево";
            char wallSymbol = '#';
            char playerSymbol = '@';
            char emptySymbol = ' ';
            bool isModifyMap = true;
            int cursorPositionX = 0;
            int cursorPositionY = 0;

            while (isModifyMap == true)
            {
               Console.Clear();
                DrawMap(map);

                Console.SetCursorPosition(0, map.GetLength(0) + 1);
                Console.WriteLine($"Для движения вверх   - {upMoveKeyInMenu}");
                Console.WriteLine($"Для движения вниз    - {downMoveKeyInMenu}");
                Console.WriteLine($"Для движения вправо  - {rightMoveKeyInMenu}");
                Console.WriteLine($"Для движения влево   - {leftMoveKeyInMenu}");
                Console.WriteLine($"Нарисовать #        - Введите   {wallKeylInMenu}");
                Console.WriteLine($"Нарисовать @        - Введите   {playerKeyInMenu}");
                Console.WriteLine($"Стереть символ      - Введите   {deleteKeyInMenu}");
                Console.WriteLine($"Для выхода          - Введите   {escapeKeyInMenu}");
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.D1:
                        map[cursorPositionY, cursorPositionX] = wallSymbol; 
                        break;

                    case ConsoleKey.D2:
                        map[cursorPositionY, cursorPositionX] = playerSymbol;
                        playerX = cursorPositionX;
                        playerY = cursorPositionY;
                        break;

                    case ConsoleKey.D3:
                        map[cursorPositionY, cursorPositionX] = emptySymbol;
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

                        isModifyMap = false;
                        break;
                }
            }

            Console.SetCursorPosition(0, 0);
            return;
        }

        static void PlayOnMap(char[,] map, int playerX, int playerY)
        {
            char wallSymbol = '#';
            char playerSymbol = '@';
            string exitKeyInMenu = "1";
            char deletePlayerSymbol = ' ';
            int playerMoveX = 0;
            int playerMoveY = 0;
            bool isPlayOnMap = true;

            Console.Clear();
            DrawMap(map);

            Console.SetCursorPosition(0, map.GetLength(0) + 1);
            Console.WriteLine($"Для выхода нажмите - {exitKeyInMenu}");

            while (isPlayOnMap == true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    MoveCursor(key, ref playerMoveX, ref playerMoveY, ref isPlayOnMap);

                    if (map[playerX + playerMoveX, playerY + playerMoveY] != wallSymbol)
                    {
                        DrawSymbol(deletePlayerSymbol, playerY, playerX);

                        playerX += playerMoveX;
                        playerY += playerMoveY;

                        DrawSymbol(playerSymbol, playerY, playerX);
                    }
                }
            }
        }

        static void DrawSymbol(char symbol, int playerY, int playerX)
        {
            Console.SetCursorPosition(playerY, playerX);
            Console.Write(symbol);
        }

        static void MoveCursor(ConsoleKeyInfo key, ref int playerMoveX, ref int playerMoveY, ref bool isPlayOnMap)
        {
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
                    isPlayOnMap = false;
                    break;
            }
        }
    }   
}
