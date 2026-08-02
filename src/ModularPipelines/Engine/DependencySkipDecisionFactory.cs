using ModularPipelines.Models;

namespace ModularPipelines.Engine;

internal static class DependencySkipDecisionFactory
{
    private const string DependencyReasonPrefix = "Required dependency '";
    private const string DependencyReasonSeparator = " was skipped: ";

    public static SkipDecision Create(
        IReadOnlyList<(Type ModuleType, SkipDecision? SkipDecision)> skippedDependencies)
    {
        if (skippedDependencies.Count == 1)
        {
            var dependency = skippedDependencies[0];
            var dependencyReason = GetRootReason(dependency.SkipDecision?.Reason);
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

    private static string? GetRootReason(string? reason)
    {
        while (reason?.StartsWith(DependencyReasonPrefix, StringComparison.Ordinal) == true)
        {
            var separatorIndex = reason.IndexOf(
                DependencyReasonSeparator,
                DependencyReasonPrefix.Length,
                StringComparison.Ordinal);
            if (separatorIndex < 0)
            {
                break;
            }

            reason = reason[(separatorIndex + DependencyReasonSeparator.Length)..];
        }

        return reason;
    }
}
