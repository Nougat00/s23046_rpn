using System;
using System.Collections.Generic;

namespace RPN;

public class Rpn
{
    private readonly MyStack<int> _operators= new MyStack<int>();
    private readonly Dictionary<string, Func<int, int, int>> _operationFunction = new Dictionary<string, Func<int, int, int>>();

    public int EvalRpn(string input)
    {
        _operationFunction["+"] = (fst, snd) => (fst + snd);
        _operationFunction["-"] = (fst, snd) => (fst - snd);
        _operationFunction["*"] = (fst, snd) => (fst * snd);
        _operationFunction["/"] = (fst, snd) => (fst / snd);

        var splitInput = input.Split(' ');
        foreach (var op in splitInput)
        {
            if (IsNumber(op))
                _operators.Push(Int32.Parse(op));
            else
            if (IsOperator(op))
            {
                var num1 = _operators.Pop();
                var num2 = _operators.Pop();
                _operators.Push(_operationFunction[op](num1, num2));
                //_operators.Push(Operation(op)(num1, num2));
            }
        }

        var result = _operators.Pop();
        if (_operators.IsEmpty)
        {
            return result;
        }
        throw new InvalidOperationException();
    }

    private bool IsNumber(String input) => Int32.TryParse(input, out _);

    private bool IsOperator(String input) =>
        input.Equals("+") || input.Equals("-") ||
        input.Equals("*") || input.Equals("/");

    private Func<int, int, int> Operation(String input) =>
        (x, y) =>
        (
            (input.Equals("+") ? x + y :
                (input.Equals("*") ? x * y : int.MinValue)
            )
        );
}