using ShuntingYard_Ovcharenko;

string input;
while (true)
{
    input = Console.ReadLine();
    if (input == "")
        break;
    var calc = new Calculator();
    var ans = calc.Tokenize(input); 
    //for (int i=0; i<ans.Count; i++)
    //    Console.WriteLine(ans.GetAt(i));
     
    var q = calc.ChangeNotation(ans);
    //for (int i=0; i<q.Count; i++)
    //    Console.WriteLine(q.GetAt(i));

    var bareResult = calc.Calculate(q);
    Console.WriteLine(Math.Round(bareResult, 4));   
}

 