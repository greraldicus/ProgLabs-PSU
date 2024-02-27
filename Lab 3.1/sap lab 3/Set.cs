public abstract class Set
{
    // Абстрактные методы
    public abstract void AddElement(int element);
    public abstract void RemoveElement(int element);
    public abstract bool ContainsElement(int element);

    // Реализованные методы
    public abstract void FillFromString(string elements);
    public abstract void FillFromArray(int[] elements);
    public abstract void PrintElements();
}
