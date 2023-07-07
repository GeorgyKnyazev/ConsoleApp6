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
            int playerMapPositionOnX;
            int playerMapPositionOnY;

            char[,] map = ReadMap("map1", out playerMapPositionOnX, out playerMapPositionOnY); 

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
                        ModifyMap(ref map, ref playerMapPositionOnX, ref playerMapPositionOnY);
                        break;

                    case GameKeyInMenu:
                        PlayOnMap(map, playerMapPositionOnX, playerMapPositionOnY);
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
            const ConsoleKey ExitKeyInMenu = ConsoleKey.D4;

            string upMoveKeyInMenu = "стрелка вверх";
            string downMoveKeyInMenu = "стрелка вниз";
            string rightMoveKeyInMenu = "стрелка вправо";
            string leftMoveKeyInMenu = "стрелка влево";

            char wallSymbol = '#';
            char playerSymbol = '@';
            char emptySymbol = ' ';
            bool isModifyMap = true;
            int cursorPositionOnMapX = 0;
            int cursorPositionOnMapY = 0;

            while (isModifyMap == true)
            {
               Console.Clear();
                DrawMap(map);
                Console.SetCursorPosition(cursorPositionOnMapX, cursorPositionOnMapY);

                Console.SetCursorPosition(0, map.GetLength(0) + 1);
                Console.WriteLine($"Для движения вверх   - {upMoveKeyInMenu}");
                Console.WriteLine($"Для движения вниз    - {downMoveKeyInMenu}");
                Console.WriteLine($"Для движения вправо  - {rightMoveKeyInMenu}");
                Console.WriteLine($"Для движения влево   - {leftMoveKeyInMenu}");
                Console.WriteLine($"Нарисовать {wallSymbol}        - Введите   {DrawWallKeyInMenu}");
                Console.WriteLine($"Нарисовать {playerSymbol}        - Введите   {DrawPlayerKeyInMenu}");
                Console.WriteLine($"Стереть символ      - Введите   {DeleteKeyInMenu}");
                Console.WriteLine($"Для выхода          - Введите   {ExitKeyInMenu}");
                Console.SetCursorPosition(cursorPositionOnMapX, cursorPositionOnMapY);

                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();

                switch (consoleKeyInfo.Key)
                {
                    case DrawWallKeyInMenu:
                        DrawSymbol(wallSymbol, cursorPositionOnMapY, cursorPositionOnMapX); 
                        break;

                    case DrawPlayerKeyInMenu:
                        DrawSymbol(playerSymbol, cursorPositionOnMapY, cursorPositionOnMapX);
                        break;

                    case DeleteKeyInMenu:
                        DrawSymbol(emptySymbol, cursorPositionOnMapY, cursorPositionOnMapX);
                        break;

                    case ConsoleKey.UpArrow:
                        GetDirection(ConsoleKey.UpArrow, map, ref cursorPositionOnMapX, ref cursorPositionOnMapY);
                        break;

                    case ConsoleKey.DownArrow:
                        GetDirection(ConsoleKey.DownArrow, map, ref cursorPositionOnMapX, ref cursorPositionOnMapY);
                        break;

                    case ConsoleKey.RightArrow:
                        GetDirection(ConsoleKey.RightArrow, map, ref cursorPositionOnMapX, ref cursorPositionOnMapY);
                        break;

                    case ConsoleKey.LeftArrow:
                        GetDirection(ConsoleKey.LeftArrow, map, ref cursorPositionOnMapX, ref cursorPositionOnMapY);
                        break;

                    case ExitKeyInMenu:
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
            int playerDirectionX = 0;
            int playerDirectionY = 0;
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

                    GetDirection(key, ref playerDirectionX, ref playerDirectionY, ref isPlayOnMap);

                    if (map[playerX + playerDirectionX, playerY + playerDirectionY] != wallSymbol)
                    {
                        DrawSymbol(deletePlayerSymbol, playerY, playerX);

                        playerX += playerDirectionX;
                        playerY += playerDirectionY;

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

        static void GetDirection(ConsoleKeyInfo key, ref int playerMoveX, ref int playerMoveY, ref bool isPlayOnMap)
        {
            const ConsoleKey GetDirectionPlayerUp = ConsoleKey.UpArrow;
            const ConsoleKey GetDirectionPlayerDown = ConsoleKey.DownArrow;
            const ConsoleKey GetDirectionplayerLeft = ConsoleKey.LeftArrow;
            const ConsoleKey GetDirectionPlayerRight = ConsoleKey.RightArrow;
            const ConsoleKey IsPlayOnMap = ConsoleKey.D1;

            switch (key.Key)
            {
                case GetDirectionPlayerUp:
                    playerMoveX = -1;
                    playerMoveY = 0;
                    break;

                case GetDirectionPlayerDown:
                    playerMoveX = 1;
                    playerMoveY = 0;
                    break;

                case GetDirectionplayerLeft:
                    playerMoveX = 0;
                    playerMoveY = -1;
                    break;

                case GetDirectionPlayerRight:
                    playerMoveX = 0;
                    playerMoveY = 1;
                    break;

                case IsPlayOnMap:
                    isPlayOnMap = false;
                    break;
            }
        }

        static void GetDirection(ConsoleKey key , char[,] map, ref int cursorPositionX, ref int cursorPositionY)
        {
            if (key == ConsoleKey.LeftArrow)
            {
                if (cursorPositionX != 0)
                {
                    cursorPositionX--;
                }
            }
            else if (key == ConsoleKey.RightArrow)
            {
                if (cursorPositionX < map.GetLength(1) - 1)
                {
                    cursorPositionX++;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                if (cursorPositionY < map.GetLength(0) - 1)
                {
                    cursorPositionY++;
                }
            }
            else if (key == ConsoleKey.UpArrow)
            {
                if (cursorPositionY != 0)
                {
                    cursorPositionY--;
                }
            }
        }
    }   
}
