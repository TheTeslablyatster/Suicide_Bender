using System;
using System.Collections.Generic;
using System.Linq;

namespace Suicide_Bender
{
    class Bender
    {
        public enum directions
        {
            SOUTH,
            EAST,
            NORTH,
            WEST
        }
        

        public static (int, int) start_position;
        public const directions start_direction = directions.SOUTH;
        public const bool start_breaker = false;

        public static directions current_direction;
        public static bool current_breaker;
        public static (int, int) current_position;
        public static (int, int) next_position;
        public static List<directions> way = new List<directions>();


        public static void Turn()
        {
            switch (current_direction)
            {
                case directions.SOUTH:
                    current_direction = directions.EAST;
                    break;
                case directions.EAST:
                    current_direction = directions.NORTH;
                    break;
                case directions.NORTH:
                    current_direction = directions.WEST;
                    break;
                case directions.WEST:
                    current_direction = directions.SOUTH;
                    break;
            }
        }
        public static void SetNextPosition()
        {
            switch (Program.field[current_position.Item1, current_position.Item2])
            {
                case "S":
                    current_direction = directions.EAST; 
                    break;
                case "E":
                    current_direction = directions.NORTH;
                    break;
                case "N":
                    current_direction = directions.WEST;
                    break;
                case "W":
                    current_direction = directions.SOUTH;
                    break;
            }
            while (CanMove() == false)
            {
                Turn();
            }
            switch (current_direction)
            {
                case directions.SOUTH:
                    next_position = (current_position.Item1 + 1, current_position.Item2);
                    break;
                case directions.EAST:
                    next_position = (current_position.Item1, current_position.Item2 + 1);
                    break;
                case directions.NORTH:
                    next_position = (current_position.Item1 - 1, current_position.Item2);
                    break;
                case directions.WEST:
                    next_position = (current_position.Item1, current_position.Item2 - 1);
                    break;
            }
        }
        public static bool CanMove()
        {
            if (Program.field[next_position.Item1, next_position.Item2] == "#")
                return false;
            if (Program.field[next_position.Item1, next_position.Item2] == "X" && current_breaker == false)
                return false;
            return true;
        }
        public static void Move()
        {
            SetNextPosition();
            current_position = next_position;
            way.Add(current_direction);
        }
    }


    class Program
    {
        public static int field_lines;
        public static int field_columns;
        public static bool is_loop = false;
        public static bool is_dead = false;

        public static string[,] field = new string[100, 100];

        static void Input()
        {
            int[] field_proportions = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            field_lines = field_proportions[0];
            field_columns = field_proportions[1];
            string current_line;

            for (int i = 0; i < field_lines; i++)
            {
                current_line = Console.ReadLine();
                for (int j = 0; j < field_columns; j++)
                {
                    field[i, j] = current_line.Substring(j, 1);
                    if (field[i, j] == "@")
                    {
                        Bender.start_position = (i, j);
                        Bender.current_position = (i, j);
                        Bender.current_breaker = false;
                        Bender.current_direction = Bender.directions.SOUTH;
                    }
                }
            }

        }
        static void Prediction()
        {
            while (is_loop == false && is_dead == false)
            {
                Bender.Move();
                if (Bender.current_breaker == Bender.start_breaker && Bender.current_position == Bender.start_position && Bender.current_direction == Bender.start_direction)
                {
                    is_loop = true;
                }
                if (field[Bender.current_position.Item1, Bender.current_position.Item2] == "$")
                {
                    is_dead = true;
                }
                if (field[Bender.current_position.Item1, Bender.current_position.Item2] == "B")
                    if (Bender.current_breaker == true)
                        Bender.current_breaker = false;
                    if (Bender.current_breaker == false)
                         Bender.current_breaker = true;
                if (field[Bender.current_position.Item1, Bender.current_position.Item2] == "X")
                    field[Bender.current_position.Item1, Bender.current_position.Item2] = " ";
            }
        }
        static void Output()
        {
            if (is_loop == true)
                Console.WriteLine("LOOP");
            if (is_dead == true)
                foreach (Bender.directions direction in Bender.way)
                {
                    switch (direction)
                    {
                        case Bender.directions.SOUTH:
                            Console.WriteLine("SOUTH");
                            break;
                        case Bender.directions.EAST:
                            Console.WriteLine("EAST");
                            break;
                        case Bender.directions.NORTH:
                            Console.WriteLine("NORTH");
                            break;
                        case Bender.directions.WEST:
                            Console.WriteLine("WEST");
                            break;
                    }
                }
        }

        static void Main()
        {
            Input();
            Prediction();
            Output();
        }

    }
}
