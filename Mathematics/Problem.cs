using System.Globalization;
using System.Text;
using Pavlychev.Mathematics.Operators;

namespace Pavlychev.Mathematics;

/// <summary>
/// Класс, решающий простые математические примеры.
/// </summary>
public static class Problem
{
    static List<char> NumberContents => "0123456789-.".ToList();
    static (List<char> OpeningBrackets, List<char> ClosingBrackets) Brackets => ("([{".ToList(), ")]}".ToList());

    /// <summary>
    /// Решить пример из строки.
    /// </summary>
    /// <param name="equation">Пример без пробелов.</param>
    /// <returns></returns>
    public static double SolveFromString(string equation)
    {
        equation = equation.Replace("+-", "-");
        equation = equation.Replace("-", "+-");
        equation = equation.Replace(",", ".");

        if (equation[0] == '+') equation = "0" + equation;

        int? brPos = null, brEnd = null;
        do {
            brPos = null; brEnd = null;

            for (int i = 0; i < equation.Length; i++)
            {
                if (Brackets.OpeningBrackets.Exists(x => x == equation[i])) brPos = i;

                if (Brackets.ClosingBrackets.Exists(x => x == equation[i]) && brPos.HasValue)
                {
                    brEnd = i;
                    break;
                }
            }

            if (!brPos.HasValue || !brEnd.HasValue) continue;

            var newEq = equation.Substring(brPos.Value + 1, brEnd.Value - brPos.Value - 1);
            var res = SolveFromString(newEq);

            if (brEnd.Value + 2 < equation.Length && equation[brEnd.Value + 1] == '^')
            {
                string _b = "";
                int expEnd;
                for (expEnd = brEnd.Value + 2; expEnd < equation.Length; expEnd++)
                    _b += equation[expEnd];

                var bop = BinaryOperator.GetBinaryOperator('^', res, double.Parse(_b));

                if (bop != null)
                {
                    var str = equation.Substring(brPos.Value, expEnd - brPos.Value);

                    equation = equation.Replace(str, bop.Solve().ToString());

                    if (double.TryParse(equation, out var _result)) return _result;

                    continue;
                }
            }

            equation = equation.Replace(equation.Substring(brPos.Value, brEnd.Value - brPos.Value + 1), res.ToString());
        } while (brPos.HasValue || brEnd.HasValue);

        double result = 0;

        int bEnd = 0;

        StringBuilder sb = new(equation);

        for (int i = sb.Length - 1; i >= 0; i--)
        {
            bEnd = sb.Length - 1;
            for (int j = sb.Length - 1; j >= 0; j--)
            {
                bool solved =
                    CheckAndSolve(sb, j, bEnd, '^', "", ref result) ||

                    CheckAndSolve(sb, j, bEnd, '*', "^", ref result) ||
                    CheckAndSolve(sb, j, bEnd, '/', "^", ref result) ||

                    CheckAndSolve(sb, j, bEnd, '+', "*/^–", ref result) ||
                    CheckAndSolve(sb, j, bEnd, '–', "*/^", ref result);

                if (solved) break;

                if (!NumberContents.Exists(x => x == sb[j])) bEnd = j - 1;
            }
        }

        return result;
    }

    private static BinaryOperator? Parse(StringBuilder sb, int j, int bEnd, char op)
    {
        int bPos = j + 1;

        int aPos = bPos - 2, aEnd = bPos - 2;

        for (int i = bPos - 2; i >= 0; i--)
        {
            if (NumberContents.Exists(x => x == sb[i]) && !(op == '^' && sb[i] == '-')) continue;

            aPos = i + 1;
            break;
        }

        int aLength = aEnd - aPos + 1;
        int bLength = bEnd - bPos + 1;

        string
            _a = sb.ToString().Substring(aPos, aLength),
            _b = sb.ToString().Substring(bPos, bLength);

        double
            a = double.Parse(aLength == 0 ? "0" : _a, NumberStyles.Any, CultureInfo.InvariantCulture),
            b = double.Parse(_b, NumberStyles.Any, CultureInfo.InvariantCulture);

        var bop = BinaryOperator.GetBinaryOperator(op, a, b);

        if (bop == null) throw new Exception();

        sb = sb.Replace($"{_a}{op}{_b}", $"{bop.Solve()}", aPos, bEnd - aPos + 1);

        return bop;
    }

    private static bool CheckAndSolve(StringBuilder sb, int j, int bEnd, char opChar, string uppedOpChars, ref double pre)
    {
        sb = sb.Replace(',', '.');
        if (sb[j] != opChar || sb.ToString().ToList().Exists(x => uppedOpChars.ToList().Exists(y => x == y))) return false;

        var bop = Parse(sb, j, bEnd, opChar);

        if (bop == null) throw new Exception();

        pre = bop.Solve();
        return true;
    }
}
