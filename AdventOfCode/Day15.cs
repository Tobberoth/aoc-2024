using System.Drawing;

namespace AdventOfCode;

public class Day15 : BaseDay
{
    private List<List<char>> MapData { get; set; } = [];
    private string Dirs { get; set; } = "";
    public Day15()
    {
        var input = File.ReadAllLines(InputFilePath);
        var readDirs = false;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                readDirs = true;
                continue;
            }
            if (!readDirs)
                MapData.Add([.. line]);
            else
                Dirs += line.Trim('\n');
        }
    }

    private static void UpdateMap(List<List<char>> map, char direction)
    {
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[0].Count; x++)
            {
                if (map[y][x] == '@')
                {
                    Size dir;
                    dir = direction switch
                    {
                        '^' => new Size(0, -1),
                        '>' => new Size(1, 0),
                        'v' => new Size(0, 1),
                        '<' => new Size(-1, 0),
                        _ => throw new InvalidOperationException("Invalid direction")
                    };
                    TryMove(new Point(x, y), dir, map);
                    return;
                }
            }
        }
    }

    private static bool TryMove(Point point, Size dir, List<List<char>> ret, bool wideMode = false)
    {
        var target = point + dir;
        if (ret[target.Y][target.X] == '.')
        {
            if (!wideMode)
            {
                var oldObj = ret[point.Y][point.X];
                ret[point.Y][point.X] = '.';
                ret[target.Y][target.X] = oldObj;
            }
            return true;
        }
        else if (ret[target.Y][target.X] == '#')
        {
            return false;
        }
        else if (dir.Height != 0 && (ret[target.Y][target.X] == '[' || ret[target.Y][target.X] == ']'))
        {
            Point target2;
            if (ret[target.Y][target.X] == '[')
                target2 = new Point(target.X + 1, target.Y);
            else
                target2 = new Point(target.X - 1, target.Y);
            if (TryMove(target, dir, ret, true) && TryMove(target2, dir, ret, true))
            {
                if (!wideMode)
                {
                    TryMove(target, dir, ret);
                    TryMove(target2, dir, ret);
                    var oldObj = ret[point.Y][point.X];
                    ret[point.Y][point.X] = '.';
                    ret[target.Y][target.X] = oldObj;
                }
                return true;
            }
            return false;
        }
        else
        {
            if (TryMove(target, dir, ret))
            {
                if (!wideMode)
                {
                    var oldObj = ret[point.Y][point.X];
                    ret[point.Y][point.X] = '.';
                    ret[target.Y][target.X] = oldObj;
                }
                return true;
            }
            return false;
        }
    }

    private static long CalculateGPSCoords(List<List<char>> map)
    {
        long score = 0;
        for (var y = 0; y < map.Count; y++)
        {
            for (var x = 0; x < map[0].Count; x++)
            {
                if (map[y][x] == 'O')
                    score += (y * 100) + x;
                if (map[y][x] == '[')
                    score += (y * 100) + x;
            }
        }
        return score;
    }

    private static List<List<char>> WidenMap(List<List<char>> mapData)
    {
        List<List<char>> ret = [];
        for (var y = 0; y < mapData.Count; y++)
        {
            ret.Add([]);
            foreach (var c in mapData[y])
            {
                switch (c)
                {
                    case '#':
                        ret[y].Add('#');
                        ret[y].Add('#');
                        break;
                    case '.':
                        ret[y].Add('.');
                        ret[y].Add('.');
                        break;
                    case 'O':
                        ret[y].Add('[');
                        ret[y].Add(']');
                        break;
                    case '@':
                        ret[y].Add('@');
                        ret[y].Add('.');
                        break;
                }
            }
        }
        return ret;
    }

    private static void DrawMap(List<List<char>> mapData)
    {
        foreach (var line in mapData)
        {
            foreach (var c in line)
            {
                Console.Write(c);
            }
            Console.Write('\n');
        }
    }

    public override ValueTask<string> Solve_1()
    {
        // Copy map so the original is unedited for solve_2
        List<List<char>> mapData = [];
        foreach (var line in MapData)
            mapData.Add(new(line));

        foreach (char dir in Dirs)
        {
            UpdateMap(mapData, dir);
        }
        return new($"{CalculateGPSCoords(mapData)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var wideMap = WidenMap(MapData);
        foreach (char dir in Dirs)
        {
            UpdateMap(wideMap, dir);
        }
        return new($"{CalculateGPSCoords(wideMap)}");
    }
}