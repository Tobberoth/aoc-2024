namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    private readonly List<int> A = [];
    private readonly List<int> B = [];

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
        foreach (var part in _input.Split('\n')) {
            var parts = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            A.Add(int.Parse(parts[0]));
            B.Add(int.Parse(parts[1]));
        }
        A.Sort();
        B.Sort();
    }

    public override ValueTask<string> Solve_1() {
        var diffSum = A.Zip(B, (a, b) => Math.Abs(a - b)).Sum();
        return new ValueTask<string>($"{diffSum}");
    }

    public override ValueTask<string> Solve_2() {
        long bigInt = 0;
        foreach (var num in A)
            bigInt += num * B.Where(b => b == num).Count();
        return new($"{bigInt}");
    }
}
