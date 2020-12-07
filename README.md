# AdventOfCode.Template

![CI](https://github.com/eduherminio/AdventOfCode.Template/workflows/CI/badge.svg)

Advent of Code template based on [AoCHelper](https://github.com/eduherminio/AoCHelper) project.

It allows you to focus on solving AoC puzzles while providing you with some performance stats.  

Problem example:

```csharp
using AoCHelper;
using System.IO;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        public override string Solve_1() => $"Solution 1";

        public override string Solve_2() => $"Solution 2";
    }
}
```

Output example:

![image](https://user-images.githubusercontent.com/11148519/101387503-d0071980-38be-11eb-96de-49b7893bb9ac.png)

## Basic usage

- Create one class per advent day, following `DayXX` or `Day_XX` naming convention and implementing `AoCHelper.BaseDay`.
- Place input files under `Inputs/` dir, following `XX.txt` convention.
- Read the input content from `InputFilePath` and solve the puzzle by implementing `Solve_1()` and `Solve_2()`!

**By default, only your last problem will be solved when running the project**. You can change that by behavior by modifying `Program.cs`.

Invoking **different methods**:

- `Solver.SolveAll();` → solves all the days.

- `Solver.SolveLast();` → solves only the last day.

- `Solver.Solve<Day_XX>();` → solves only day `XX`.

- `Solver.Solve(new uint[] { XX, YY });` → solves only days `XX` and `YY`.

- `Solver.Solve(new [] { typeof(Day_XX), typeof(Day_YY) });` → same as above.

Providing a **custom `SolverConfiguration`** instance to any of those methods:

- `Solver.SolveLast(new SolverConfiguration() { ClearConsole = false } );` → solves only the last day providing a custom configuration.

- `Solver.SolveAll(new SolverConfiguration() { ElapsedTimeFormatSpecifier = "F3" } );` → solves all the days providing a custom configuration.

## Advanced usage

Check [AoCHelper README file](https://github.com/eduherminio/AoCHelper#advanced-usage) for detailed information about how to override the default file naming and location conventions of your problem classes and input files.
