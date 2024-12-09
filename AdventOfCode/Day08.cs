using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode;

public class Day08: BaseDay {
    private int MapWidth { get; set; }
    private int MapHeight { get; set; }
    private Dictionary<char, List<Pos>> Antennas { get; set; } = [];

    public Day08() {
        var input = File.ReadAllLines(InputFilePath);
        MapWidth = input[0].Length;
        MapHeight = input.Length;
        for (var y = 0; y < MapHeight; y++) {
            for (var x = 0; x < MapWidth; x++) {
                if (input[y][x] == '.')
                    continue;
                if (!Antennas.ContainsKey(input[y][x]))
                    Antennas[input[y][x]] = [];
                Antennas[input[y][x]].Add(new Pos { X = x, Y = y });
            }
        }
    }

    public override ValueTask<string> Solve_1() {
        HashSet<Pos> antinodes = [];
        foreach (var antennaFreq in Antennas.Keys) {
            List<Pos> antennas = [..Antennas[antennaFreq]];
            // Find each antinode compared to all other antennas of this freq
            // and add to antinodes
            while (antennas.Count > 1) {
                var currentAntenna = antennas.First();
                antennas.Remove(currentAntenna);
                foreach (var antennaLeft in antennas) {
                    foreach (var antinode in GetAntinodes1(currentAntenna, antennaLeft))
                        antinodes.Add(antinode);
                }
            }
        }
        return new($"{antinodes.Count}");
    }

    private List<Pos> GetAntinodes1(Pos currentAntenna, Pos antennaLeft) {
        var diffX = antennaLeft.X - currentAntenna.X;
        var diffY = antennaLeft.Y - currentAntenna.Y;
        var pos1 = new Pos { X = currentAntenna.X + (-diffX), Y = currentAntenna.Y + (-diffY) };
        var pos2 = new Pos { X = antennaLeft.X + diffX, Y = antennaLeft.Y + diffY };
        List<Pos> ret = [];
        if (pos1.X > -1 && pos1.X < MapWidth && pos1.Y > -1 && pos1.Y < MapHeight)
            ret.Add(pos1);
        if (pos2.X > -1 && pos2.X < MapWidth && pos2.Y > -1 && pos2.Y < MapHeight)
            ret.Add(pos2);
        return ret;
    }

    private List<Pos> GetAntinodes2(Pos currentAntenna, Pos antennaLeft) {
        List<Pos> ret = [];
        var diffX = antennaLeft.X - currentAntenna.X;
        var diffY = antennaLeft.Y - currentAntenna.Y;
        var pos1 = currentAntenna;
        while (pos1.X > -1 && pos1.X < MapWidth && pos1.Y > -1 && pos1.Y < MapHeight) {
            ret.Add(pos1);
            pos1 = new Pos { X = pos1.X + (-diffX), Y = pos1.Y + (-diffY) };
        }
        var pos2 = antennaLeft;
        while (pos2.X > -1 && pos2.X < MapWidth && pos2.Y > -1 && pos2.Y < MapHeight) {
            ret.Add(pos2);
            pos2 = new Pos { X = pos2.X + diffX, Y = pos2.Y + diffY };
        }
        return ret;
    }

    public override ValueTask<string> Solve_2() {
        HashSet<Pos> antinodes = [];
        foreach (var antennaFreq in Antennas.Keys) {
            List<Pos> antennas = [..Antennas[antennaFreq]];
            while (antennas.Count > 1) {
                var currentAntenna = antennas.First();
                antennas.Remove(currentAntenna);
                foreach (var antennaLeft in antennas) {
                    foreach (var antinode in GetAntinodes2(currentAntenna, antennaLeft))
                        antinodes.Add(antinode);
                }
            }
        }
        return new($"{antinodes.Count}");
    }

    public record Pos {
        public int X { get; set; }
        public int Y { get; set; }
    }
}