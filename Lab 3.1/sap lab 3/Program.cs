
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

using System;
using System.Collections.Generic;





class SetException : Exception
{
    public SetException(string message) : base(message)
    {
    }
}
internal class Program




{
    static void Main(string[] args)
    {


        Set set = null;

        while (true)
        {
            Console.WriteLine("Выберите вариант представления:");
            Console.WriteLine("1. Логический массив (SimpleSet)");
            Console.WriteLine("2. Битовый массив (BitSet)");
            Console.WriteLine("3. Мультимножество (MultiSet)");
            Console.WriteLine("4. Выход");

            int choice = ReadIntInput("Введите номер варианта: ");

            switch (choice)
            {
                case 1:
                    int maxElement = ReadIntInput("Введите максимальное число в множестве: ");
                    set = new SimpleSet(maxElement);
                    break;
                case 2:
                    set = new BitSet();
                    break;
                case 3:
                    int MaxElement = ReadIntInput("Введите максимальное число в мультимножестве: ");
                    set = new MultiSet(MaxElement);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неправильный выбор. Попробуйте еще раз.");
                    continue;
            }

            break;
        }

        while (true)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Добавить элементы из строки");
            Console.WriteLine("3. Загрузить элементы из файла");
            Console.WriteLine("4. Исключить элемент");
            Console.WriteLine("5. Проверить наличие элемента");
            Console.WriteLine("6. Вывести все элементы");
            Console.WriteLine("7. Выход");

            int choice = ReadIntInput("Введите номер действия: ");

            int element;
            string elementsString;
            string fileName;
            int[] elementsArray;

            switch (choice)
            {
                case 1:
                    element = ReadIntInput("Введите элемент: ");
                    try
                    {
                        set.AddElement(element);
                        Console.WriteLine($"Элемент {element} успешно добавлен в множество.");
                    }
                    catch (SetException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;
                case 2:
                    Console.WriteLine("Введите элементы через пробел: ");
                    try
                    {
                        elementsString = Console.ReadLine();
                        set.FillFromString(elementsString);
                        Console.WriteLine($"Элементы {elementsString} успешно добавлены в множество.");
                    }
                    catch (SetException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;
                case 3:
                    Console.WriteLine("Введите имя файла: ");
                    fileName = Console.ReadLine();
                    try
                    {
                        elementsArray = File.ReadAllLines(fileName).Select(int.Parse).ToArray();
                        set.FillFromArray(elementsArray);
                        Console.WriteLine($"Элементы из файла {fileName} успешно добавлены в множество.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;
                case 4:
                    element = ReadIntInput("Введите элемент: ");
                    set.RemoveElement(element);
                    Console.WriteLine($"Элемент {element} успешно исключен из множества.");
                    break;
                case 5:
                    element = ReadIntInput("Введите элемент: ");
                    if (set.ContainsElement(element))
                    {
                        Console.WriteLine($"Элемент {element} содержится в множестве.");
                    }
                    else
                    {
                        Console.WriteLine($"Элемент {element} не содержится в множестве.");
                    }
                    break;
                case 6:
                    set.PrintElements();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неправильный выбор. Попробуйте еще раз.");
                    break;
            }
        }
    }

        static int ReadIntInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            string[] values = input.Split(' ');

            if (int.TryParse(values[0], out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Неправильный формат числа. Попробуйте еще раз.");
            }
        }
    }

    //SimpleSet ss = new SimpleSet(7);
    //SimpleSet sss = new SimpleSet(55);
    //int[] baby = new int[4] { 1, 2, 3, 9 };
    //ss.FillFromString("1 5 5 6 6 4");
    //sss.FillFromString("44 55 8 6 9 1 2 3 4");
    //ss.PrintElements();
    //sss.PrintElements();
    //SimpleSet ssss = ss * sss;
    //ssss.PrintElements();
    //MultiSet ms = new MultiSet(7);
    //ms.FillFromString("1 1 1 1 2 2 2 3 3 4");
    //ms.AddElement(8);
    //ms.PrintElements();
    static int[] ReadIntArrayInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            string[] stringArray = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int[] intArray = new int[stringArray.Length];
            bool validInput = true;

            for (int i = 0; i < stringArray.Length; i++)
            {
                if (!int.TryParse(stringArray[i], out intArray[i]))
                {
                    Console.WriteLine("Неправильный формат числа. Попробуйте еще раз.");
                    validInput = false;
                    break;
                }
            }

            if (validInput)
            {
                return intArray;
            }
        }
    }

}
