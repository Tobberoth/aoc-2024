namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day02() {
        var lines = File.ReadAllLines(InputFilePath);
        _input = lines.Select(l => l.Split(' ').Select(int.Parse).ToList()).ToList();
    }

    private static bool IsSetValid(List<int> set) {
        bool? isIncreasing = null;
        for (var i = 0; i < set.Count-1; i++) {
            var diff = set[i] - set[i+1];
            if (isIncreasing.HasValue && isIncreasing == true && diff > 0)
                return false;
            if (isIncreasing.HasValue && isIncreasing == false && diff < 0)
                return false;
            if (isIncreasing == null)
                isIncreasing = diff < 0;
            if (diff == 0)
                return false;
            if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
                return false;
        }
        return true;
    }

    public override ValueTask<string> Solve_1() {
        var validReports = 0;
        foreach (var levelSet in _input) {
            if (IsSetValid(levelSet))
                validReports++;
        }
        return new($"{validReports}");
    }

    public override ValueTask<string> Solve_2() {
        var validReports = 0;
        foreach (var levelSet in _input) {
            if (IsSetValid(levelSet))
                validReports++;
            else {
                for (var i = 0; i < levelSet.Count; i++) {
                    if (IsSetValid(levelSet.Where((v, j) => j != i).ToList())) {
                        validReports++;
                        break;
                    }

                }
            }
        }
        return new($"{validReports}");
    }
}