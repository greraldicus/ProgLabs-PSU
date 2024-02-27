class MultiSet : Set
{
    private int[] multiSetArray;
    private int maxElement;

    public MultiSet(int maxElement)
    {
        this.maxElement = maxElement;
        multiSetArray = new int[maxElement + 1];
    }

    public override void AddElement(int element)
    {
        if (element > maxElement)
        {
            throw new SetException("Element is outside the set boundaries.");
        }

        if (element >= 0 && element <= maxElement)
        {
            multiSetArray[element]++;
        }
    }

    public override void RemoveElement(int element)
    {
        if (element >= 0 && element <= maxElement && multiSetArray[element] > 0)
        {
            multiSetArray[element]--;
        }
    }

    public override bool ContainsElement(int element)
    {
        if (element >= 0 && element <= maxElement && multiSetArray[element] > 0)
        {
            return true;
        }
        return false;
    }

    public override void FillFromString(string elements)
    {
        string[] elementStrings = elements.Split(' ');
        foreach (string elementString in elementStrings)
        {
            if (Int32.TryParse(elementString, out int element))
            {
                AddElement(element);
            }
        }
    }

    public override void FillFromArray(int[] elements)
    {
        foreach (int element in elements)
        {
            AddElement(element);
        }
    }

    public override void PrintElements()
    {
        Console.WriteLine("Elements in the MultiSet: ");
        for (int i = 0; i <= maxElement; i++)
        {
            if (multiSetArray[i] > 0)
            {
                Console.Write($"{i} (count: {multiSetArray[i]}) ");
            }
        }
        Console.WriteLine();
    }
}
