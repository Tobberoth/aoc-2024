namespace AdventOfCode;

public class Day13 : BaseDay
{
    private List<ClawMachine> ClawMachines { get; set; } = [];
    public Day13()
    {
        var input = File.ReadAllLines(InputFilePath);
        for (var i = 0; i < input.Length; i++)
        {
            ClawMachine clawMachine = new();
            var data = input[i].Split(':')[1].Trim().Split(", ");
            clawMachine.A = (int.Parse(data[0].Split('+')[1]), int.Parse(data[1].Split('+')[1]));
            data = input[++i].Split(':')[1].Trim().Split(", ");
            clawMachine.B = (int.Parse(data[0].Split('+')[1]), int.Parse(data[1].Split('+')[1]));
            data = input[++i].Split(':')[1].Trim().Split(", ");
            clawMachine.Prize = (int.Parse(data[0].Split('=')[1]), int.Parse(data[1].Split('=')[1]));
            i++; // Skip empty
            ClawMachines.Add(clawMachine);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        return new($"{ClawMachines.Select(c => c.CalculateTokens()).Sum()}");
    }

    public override ValueTask<string> Solve_2()
    {
        ClawMachines.ForEach(c => c.Prize = (c.Prize.x + 10000000000000, c.Prize.y + 10000000000000));
        return new($"{ClawMachines.Select(c => c.SolveTokens()).Sum()}");
    }

    public class ClawMachine
    {
        public (int x, int y) A { get; set; }
        public (int x, int y) B { get; set; }
        public (long x, long y) Prize { get; set; }

        // Calculates the y=mx+b form of the lines representing A and B presses to get to the target X,Y coordinates
        // then finds the intersection, which is the correct amount of A and B presses.
        public long SolveTokens()
        {
            // Line 1
            var a = (double)A.x;
            var b = (double)B.x;
            var c = (double)-Prize.x;
            var m = -(a / b);
            var bb = -(c / b);
            // Line 2
            var a2 = (double)A.y;
            var b2 = (double)B.y;
            var c2 = (double)-Prize.y;
            var m2 = -(a2 / b2);
            var bb2 = -(c2 / b2);
            // Clac
            var m_diff = (m - m2);
            var intersect_x = (bb2 - bb) / m_diff;
            var intersect_y = m * intersect_x + bb;
            // Need exact matches for button presses, this makes sure don't round away significant mismatches
            if (Math.Abs(Math.Round(intersect_x) - intersect_x) < 0.01f && Math.Abs(Math.Round(intersect_y) - intersect_y) < 0.01f)
            {
                var lix = (long)Math.Round(intersect_x);
                var liy = (long)Math.Round(intersect_y);
                if (lix < 0 || liy < 0) return 0;
                return lix * 3 + liy;
            }
            return 0;
        }

        public int CalculateTokens()
        {
            var lowestToken = 100_000;
            for (var a = 0; a < 100; a++)
            {
                for (var b = 0; b < 100; b++)
                {
                    var aPos = (x: A.x * a, y: A.y * a);
                    var bPos = (x: B.x * b, y: B.y * b);
                    if (Prize == (aPos.x + bPos.x, aPos.y + bPos.y))
                    {
                        var token = a * 3 + b;
                        if (token < lowestToken)
                            lowestToken = token;
                    }
                }
            }
            return lowestToken == 100_000 ? 0 : lowestToken;
        }
    }
}