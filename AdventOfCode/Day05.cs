namespace AdventOfCode;

public class Day05 : BaseDay {
    
    private List<ValidationRule> ValidationRules { get; set; } = [];
    private List<List<int>> Updates { get; set; } = [];

    public Day05() {
        var _input = File.ReadAllLines(InputFilePath);

        foreach (var line in _input) {
            if (line.Contains('|')) {
                ValidationRules.Add(new ValidationRule {
                    Before = int.Parse(line.Split('|')[0]), 
                    After = int.Parse(line.Split('|')[1])
                });
                continue;
            }
            if (string.IsNullOrWhiteSpace(line)) continue;
            Updates.Add(line.Split(',').Select(int.Parse).ToList());
        }
    }
    public override ValueTask<string> Solve_1() {
        var sum = Updates.Where(IsValid).Sum(GetMidPage);
        return new($"{sum}");
    }

    private bool IsValid(List<int> update) {
        for (var i = 0; i < update.Count; i++) {
            var mustBeAfter = ValidationRules.Where(r => r.Before == update[i]).Select(r => r.After).ToList();
            for (var j = 0; j < i; j++) {
                if (mustBeAfter.Contains(update[j]))
                    return false;
            }
            var mustBeBefore = ValidationRules.Where(r => r.After == update[i]).Select(r => r.Before).ToList();
            for (var j = i+1; j < update.Count; j++) {
                if (mustBeBefore.Contains(update[j]))
                    return false;
            }
        }
        return true;
    }

    public override ValueTask<string> Solve_2() {
        var sum = 0;
        foreach (var update in Updates) {
            if (!IsValid(update)) {
                var fixedUpdate = FixOrder(update);
                sum += GetMidPage(fixedUpdate);
            }
        }
        return new($"{sum}");
    }

    private List<int> FixOrder(List<int> update) {
        var initialLength = update.Count;
        List<int> newOrder = [];
        while (newOrder.Count < initialLength) {
            foreach (var num in update) {
                var mustBeAfter = ValidationRules.Where(r => r.After == num && update.Contains(r.Before)).Select(r => r.Before).ToList();
                if (mustBeAfter.Count == 0) {
                    update.Remove(num);
                    newOrder.Add(num);
                    break;
                }
            }
        }
        return newOrder;
    }

    private static int GetMidPage(List<int> update) {
        int midIndex = (int)Math.Floor(update.Count / 2.0);
        return update[midIndex];
    }

    public class ValidationRule {
        public int Before { get; set; }
        public int After { get; set; }

    }
}