using System;
namespace Pavlychev.Mathematics.Operators;

class BinaryOperator
{
    public double FirstOperand;
    public double SecondOperand;

    public BinaryOperator(double a, double b)
    {
        FirstOperand = a;
        SecondOperand = b;
    }

    public static BinaryOperator? GetBinaryOperator(char oper, double a, double b)
    {
        if (oper == '+') return new PlusOperator(a, b);
        if (oper == '–') return new MinusOperator(a, b);
        if (oper == '*') return new MultiplicationOperator(a, b);
        if (oper == '/') return new DivisionOperator(a, b);
        if (oper == '^') return new PowerOperator(a, b);

        return null;
    }

    public virtual double Solve() => throw new NotImplementedException();
}

class PlusOperator : BinaryOperator
{
    public PlusOperator(double a, double b) : base(a, b) { }

    public override double Solve() => FirstOperand + SecondOperand;
}

class MinusOperator : BinaryOperator
{
    public MinusOperator(double a, double b) : base(a, b) { }

    public override double Solve() => FirstOperand - SecondOperand;
}

class MultiplicationOperator : BinaryOperator
{
    public MultiplicationOperator(double a, double b) : base(a, b) { }

    public override double Solve() => FirstOperand * SecondOperand;
}

class DivisionOperator : BinaryOperator
{
    public DivisionOperator(double a, double b) : base(a, b) { }

    public override double Solve() => FirstOperand / SecondOperand;
}

class PowerOperator : BinaryOperator
{
    public PowerOperator(double a, double b) : base(a, b) { }

    public override double Solve() => Math.Pow(FirstOperand, SecondOperand);
}

