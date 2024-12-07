namespace AdventOfCode;

public class Day06 : BaseDay {
    private List<string> Map { get; set; }
    private (int x, int y) InitialGuardPos { get; set; }
    private GuardNPC Guard { get; set; }
    private HashSet<(int x, int y, DIRECTION dir)> VisitedPositions { get; set; } = [];

    public Day06() {
        Map = File.ReadAllLines(InputFilePath).ToList();
        for (var y = 0; y < Map.Count; y++) {
            for (var x = 0; x < Map[0].Length; x++) {
                if (Map[y][x] == '^') {
                    InitialGuardPos = (x, y);
                    Guard = new() { Pos = InitialGuardPos, Direction = DIRECTION.UP };
                    return;
                }
            }
        }
    }

    public override ValueTask<string> Solve_1() {
        while (IsPosInMap(Guard.Pos, Map))
            UpdateGuard(VisitedPositions, Map);
        return new($"{VisitedPositions.Select(p => (p.x, p.y)).ToHashSet().Count}");
    }

    private bool UpdateGuard(HashSet<(int x, int y, DIRECTION dir)> visitedPositions, List<string> map) {
        if (!visitedPositions.Add((Guard.Pos.x, Guard.Pos.y, Guard.Direction)))
            return true;
        (int x, int y) newPos = Guard.Direction switch {
            DIRECTION.UP => (Guard.Pos.x, Guard.Pos.y-1),
            DIRECTION.DOWN => (Guard.Pos.x, Guard.Pos.y+1),
            DIRECTION.RIGHT => (Guard.Pos.x+1, Guard.Pos.y),
            DIRECTION.LEFT => (Guard.Pos.x-1, Guard.Pos.y),
            _ => throw new InvalidOperationException("Invalid direction")
        };
        if (IsPosInMap(newPos, map)) {
            while (map[newPos.y][newPos.x] == '#') {
                var newEnum = ((int)Guard.Direction + 1) % 4;
                Guard.Direction = (DIRECTION)newEnum;
                newPos = Guard.Direction switch {
                    DIRECTION.UP => (Guard.Pos.x, Guard.Pos.y-1),
                    DIRECTION.DOWN => (Guard.Pos.x, Guard.Pos.y+1),
                    DIRECTION.RIGHT => (Guard.Pos.x+1, Guard.Pos.y),
                    DIRECTION.LEFT => (Guard.Pos.x-1, Guard.Pos.y),
                    _ => throw new InvalidOperationException("Invalid direction")
                };
                if (!visitedPositions.Add((Guard.Pos.x, Guard.Pos.y, Guard.Direction)))
                    return true;
            }
        }
        Guard.Pos = newPos;
        return false;
    }

    private bool IsPosInMap((int x, int y) pos, List<string> map) {
        if (pos.x < 0 || pos.x > map[0].Length - 1)
            return false;
        if (pos.y < 0 || pos.y > map.Count - 1)
            return false;
        return true;
    }

    public override ValueTask<string> Solve_2() {
        var sum = 0;
        HashSet<(int x, int y)> JustPoses = VisitedPositions.Select(p => (x: p.x, y: p.y)).ToHashSet();
        foreach (var pos in JustPoses) {
            Guard = new() { Pos = InitialGuardPos, Direction = DIRECTION.UP };
            List<string> newMap = new(Map);
            newMap[pos.y] = newMap[pos.y].Remove(pos.x, 1);
            newMap[pos.y] = newMap[pos.y].Insert(pos.x, "#");
            HashSet<(int x, int y, DIRECTION dir)> LocalPositions = [];
            while (IsPosInMap(Guard.Pos, newMap)) {
                if (UpdateGuard(LocalPositions, newMap)) {
                    sum++;
                    break;
                }
            }
        }
        return new($"{sum}");
    }

    public class GuardNPC {
        public (int x, int y) Pos { get; set; }
        public DIRECTION Direction { get; set; }
    }

    public enum DIRECTION { UP, RIGHT, DOWN, LEFT }
}