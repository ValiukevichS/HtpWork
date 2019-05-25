using System;

namespace DaysOfTheWeek
{
    class Program
    {
        static void Main(string[] args)
        {
            int date;
            Console.WriteLine("Input number");
            date = Convert.ToInt32(Console.ReadLine());
            switch(date)
            {
                case 1:
                    Console.WriteLine("Mondey");
                    break;
                case 2:
                    Console.WriteLine("Tuesday");
                    break;
                case 3:
                    Console.WriteLine("Wednesday");
                    break;
                case 4:
                    Console.WriteLine("Wednesday");
                    break;
                case 5:
                    Console.WriteLine("Friday");
                    break;
                case 6:
                    Console.WriteLine("Saturday");
                    break;
                case 7:
                    Console.WriteLine("Sunday");
                    break;
                default:                    
                    Console.WriteLine("Enter from 1 to 7");
                    break;
            }
        }
    }
}
