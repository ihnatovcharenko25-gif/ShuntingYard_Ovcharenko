namespace ShuntingYard_Ovcharenko;

public class MyArrayList
{
    private string[] _array = new string[10];
    private int _pointer = 0;
    
    public void Add(string element)
    {
        _array[_pointer] = element;
        _pointer += 1;
        if (_pointer == _array.Length)
        {
            var extendedArray = new string[_array.Length * 2];
            for (var i = 0; i < _array.Length; i++)
            {
                extendedArray[i] = _array[i];
            }
            _array = extendedArray;
            //this also can be achieved via
            //Array.Resize(ref _array, _array.Length * 2);
        }
    }
    
    public void Remove(string element)
    {
        for (var i = 0; i < _pointer; i++)
        {
            if (_array[i] == element)
            {
                for (var j = i; j < _pointer - 1; j++)
                {
                    _array[j] = _array[j + 1];
                }
                _pointer -= 1;
                return;
            }
        }
    }
    
    public string GetAt(int index)
    {
        return _array[index];
    }
    
    public void SetAt(int index, string value)
    {
        if (index >= 0 && index < _pointer)
            _array[index] = value;
    }
    
    public int IndexOf(string element)
    {
        for (var i = 0; i < _pointer; i++)
        {
            if (_array[i] == element)
            {
                return i;
            }
        }
        return -1;
    }
    
    public bool Contains(string element)
    {
        return IndexOf(element) != -1;
    }

    public int Count => _pointer;
}
