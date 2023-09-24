namespace ModularPipelines.Models;

public sealed class SkipDecision
{
    public bool ShouldSkip { get; }
    public string? Reason { get; private init; }

    private SkipDecision(bool shouldSkip)
    {
        ShouldSkip = shouldSkip;
    }

    public static readonly SkipDecision DoNotSkip = new SkipDecision(false);

    public static SkipDecision Skip(string? reason) => new SkipDecision(true)
    {
        Reason = reason
    };
    
    public static implicit operator SkipDecision(bool shouldSkip) => shouldSkip ? Skip(null) : DoNotSkip;
    
    public static implicit operator SkipDecision(string reason) => Skip(reason);
}