class BitSet : Set
{
    private int[] bitArray;

    public BitSet()
    {
        bitArray = new int[1];
    }

    public override void AddElement(int element)
    {
        int index = element / 32;
        int bitPosition = element % 32;
        while (index >= bitArray.Length)
        {
            Array.Resize(ref bitArray, bitArray.Length + 1);
        }
        bitArray[index] |= 1 << bitPosition;
    }

    public override void RemoveElement(int element)
    {
        int index = element / 32;
        int bitPosition = element % 32;
        if (index < bitArray.Length)
        {
            bitArray[index] &= ~(1 << bitPosition);
        }
    }

    public override bool ContainsElement(int element)
    {
        int index = element / 32;
        int bitPosition = element % 32;
        if (index < bitArray.Length)
        {
            return (bitArray[index] & (1 << bitPosition)) != 0;
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
        Console.WriteLine("Elements in the BitSet: ");
        for (int i = 0; i < bitArray.Length; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                if ((bitArray[i] & (1 << j)) != 0)
                {
                    int element = i * 32 + j;
                    Console.Write(element + " ");
                }
            }
        }
        Console.WriteLine();
    }
}
