using Pavlychev.Mathematics;

(string Equation, double Result)[] tests =
{
    ("2+3*4/5-6^2", -31.6),
    ("-2+3*4/5-6^2", -35.6),
    ("2+3*4/5+(-6)^2", 40.4),
    ("2+3*4/5+(-6)*2", -7.6),
    ("2^3^2", 512),
    ("2+2*2", 6),
    ("2+3^4", 83),
    ("2^3+4", 12),
    ("5-2*(3+4)+(5+6)", 2),
    ("(3+4)^2", 49),
    ("-1*(3+4)^2", -49),
    ("-1*(3+4*(3+5))^2", -1225),
};

var allTests = true;
var testNum = 0;

if (allTests)
{
    for (int i = testNum; i < tests.Length; i++)
        Test(tests[i], ref testNum);

} else Test(tests[testNum], ref testNum);

Environment.Exit(0);


void Test((string Equation, double Result) test, ref int testNum)
{
    int a = 0;
    Thread thread = new(testNum =>
    {
        int tn = (int)(testNum ?? 0);
        try
        {
            var result = Problem.SolveFromString(test.Equation);
            Console.WriteLine($"TEST {++tn}: {(result == test.Result ? "Passed" : "Failed")}! ({test.Equation} = {test.Result} {(result == test.Result ? "=" : "≠")} {result})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TEST {tn}: Threw! ({ex.GetType()})");
        }

        a = tn;
    });

    thread.Start(testNum);

    DateTime started = DateTime.Now;

    while (thread.IsAlive)
    {
        if (DateTime.Now - started < new TimeSpan(0, 0, 2)) continue;

        a = testNum + 1;

        thread.Interrupt();
        Console.WriteLine($"TEST {a}: Timed Out! ({test.Equation} = {test.Result} = …?");
        break;
    }

    testNum = a;
}