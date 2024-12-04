
namespace AdventOfCode;

public class Day04 : BaseDay {
    private List<string> _input;

    public Day04() {
        _input = File.ReadAllLines(InputFilePath).Select(l => "OOO" + l + "OOO").ToList();
        var fillerString = "";
        foreach (var letter in _input[0])
            fillerString += "O";
        _input = _input.Prepend(fillerString).ToList();
        _input = _input.Prepend(fillerString).ToList();
        _input = _input.Prepend(fillerString).ToList();
        _input = _input.Append(fillerString).ToList();
        _input = _input.Append(fillerString).ToList();
        _input = _input.Append(fillerString).ToList();
    }

    public override ValueTask<string> Solve_1() {
        var total = 0;
        for (var y = 0; y < _input.Count; y++) {
            for (var x = 0; x < _input[0].Length; x++) {
                if (_input[y][x] == 'X') {
                    total += CountXMAS(y, x);
                }
            }
        }
        return new($"{total}");
    }

    private int CountXMAS(int y, int x)
    {
        var sum = 0;
        // Check each direction
        // Right, Left, Up, Down
        if (_input[y][x..].StartsWith("XMAS"))
            sum++;
        if (_input[y][..x].EndsWith("SAM"))
            sum++;
        if ($"{_input[y-1][x]}{_input[y-2][x]}{_input[y-3][x]}" == "MAS")
            sum++;
        if ($"{_input[y+1][x]}{_input[y+2][x]}{_input[y+3][x]}" == "MAS")
            sum++;
        // NE,SE,SW,NW
        if ($"{_input[y-1][x+1]}{_input[y-2][x+2]}{_input[y-3][x+3]}" == "MAS")
            sum++;
        if ($"{_input[y+1][x+1]}{_input[y+2][x+2]}{_input[y+3][x+3]}" == "MAS")
            sum++;
        if ($"{_input[y-1][x-1]}{_input[y-2][x-2]}{_input[y-3][x-3]}" == "MAS")
            sum++;
        if ($"{_input[y+1][x-1]}{_input[y+2][x-2]}{_input[y+3][x-3]}" == "MAS")
            sum++;
        return sum;
    }

    private bool IsX_MAS(int y, int x) {
        if (_input[y][x] != 'A') return false;

        var correctLegs = 0;
        if (_input[y-1][x-1] == 'M') {
            if (_input[y+1][x+1] == 'S')
                correctLegs++;
        } else if (_input[y-1][x-1] == 'S') {
            if (_input[y+1][x+1] == 'M')
                correctLegs++;
        }

        if (_input[y-1][x+1] == 'M') {
            if (_input[y+1][x-1] == 'S')
                correctLegs++;
        } else if (_input[y-1][x+1] == 'S') {
            if (_input[y+1][x-1] == 'M')
                correctLegs++;
        }

        return correctLegs == 2;
    }

    public override ValueTask<string> Solve_2() {
        var total = 0;
        for (var y = 0; y < _input.Count; y++) {
            for (var x = 0; x < _input[0].Length; x++) {
                if (IsX_MAS(y, x))
                    total++;
            }
        }
        return new($"{total}");
    }
}