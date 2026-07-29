namespace ModularPipelines.Engine;

internal static class DependencyCycleDetector
{
    public static IReadOnlyList<Type>? FindCycle<TDependencies>(
        IReadOnlyDictionary<Type, TDependencies> graph)
        where TDependencies : IEnumerable<Type>
    {
        var colors = graph.Keys.ToDictionary(type => type, _ => VisitColor.White);
        var currentPath = new List<Type>();

        foreach (var start in graph.Keys)
        {
            if (colors[start] != VisitColor.White)
            {
                continue;
            }

            var cycle = FindCycleFrom(start, graph, colors, currentPath);
            if (cycle is not null)
            {
                return cycle;
            }
        }

        return null;
    }

    private static List<Type>? FindCycleFrom<TDependencies>(
        Type start,
        IReadOnlyDictionary<Type, TDependencies> graph,
        Dictionary<Type, VisitColor> colors,
        List<Type> currentPath)
        where TDependencies : IEnumerable<Type>
    {
        var stack = new Stack<(Type Module, IEnumerator<Type> Dependencies)>();
        colors[start] = VisitColor.Gray;
        currentPath.Add(start);
        stack.Push((start, graph[start].GetEnumerator()));

        while (stack.Count > 0)
        {
            var (module, dependencies) = stack.Pop();
            if (!dependencies.MoveNext())
            {
                dependencies.Dispose();
                colors[module] = VisitColor.Black;
                currentPath.RemoveAt(currentPath.Count - 1);
                continue;
            }

            var dependency = dependencies.Current;
            stack.Push((module, dependencies));

            if (!colors.TryGetValue(dependency, out var color))
            {
                continue;
            }

            if (color == VisitColor.Gray)
            {
                DisposeEnumerators(stack);
                var cycleStartIndex = currentPath.IndexOf(dependency);
                var cycle = currentPath.GetRange(
                    cycleStartIndex,
                    currentPath.Count - cycleStartIndex);
                cycle.Add(dependency);
                return cycle;
            }

            if (color == VisitColor.White)
            {
                colors[dependency] = VisitColor.Gray;
                currentPath.Add(dependency);
                stack.Push((dependency, graph[dependency].GetEnumerator()));
            }
        }

        return null;
    }

    private static void DisposeEnumerators(
        Stack<(Type Module, IEnumerator<Type> Dependencies)> stack)
    {
        foreach (var (_, dependencies) in stack)
        {
            dependencies.Dispose();
        }
    }

    private enum VisitColor
    {
        White,
        Gray,
        Black,
    }
}
