namespace AdventOfCode;

public class Day12 : BaseDay
{
    private List<Region> Regions { get; set; } = [];

    public Day12()
    {
        var input = File.ReadAllLines(InputFilePath).Select(l => l.ToList()).ToList();
        input = input.Select(l => (List<char>)['0', .. l, '0']).ToList();
        var filler = input[0].Select(c => '0').ToList();
        input = [filler, .. input, filler];
        try
        {
            while (true)
            {
                Regions.Add(new Region(input));
            }
        }
        catch (Exception)
        {
            // Done getting all regions
        }
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{Regions.Select(r => r.Price).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new($"{Regions.Select(r => r.Plots.Count * CalcSides(r)).Sum()}");
    }

    private static long CalcSides(Region r)
    {
        long sides = 0;
        HashSet<(int x, int y)> UP = [];
        HashSet<(int x, int y)> DOWN = [];
        HashSet<(int x, int y)> LEFT = [];
        HashSet<(int x, int y)> RIGHT = [];
        (int x, int y) nextPlot;
        foreach (var plot in r.Plots)
        {
            // UP
            if (!r.Plots.Contains((plot.x, plot.y - 1)))
            {
                if (!UP.Contains(plot))
                {
                    sides++;
                    UP.Add(plot);
                    nextPlot = (plot.x - 1, plot.y);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x, nextPlot.y - 1)))
                    {
                        UP.Add(nextPlot);
                        nextPlot = (nextPlot.x - 1, nextPlot.y);
                    }
                    nextPlot = (plot.x + 1, plot.y);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x, nextPlot.y - 1)))
                    {
                        UP.Add(nextPlot);
                        nextPlot = (nextPlot.x + 1, nextPlot.y);
                    }
                }
            }
            // DOWN
            if (!r.Plots.Contains((plot.x, plot.y + 1)))
            {
                if (!DOWN.Contains(plot))
                {
                    sides++;
                    DOWN.Add(plot);
                    nextPlot = (plot.x - 1, plot.y);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x, nextPlot.y + 1)))
                    {
                        DOWN.Add(nextPlot);
                        nextPlot = (nextPlot.x - 1, nextPlot.y);
                    }
                    nextPlot = (plot.x + 1, plot.y);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x, nextPlot.y + 1)))
                    {
                        DOWN.Add(nextPlot);
                        nextPlot = (nextPlot.x + 1, nextPlot.y);
                    }
                }
            }
            // LEFT
            if (!r.Plots.Contains((plot.x - 1, plot.y)))
            {
                if (!LEFT.Contains(plot))
                {
                    sides++;
                    LEFT.Add(plot);
                    nextPlot = (plot.x, plot.y - 1);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x - 1, nextPlot.y)))
                    {
                        LEFT.Add(nextPlot);
                        nextPlot = (nextPlot.x, nextPlot.y - 1);
                    }
                    nextPlot = (plot.x, plot.y + 1);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x - 1, nextPlot.y)))
                    {
                        LEFT.Add(nextPlot);
                        nextPlot = (nextPlot.x, nextPlot.y + 1);
                    }
                }
            }
            // RIGHT
            if (!r.Plots.Contains((plot.x + 1, plot.y)))
            {
                if (!RIGHT.Contains(plot))
                {
                    sides++;
                    RIGHT.Add(plot);
                    nextPlot = (plot.x, plot.y - 1);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x + 1, nextPlot.y)))
                    {
                        RIGHT.Add(nextPlot);
                        nextPlot = (nextPlot.x, nextPlot.y - 1);
                    }
                    nextPlot = (plot.x, plot.y + 1);
                    while (r.Plots.Contains(nextPlot) && !r.Plots.Contains((nextPlot.x + 1, nextPlot.y)))
                    {
                        RIGHT.Add(nextPlot);
                        nextPlot = (nextPlot.x, nextPlot.y + 1);
                    }
                }
            }
        }
        return sides;
    }

    public class Region
    {
        public char Flower { get; set; } = '1';
        public HashSet<(int x, int y)> Plots { get; set; } = [];
        public int Perimeter { get; set; } = 0;
        public long Price => Plots.Count * Perimeter;

        public Region(List<List<char>> map)
        {
            for (var y = 0; y < map.Count; y++)
            {
                for (var x = 0; x < map[0].Count; x++)
                {
                    if (map[y][x] == '0') continue;
                    Flower = map[y][x];
                    Expand(map, [(x, y)]);
                    return;
                }
            }
            if (Flower == '1')
                throw new InvalidOperationException("Out of flowers");
        }
        private void Expand(List<List<char>> map, HashSet<(int x, int y)> positions)
        {
            HashSet<(int x, int y)> nextPoints = [];
            foreach (var pos in positions)
            {
                if (Plots.Contains(pos)) continue;
                if (map[pos.y][pos.x] == Flower)
                {
                    Plots.Add(pos);
                    map[pos.y][pos.x] = '0';
                }

                if (!Plots.Contains((pos.x, pos.y - 1)))
                {
                    if (map[pos.y - 1][pos.x] == Flower)
                        nextPoints.Add((pos.x, pos.y - 1));
                    else Perimeter++;
                }

                if (!Plots.Contains((pos.x, pos.y + 1)))
                {
                    if (map[pos.y + 1][pos.x] == Flower)
                        nextPoints.Add((pos.x, pos.y + 1));
                    else Perimeter++;
                }

                if (!Plots.Contains((pos.x - 1, pos.y)))
                {
                    if (map[pos.y][pos.x - 1] == Flower)
                        nextPoints.Add((pos.x - 1, pos.y));
                    else Perimeter++;
                }

                if (!Plots.Contains((pos.x + 1, pos.y)))
                {
                    if (map[pos.y][pos.x + 1] == Flower)
                        nextPoints.Add((pos.x + 1, pos.y));
                    else Perimeter++;
                }
            }
            if (nextPoints.Count > 0)
                Expand(map, nextPoints);
        }
    }
}