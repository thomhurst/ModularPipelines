using ModularPipelines.Models;

namespace ModularPipelines.Extensions;

public static class BooleanExtensions
{
    public static SkipDecision AsSkipDecisionIfTrue(this bool value, string reason)
    {
        if (value)
        {
            return SkipDecision.Skip(reason);
        }
        
        return SkipDecision.DoNotSkip;
    }
}