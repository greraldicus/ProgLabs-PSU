
// Класс SimpleSet, наследующий от класса Set
public class SimpleSet : Set
{
    private bool[] set;
    private int maxElement;

    // Конструктор с 1 параметром – максимальным числом, представимым в множестве
    public SimpleSet(int maxElement)
    {
        this.maxElement = maxElement;
        set = new bool[maxElement + 1];
    }

    // Метод добавления элемента в множество
    public override void AddElement(int element)
    {
        if (element > maxElement)
        {
            throw new SetException("Element is outside the set boundaries.");
        }

        if (element >= 0 && element <= maxElement)
        {
            set[element] = true;
        }

    }


    // Метод исключения элемента из множества
    public override void RemoveElement(int element)
    {
        if (element >= 0 && element <= maxElement)
        {
            set[element] = false;
        }
        else
        {
            Console.WriteLine("Ошибка: элемент выходит за пределы множества.");
        }
    }

    // Метод проверки наличия элемента в множестве
    public override bool ContainsElement(int element)
    {
        if (element >= 0 && element <= maxElement)
        {
            return set[element];
        }
        else
        {
            Console.WriteLine("Ошибка: элемент выходит за пределы множества.");
            return false;
        }
    }

    // Метод заполнения множества из строки элементов
    public override void FillFromString(string elements)
    {
        string[] strArray = elements.Split(',', ';', ' ');

        foreach (string elementStr in strArray)
        {
            if (int.TryParse(elementStr, out int element))
            {
                AddElement(element);
            }
            else
            {
                throw new SetException($"Invalid element: {elementStr}");
            }
        }
    }

    // Метод заполнения множества из массива элементов
    public override void FillFromArray(int[] elements)
    {
        foreach (int element in elements)
        {
            AddElement(element);
        }
    }

    // Метод вывода всех элементов множества на экран
    public override void PrintElements()
    {
        Console.Write("Элементы множества: ");
        for (int i = 0; i <= maxElement; i++)
        {
            if (set[i])
            {
                Console.Write(i + " ");
            }
        }
        Console.WriteLine();
    }

    // Операторный метод "+", выполняющий объединение множеств
    public static SimpleSet operator +(SimpleSet set1, SimpleSet set2)
    {
        SimpleSet result = new SimpleSet(Math.Max(set1.maxElement, set2.maxElement));
        for (int i = 0; i <= result.maxElement; i++)
        {
            if (set1.ContainsElement(i) || set2.ContainsElement(i))
            {
                result.AddElement(i);
            }
        }
        return result;
    }

    public static SimpleSet operator *(SimpleSet set1, SimpleSet set2)
    {
        SimpleSet result = new SimpleSet(Math.Max(set1.maxElement, set2.maxElement));
        for (int i = 0; i <= result.maxElement; i++)
        {
            if (set1.ContainsElement(i) && set2.ContainsElement(i))
            {
                result.AddElement(i);
            }
        }
        return result;
    }
}
