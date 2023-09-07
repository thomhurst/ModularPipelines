using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Models;

public record PipelineSummary
{
    public IReadOnlyList<ModuleBase> Modules { get; }
    public TimeSpan TotalDuration { get; }
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    internal PipelineSummary(IReadOnlyList<ModuleBase> modules, 
        TimeSpan totalDuration, 
        DateTimeOffset start,
        DateTimeOffset end)
    {
        Modules = modules;
        TotalDuration = totalDuration;
        Start = start;
        End = end;
    }

    public Status Status => GetStatus();

    private Status GetStatus()
    {
        if (Modules.Any(x => x.Status == Status.Failed))
        {
            return Status.Failed;
        }
        
        if (Modules.Any(x => x.Status == Status.Unknown))
        {
            return Status.Unknown;
        }
        
        if (Modules.Any(x => x.Status == Status.Processing))
        {
            return Status.Processing;
        }
        
        if (Modules.Any(x => x.Status == Status.NotYetStarted))
        {
            return Status.Failed;
        }
        
        if (Modules.Any(x => x.Status == Status.TimedOut))
        {
            return Status.Failed;
        }

        return Status.Successful;
    }
}