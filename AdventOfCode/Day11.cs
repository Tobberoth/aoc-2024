namespace AdventOfCode;

public class Day11: BaseDay {
    private List<long> Stones { get; set; }
    public Day11() {
        Stones = File.ReadAllText(InputFilePath).Split(' ').Select(long.Parse).ToList();
    }
    public override ValueTask<string> Solve_1() =>
        new($"{CalcStones(25)}");
    public override ValueTask<string> Solve_2() =>
        new($"{CalcStones(75)}");


    private long CalcStones(int blinks) =>
        Stones.Select(s => CalcAmount(0, s, blinks)).Sum();
    private Dictionary<(int count, long stoneValue, int maxCount), long> PreCalced = [];
    private long CalcAmount(int count, long stoneValue, int maxCount) {
        if (PreCalced.TryGetValue((count,stoneValue,maxCount), out long ret))
            return ret;
        long val = 0;
        if (count == maxCount)
            val = 1;
        else if (stoneValue == 0) {
            val = CalcAmount(count+1, 1, maxCount);
        } else if (stoneValue.ToString().Length % 2 == 0) {
            var stringRep = stoneValue.ToString();
            long sum = 0;
            sum += CalcAmount(count+1, long.Parse(stringRep[0..(stringRep.Length/2)]), maxCount);
            sum += CalcAmount(count+1, long.Parse(stringRep[(stringRep.Length/2)..]), maxCount);
            val = sum;
        } else
            val = CalcAmount(count+1, stoneValue * 2024, maxCount);
        PreCalced[(count,stoneValue,maxCount)] = val;
        return val;
    }
}