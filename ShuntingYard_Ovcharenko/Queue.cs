public class MyQueue
{
    private const int Capacity = 50;
    private string[] _array = new string[Capacity];
    private int _pointer, _pointerBegin;
    private int _count;
    
    public void Enqueue(string value)
    {
        if (_count == Capacity)
        {
            throw new Exception("Queue overflowed");
        }
        _array[_pointer] = value;
        _pointer++; _pointer %= Capacity;
        _count++;
    }
    
    public string Dequeue()
    {
        if (_count == 0)
            return null;
        var value = _array[_pointerBegin];
        _pointerBegin++; _pointerBegin %= Capacity;
        _count--;
        return value;
    }
    
    public string Examine()
    {
        if (_count == 0) return null;
        return _array[_pointerBegin];
    }
}