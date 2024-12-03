using System.Text.RegularExpressions;

namespace AdventOfCode;

public class Day03 : BaseDay {
    private string _input { get; set; }
    private Regex _funcRegex { get; set; }

    public Day03() {
        _input = File.ReadAllText(InputFilePath);
        _funcRegex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
    }

    public override ValueTask<string> Solve_1() {
        var sum = 0;
        foreach (Match match in _funcRegex.Matches(_input))
            sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        return new($"{sum}");
    }

    public override ValueTask<string> Solve_2() {
        var input = _input;
        while (true) {
            var dontIndex = input.IndexOf("don't()");
            if (dontIndex < 0) break;
            var doIndex = input.IndexOf("do()", dontIndex);
            input = input.Remove(dontIndex, doIndex-dontIndex);
        }
        var sum = 0;
        foreach (Match match in _funcRegex.Matches(input))
            sum += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        return new($"{sum}");
    }
}