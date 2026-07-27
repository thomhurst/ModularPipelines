using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal static class DependencySkipDecisionFactory
{
    public static SkipDecision Create(
        IReadOnlyList<(Type ModuleType, SkipDecision? SkipDecision)> skippedDependencies)
    {
        if (skippedDependencies.Count == 1)
        {
            var dependency = skippedDependencies[0];
            var dependencyReason = dependency.SkipDecision?.Reason;
            var reasonSuffix = string.IsNullOrWhiteSpace(dependencyReason)
                ? string.Empty
                : $": {dependencyReason}";
            return SkipDecision.Skip(
                $"Required dependency '{dependency.ModuleType.Name}' was skipped{reasonSuffix}");
        }

        var dependencyNames = string.Join(
            ", ",
            skippedDependencies.Select(dependency => $"'{dependency.ModuleType.Name}'"));
        return SkipDecision.Skip($"Required dependencies {dependencyNames} were skipped");
    }
}
