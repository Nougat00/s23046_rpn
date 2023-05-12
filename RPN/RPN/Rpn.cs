using System;
using System.Collections.Generic;

namespace RPN;

public class Rpn
{
    private readonly MyStack<int> _operators = new MyStack<int>();

    private readonly Dictionary<string, Func<int, int, int>> _operationFunction =
        new Dictionary<string, Func<int, int, int>>();

    private Dictionary<string, Func<int, int>> _oneArgOperationFunction;

    public int EvalRpn(string input)
    {
        _operationFunction["+"] = (fst, snd) => (fst + snd);
        _operationFunction["-"] = (fst, snd) => (fst - snd);
        _operationFunction["*"] = (fst, snd) => (fst * snd);
        _operationFunction["/"] = (fst, snd) =>
        {
            if (snd == 0)
            {
                throw new ArgumentException("Dzielenie przez 0 jest niedozwolone");
            }

            return fst / snd;
        };

        _oneArgOperationFunction = new Dictionary<string, Func<int, int>>
        {
            ["|"] = (arg) => (arg < 0 ? -arg : arg),
            ["!"] = (arg) =>
            {
                if (arg < 1)
                {
                    throw new ArgumentException();
                }

                int r = 1;
                for (int x = 1; x <= arg; x++)
                {
                    r *= x;
                }

                return r;
            }
        };

        var splitInput = input.Split(' ');
        
        foreach (var op in splitInput)
        {
            if (IsNumber(op))
            {
                _operators.Push(NumberParser(op));
            }
            else if (IsOperator(op))
            {
                var num1 = _operators.Pop();
                var num2 = _operators.Pop();
                _operators.Push(_operationFunction[op](num1, num2));
            }
            else if (IsOneArgumentOperator(op))
            {
                var num = _operators.Pop();
                _operators.Push(_oneArgOperationFunction[op](num));
            }
        }

        var result = _operators.Pop();
        
        if (_operators.IsEmpty)
        {
            return result;
        }

        throw new InvalidOperationException();
    }

    private int NumberParser(String input)
    {
        if (input.Length > 0 && (Char.IsDigit(input[0]) || input[0] == '-'))
        {
            return Int32.Parse(input);
        }

        if (input.Length > 1)
        {
            switch (input[0])
            {
                case 'B':
                    return Convert.ToInt32(input.Substring(1), 2);
                case 'D':
                    return Int32.Parse(input.Substring(1));
                case '#':
                    return Convert.ToInt32(input.Substring(1), 16);
            }
        }

        throw new ArgumentException();
    }

    private bool IsNumber(String input) =>
        Int32.TryParse(input, out _)
        || input.StartsWith("D") || input.StartsWith("#") || input.StartsWith("B");

    private bool IsOperator(String input) =>
        input.Equals("+") || input.Equals("-") ||
        input.Equals("*") || input.Equals("/");

    public bool IsOneArgumentOperator(String input) =>
        input == "|" || input == "!";

    private Func<int, int, int> Operation(String input) =>
        (x, y) =>
        (
            (input.Equals("+") ? x + y : (input.Equals("*") ? x * y : int.MinValue)
            )
        );
}