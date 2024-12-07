using System.Numerics;

namespace AdventOfCode;

public class Day07 : BaseDay {
    public List<Test> Input { get; set; }
    public Day07() {
        Input = File.ReadAllLines(InputFilePath).Select(l => new Test(l)).ToList();
    }
    public override ValueTask<string> Solve_1() {
        BigInteger sum = 0;
        foreach (var test in Input) {
            if (test.IsValid1())
                sum += test.TestValue;
        }
        return new($"{sum}");
    }
    public override ValueTask<string> Solve_2() {
        BigInteger sum = 0;
        foreach (var test in Input) {
            if (test.IsValid2())
                sum += test.TestValue;
        }
        return new($"{sum}");
    }

    public class Test {
        public long TestValue { get; set; }
        public List<long> Numbers { get; set; }
        public Test(string line) {
            TestValue = long.Parse(line.Split(':')[0]);
            Numbers = line.Split(':')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        }
        public bool IsValid1() {
            var sums = Calc(Numbers[0], Numbers[1..]);
            return sums.Contains(TestValue);
        }
        public HashSet<BigInteger> Calc(BigInteger sum, List<long> numbers) {
            if (numbers.Count == 0)
                return [sum];
            var addSum = Calc(sum + numbers[0], numbers[1..]);
            var multSum = Calc(sum * numbers[0], numbers[1..]);
            return [..addSum, ..multSum];
        }
        public bool IsValid2() {
            var sums = Calc2(Numbers[0], Numbers[1..]);
            return sums.Contains(TestValue);
        }
        public HashSet<BigInteger> Calc2(BigInteger sum, List<long> numbers) {
            if (numbers.Count == 0)
                return [sum];
            var addSum = Calc2(sum + numbers[0], numbers[1..]);
            var multSum = Calc2(sum * numbers[0], numbers[1..]);
            var concatNum = BigInteger.Parse(sum.ToString() + numbers[0].ToString());
            var concatSum = Calc2(concatNum, numbers[1..]);
            return [..addSum, ..multSum, ..concatSum];
        }
    }
}