using System.Xml.Schema;

namespace ShuntingYard_Ovcharenko;

public partial class Calculator 
{
    private bool IsOperator(string symbol)
    {
        return symbol == "+" || symbol == "-" || symbol == "*" || symbol == "/" || symbol == "^";
    }
    
    private bool IsFunc(string symbol)
    {
        return symbol == "max" || symbol == "sin" || symbol == "cos";
    }

    
    public MyArrayList Tokenize(string input)
    {
        
        MyArrayList arr = new MyArrayList();
        char[] inputArr = input.ToCharArray();
        foreach (char symbol in inputArr)
        {
            if (symbol == ' ')
                continue;
            
            if (!char.IsDigit(symbol) && !IsOperator(symbol.ToString()) && symbol!='(' && symbol!=')')
                throw new Exception($"Invalid symbol in input: {symbol}");
            
            if (char.IsDigit(symbol) && arr.Count != 0 && char.IsDigit(arr.GetAt(arr.Count - 1)[0]))
            {
                string s;
                s = arr.GetAt(arr.Count - 1);
                arr.SetAt(arr.Count - 1, s + symbol);
                continue;
            }
            
            arr.Add(symbol.ToString());
        }

        return arr;
    }

    
    private readonly Dictionary<string, int> _priority = new Dictionary<string, int>()
    {
        {"+", 0},
        {"-", 0},
        {"*", 1},
        {"/", 1},
        {"^", 2},
        {"(", 3},
        {")", 3}
    };
    private int Priority(string? token)
    {
        if (token == null) return -1;
        return _priority[token];
    }

    
    public MyQueue ChangeNotation(MyArrayList expr)
    {
        var queue = new MyQueue();
        var stack = new MyStack();

        for (int i = 0; i < expr.Count; i++)
        {
            var token = expr.GetAt(i);
            //Console.WriteLine(token);
            if (char.IsDigit(token[0]))
            {
                queue.Enqueue(token);
                continue;
            }
            /**/if (token == "(")
            {
                stack.Push(token);
                continue;
            }
            if (token == ")")
            {
                //for (int j=0; i<queue.Count; i++)
                //    Console.WriteLine(queue.GetAt(j));
                while (stack.Examine()!="(" && stack.Examine()!=null)
                {
                    string tokenFromStack = stack.Pop();
                    queue.Enqueue(tokenFromStack);
                }
                //Console.WriteLine(stack.Examine());
                stack.Pop();  
                continue;
            }
            while ( (Priority(stack.Examine()) >= Priority(token) && token!="^") ||
                    (Priority(stack.Examine()) >  Priority(token) && token=="^")    )
            {
                if (stack.Examine() == "(")
                    break;
                string tokenFromStack = stack.Pop();
                queue.Enqueue(tokenFromStack);
            }
            stack.Push(token);
        }

        while (stack.Examine() != null)
        {
            queue.Enqueue(stack.Pop());
            //Console.WriteLine(queue.Examine());
        }
            

        return queue;
    }

    private int Operation(int a, int b, string s)
    {
        switch (s)
        {
            case "+": return a + b;
            case "-": return a - b;
            case "*": return a * b;
            case "/": return a / b;
            case "^": return (int)Math.Pow(a, b);
            default: return 0;
        }
    }

    
    public int Calculate(MyQueue queue)
    {

        int size = queue.Count;
        string[] expr = new string[size];
        for (int i = 0; i < size; i++)
            expr[i] = queue.Dequeue();

        bool[] used = new bool[size];

        int tokenToTake = size-1; 
        int ResultRec(string token)
        {
            tokenToTake--;
            if (IsOperator(token))
            {
                int secondRes = ResultRec(expr[tokenToTake]);
                int firstRes = ResultRec(expr[tokenToTake]);
                return Operation(firstRes, secondRes, token);
            }

            return int.Parse(expr[tokenToTake + 1]);
        }

        return ResultRec(expr[tokenToTake]);

    }
}