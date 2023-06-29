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
            const ConsoleKey EditKeyInMenu = ConsoleKey.D1;
            const ConsoleKey GameKeyInMenu = ConsoleKey.D2;
            const ConsoleKey ExitKeyInMenu = ConsoleKey.D3;

            bool isProgramWork = true;
            int playerX;
            int playerY;

            char[,] map = ReadMap("map1", out playerX, out playerY); 

            while (isProgramWork == true)
            {
                Console.Clear();
                Console.WriteLine($"{EditKeyInMenu} - Редактирование");
                Console.WriteLine($"{GameKeyInMenu} - игра");
                Console.WriteLine($"{ExitKeyInMenu} - выход");
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case EditKeyInMenu:
                        ModifyMap(ref map, ref playerX, ref playerY);
                        break;

                    case GameKeyInMenu:
                        PlayOnMap(map, playerX, playerY);
                        break;

                    case ExitKeyInMenu:
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

        static void ModifyMap( ref char[,] map, ref int playerX, ref int playerY)
        {
            const ConsoleKey DrawWallKeyInMenu = ConsoleKey.D1;
            const ConsoleKey DrawPlayerKeyInMenu = ConsoleKey.D2;
            const ConsoleKey DeleteKeyInMenu = ConsoleKey.D3;
            const ConsoleKey ExitKeyINMenu = ConsoleKey.D4;

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
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);

                Console.SetCursorPosition(0, map.GetLength(0) + 1);
                Console.WriteLine($"Для движения вверх   - {upMoveKeyInMenu}");
                Console.WriteLine($"Для движения вниз    - {downMoveKeyInMenu}");
                Console.WriteLine($"Для движения вправо  - {rightMoveKeyInMenu}");
                Console.WriteLine($"Для движения влево   - {leftMoveKeyInMenu}");
                Console.WriteLine($"Нарисовать {wallSymbol}        - Введите   {DrawWallKeyInMenu}");
                Console.WriteLine($"Нарисовать {playerSymbol}        - Введите   {DrawPlayerKeyInMenu}");
                Console.WriteLine($"Стереть символ      - Введите   {DeleteKeyInMenu}");
                Console.WriteLine($"Для выхода          - Введите   {ExitKeyINMenu}");
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case DrawWallKeyInMenu:
                        DrawSymbol(wallSymbol, cursorPositionY, cursorPositionX); 
                        break;

                    case DrawPlayerKeyInMenu:
                        DrawSymbol(playerSymbol, cursorPositionY, cursorPositionX);
                        break;

                    case DeleteKeyInMenu:
                        DrawSymbol(emptySymbol, cursorPositionY, cursorPositionX);
                        break;

                    case ConsoleKey.UpArrow:

                        MoveCursor(ConsoleKey.UpArrow, map, ref cursorPositionX, ref cursorPositionY);
                        break;

                    case ConsoleKey.DownArrow:
                        MoveCursor(ConsoleKey.DownArrow, map, ref cursorPositionX, ref cursorPositionY);
                        break;

                    case ConsoleKey.RightArrow:
                        MoveCursor(ConsoleKey.RightArrow, map, ref cursorPositionX, ref cursorPositionY);
                        break;

                    case ConsoleKey.LeftArrow:
                        MoveCursor(ConsoleKey.LeftArrow, map, ref cursorPositionX, ref cursorPositionY);
                        break;

                    case ExitKeyINMenu:

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

        static void MoveCursor(ConsoleKey key , char[,] map, ref int cursorPositionX, ref int cursorPositionY)
        {
            if (key.Key == ConsoleKey.LeftArrow)
            {
                if (cursorPositionX != 0)
                {
                    cursorPositionX--;
                }
            }
            else if (key.Key == ConsoleKey.RightArrow)
            {
                if (cursorPositionX < map.GetLength(1) - 1)
                {
                    cursorPositionX++;
                }
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                if (cursorPositionY < map.GetLength(0) - 1)
                {
                    cursorPositionY++;
                }
            }
            else if (key.Key == ConsoleKey.UpArrow)
            {
                if (cursorPositionY != 0)
                {
                    cursorPositionY--;
                }
            }
        }
    }   
}
