using System.Xml.Schema;

namespace ShuntingYard_Ovcharenko;

public class Calculator 
{
    private bool IsOperator(string symbol)
    {
        return symbol is "+" or "-" or "*" or "/" or "^";
    }
    
    private bool IsFunc(string symbol)
    {
        return symbol is "max" or "sin" or "cos";
    }

    
    public MyArrayList Tokenize(string input)
    {
        
        MyArrayList arr = new MyArrayList();
        char[] inputArr = input.ToCharArray();
        foreach (char symbol in inputArr)
        {
            if (symbol == ' ')
                continue;

            if (symbol == '-' && (arr.Count == 0 || arr.GetAt(arr.Count - 1) == "("))
            {
                arr.Add("0");
                arr.Add(symbol.ToString());
                continue;
            }

            if ( char.IsLetter(symbol) && arr.Count != 0 && char.IsLetter(arr.GetAt(arr.Count - 1)[0]) )
            {
                string s;
                s = arr.GetAt(arr.Count - 1);
                arr.SetAt(arr.Count - 1, s + symbol);
                continue;
            }
            
            if ( !char.IsDigit(symbol) && !IsOperator(symbol.ToString()) && symbol!='(' 
                 && symbol!=')' && symbol != ',' && !char.IsLetter(symbol))
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


    private int Priority(string s)
    {
        return s switch
        {
            "+" => 0,
            "-" => 0,
            "*" => 1,
            "/" => 1,
            "^" => 2,
            "max" => 1,
            "sin" => 1,
            "cos" => 1,
            _ => 0
        };
    }

    
    public MyQueue ChangeNotation(MyArrayList expr)
    {
        var queue = new MyQueue();
        var stack = new MyStack();

        for (int i = 0; i < expr.Count; i++)
        {
            var token = expr.GetAt(i);
            //Console.WriteLine(token);
            if (token == ",")
            {
                while (stack.Examine()!="(" && stack.Examine()!=null)
                {
                    string tokenFromStack = stack.Pop();
                    queue.Enqueue(tokenFromStack);
                    continue;
                }
                //Console.WriteLine(stack.Examine());
                //stack.Pop();  
                continue;
            }
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
                if (IsFunc(stack.Examine()))
                {
                    string tokenFromStack = stack.Pop();
                    queue.Enqueue(tokenFromStack);
                }
                continue;
            }
            while ( (Priority(stack.Examine()) >= Priority(token) && token!="^") ||
                    (Priority(stack.Examine()) >  Priority(token) && token=="^")    )
            {
                if (stack.Examine() == "(" || stack.Examine()==null)
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

    private decimal Operation(decimal a, decimal b, string s)
    {
        return s switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => a / b,
            "^" => (int)Math.Pow((float)a, (float)b),
            "max" => Math.Max(a, b),
            "sin" => (int)Math.Sin((float)a),
            "cos" => (int)Math.Cos((float)a),
            _ => 0
        };
    }

    private bool IsUnary(string s)
    {
        return s is "sin" or "cos";
    }

    
    public decimal Calculate(MyQueue queue)
    {

        int size = queue.Count;
        string[] expr = new string[size];
        for (int i = 0; i < size; i++)
            expr[i] = queue.Dequeue();

        bool[] used = new bool[size];

        int tokenToTake = size-1; 
        decimal ResultRec(string token)
        {
            tokenToTake--;
            if (IsOperator(token) || IsFunc(token))
                if (!IsUnary(token))
                {
                    decimal secondRes = ResultRec(expr[tokenToTake]);
                    decimal firstRes = ResultRec(expr[tokenToTake]);
                    return Operation(firstRes, secondRes, token);
                }
                else
                {
                    decimal firstRes = ResultRec(expr[tokenToTake]);
                    return Operation(firstRes, 0, token);
                }

            return decimal.Parse(expr[tokenToTake + 1]);
        }

        return ResultRec(expr[tokenToTake]);

    }
}