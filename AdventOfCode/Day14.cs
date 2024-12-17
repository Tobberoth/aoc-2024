using System.Text;

namespace AdventOfCode;

public class Day14 : BaseDay
{
    private List<Robot> Robots { get; set; } = []; 
    private int MAP_WIDTH { get; set; }
    private int MAP_HEIGHT { get; set; }

    public Day14()
    {
        MAP_WIDTH = 101;
        MAP_HEIGHT = 103;
        var lines = File.ReadAllLines(InputFilePath);
        foreach (var line in lines)
        {
            var data = line.Split(' ');
            var posData = data[0][2..].Split(',');
            var velData = data[1][2..].Split(',');
            Robots.Add(new Robot {
                StartX = int.Parse(posData[0]),
                StartY = int.Parse(posData[1]),
                VelocityX = int.Parse(velData[0]),
                VelocityY = int.Parse(velData[1])
            });
        }
    }

    public override ValueTask<string> Solve_1()
    {
        List<(int x, int y)> EndPositions = [];
        foreach (var robot in Robots)
            EndPositions.Add(robot.GetEndPos(100, MAP_WIDTH, MAP_HEIGHT));
        var safetyFactor = CalucateSafetyFactor(EndPositions);
        return new($"{safetyFactor}");
    }

    private int CalucateSafetyFactor(List<(int x, int y)> endPositions)
    {
        int quad1 = 0, quad2 = 0, quad3 = 0, quad4 = 0;
        var midY = MAP_HEIGHT / 2;
        var midX = MAP_WIDTH / 2;
        foreach (var endPos in endPositions)
        {
            if (endPos.x == midX || endPos.y == midY)
                continue;
            if (endPos.x < midX && endPos.y < midY)
                quad1++;
            else if (endPos.x > midX && endPos.y < midY)
                quad2++;
            else if (endPos.x < midX && endPos.y > midY)
                quad3++;
            else if (endPos.x > midX && endPos.y > midY)
                quad4++;
        }
        return quad1 * quad2 * quad3 * quad4;
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, int> safetyScores = [];
        for (var i = 0; i < 10402; i++)
        {
            var endPositions = Robots.Select(r => r.GetEndPos(i, MAP_WIDTH, MAP_HEIGHT));
            safetyScores.Add(i, CalucateSafetyFactor(endPositions.ToList()));
        }
        var ordered = safetyScores.OrderBy(s => s.Value).ToList();
        return new($"Most likely {ordered[0].Key} or {ordered[1].Key}");
    }

    private void DrawBoard(IEnumerable<(int x, int y)> endPositions)
    {
        StringBuilder sb = new();
        for (var y = 0; y < MAP_HEIGHT; y++) {
            for (var x = 0; x < MAP_WIDTH; x++) {
                var count = endPositions.Count(e => e.x == x && e.y == y);
                if (count > 0)
                    sb.Append('X');
                else
                    sb.Append('.');
            }
            sb.Append('\n');
        }
        Console.Clear();
        Console.Write(sb.ToString());
    }

    private record Robot
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }

        public (int x, int y) GetEndPos(int steps, int map_width, int map_height)
        {
            var endX = (StartX + VelocityX * steps) % map_width;
            if (endX < 0)
                endX = map_width + endX;
            var endY = (StartY + VelocityY * steps) % map_height;
            if (endY < 0)
                endY = map_height + endY;
            return (endX, endY);
        }
    }
}