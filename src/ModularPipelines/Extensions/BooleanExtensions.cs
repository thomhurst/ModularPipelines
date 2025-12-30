using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

/// <summary>
/// Provides extension methods for <see cref="bool"/> values.
/// </summary>
public static class BooleanExtensions
{
    /// <summary>
    /// Converts a boolean value to a <see cref="SkipDecision"/>.
    /// If the value is <c>true</c>, returns a skip decision with the specified reason.
    /// If the value is <c>false</c>, returns <see cref="SkipDecision.DoNotSkip"/>.
    /// </summary>
    /// <param name="value">The boolean value to evaluate.</param>
    /// <param name="reason">The reason for skipping if the value is <c>true</c>.</param>
    /// <returns>A <see cref="SkipDecision"/> based on the boolean value.</returns>
    public static SkipDecision AsSkipDecisionIfTrue(this bool value, string reason)
    {
        if (value)
        {
            return SkipDecision.Skip(reason);
        }

        return SkipDecision.DoNotSkip;
    }
}