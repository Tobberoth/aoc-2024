
namespace AdventOfCode;

public class Day10: BaseDay {
    private int Height { get; set; }
    private int Width { get; set; }
    private int[,] Map { get; set; }
    public Day10() {
        var input = File.ReadAllLines(InputFilePath);
        Height = input.Length + 2;
        Width = input[0].Length + 2;
        Map = new int[Width,Height];
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (y == 0 || y == Height-1) {
                    Map[x,y] = 10;
                    continue;
                }
                if (x == 0 || x == Width - 1) {
                    Map[x, y] = 10;
                    continue;
                }
                Map[x, y] = int.Parse(input[y-1][x-1].ToString());
            }
        }
    }

    private HashSet<(int x, int y)> CalculateTailheadRank(int prevHeight, int x, int y) {
        var thisHeight = Map[x, y];
        if (thisHeight != prevHeight + 1) return [];
        if (thisHeight == 10) return [];
        if (thisHeight == 9) return [(x, y)];
        HashSet<(int x, int y)> sum = [];
        sum = [..sum, ..CalculateTailheadRank(thisHeight, x-1, y)];
        sum = [..sum, ..CalculateTailheadRank(thisHeight, x+1, y)];
        sum = [..sum, ..CalculateTailheadRank(thisHeight, x, y+1)];
        sum = [..sum, ..CalculateTailheadRank(thisHeight, x, y-1)];
        return sum;
    }

    private int CalculateTailheadRanking(int prevHeight, int x, int y) {
        var thisHeight = Map[x, y];
        if (thisHeight != prevHeight + 1) return 0;
        if (thisHeight == 10) return 0;
        if (thisHeight == 9) return 1;
        var sum = 0;
        sum += CalculateTailheadRanking(thisHeight, x-1, y);
        sum += CalculateTailheadRanking(thisHeight, x+1, y);
        sum += CalculateTailheadRanking(thisHeight, x, y+1);
        sum += CalculateTailheadRanking(thisHeight, x, y-1);
        return sum;
    }

    public override ValueTask<string> Solve_1() {
        var sum = 0;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Map[x, y] == 0) {
                    sum += CalculateTailheadRank(-1, x, y).Count;
                }
            }
        }

        return new($"{sum}");
    }

    public override ValueTask<string> Solve_2() {
        var sum = 0;
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                if (Map[x, y] == 0) {
                    sum += CalculateTailheadRanking(-1, x, y);
                }
            }
        }

        return new($"{sum}");
    }
}