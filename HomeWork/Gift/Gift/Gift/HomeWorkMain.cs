using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeWork
{
    public static class HomeWorkMain
    {
        public static int Main(params string[] p)
        {
            Console.WriteLine("Есть карамельные конфеты,\n" +
                "Шоколадные\n" +
                "Конфеты из горького шеколада\n" +
                "Желотиновые конфеты\n" +
                "Турецкие сладости");


            var caramelCount = GetUIntValue("Количестко карамельных конфет");
            var cololadeCount = GetUIntValue("Количестко шоколадных конфет");
            var bitCololadeCount = GetUIntValue("Количестко конфет из горького шеколада");
            var jellyCount = GetUIntValue("Количестко Желотиновых конфет");
            var turkyCount = GetUIntValue("Количестко конфет Турецкие сладости");


            var gift = new List<Candy>();

            for(int i = 0; i < caramelCount; i++)
            {
                gift.Add(new CaramelCandy());
            }

            for (int i = 0; i < cololadeCount; i++)
            {
                gift.Add(new ChocolateCandy());
            }

            for (int i = 0; i < bitCololadeCount; i++)
            {
                gift.Add(new HotCandy());
            }

            for (int i = 0; i < jellyCount; i++)
            {
                gift.Add(new JellyCandy());
            }

            for (int i = 0; i < turkyCount; i++)
            {
                gift.Add(new MadeInTurkyCandy());
            }

            WriteGift(gift);

            Console.WriteLine($"Текущий вес подарка: {gift.Sum(x => x.TotalWeight)}г");
            Console.WriteLine();

            Console.WriteLine("Делем сортировку по содержанию сахара");

            gift = gift.OrderBy(x => x.SugarWeight).ToList();

            WriteGift(gift);

            Console.WriteLine();

            Console.WriteLine("Поиск по содержанию сахара: ");
            var min = GetUIntValue("Введите нижнюю границу диопозона сахора");
            var max = GetUIntValue("Введите верхнюю границу диопозона сахора");

            var findCandy = gift.FirstOrDefault(x => x.SugarWeight < max && x.SugarWeight > min);

            if(findCandy == null)
            {
                Console.WriteLine("Конфета с заданныим значениями не найдена");
            }
            else
            {
                Console.WriteLine($"Конфета найдена : {findCandy}");
            }
                

            Console.ReadLine();
            return 0;
        }

       private static uint GetUIntValue(string nameVar)
        {
            do
            {
                Console.Write(nameVar + ": ");
                var str = Console.ReadLine();
                uint value = 0;
                if (uint.TryParse(str, out value))
                {
                    return value;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Введено не число!");
                Console.ForegroundColor = ConsoleColor.Gray;

            } while (true);
        }

        private static void WriteGift(List<Candy> gift)
        {
            Console.WriteLine("/////////////////////////////////gift Start");
            foreach (var candy in gift)
            {
                Console.WriteLine(candy);
            }
            Console.WriteLine("/////////////////////////////////gift End");
        }

    }
}
