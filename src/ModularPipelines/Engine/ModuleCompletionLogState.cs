using System.Collections;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine;

internal sealed class ModuleCompletionLogState : IReadOnlyList<KeyValuePair<string, object?>>
{
    private const string FormatWithDuration =
        "Module {ModuleName} completed after {DurationText} with lock keys: {Keys} (Active: Q={Queued}, E={Executing})";
    private const string FormatWithoutDuration =
        "Module {ModuleName} completed with lock keys: {Keys} (Active: Q={Queued}, E={Executing})";

    private readonly KeyValuePair<string, object?>[] _properties;

    public ModuleCompletionLogState(
        string moduleName,
        TimeSpan? duration,
        string lockKeys,
        int queued,
        int executing)
    {
        var durationText = duration?.ToDisplayString();
        var durationClause = durationText is null ? string.Empty : $" after {durationText}";
        Message = $"Module {moduleName} completed{durationClause} with lock keys: {lockKeys} (Active: Q={queued}, E={executing})";

        var properties = new List<KeyValuePair<string, object?>>
        {
            new("ModuleName", moduleName),
            new("Keys", lockKeys),
            new("Queued", queued),
            new("Executing", executing),
        };

        if (duration.HasValue)
        {
            properties.Add(new("DurationText", durationText));
            properties.Add(new("Duration", duration.Value));
            properties.Add(new("ExecutionTime", duration.Value.TotalMilliseconds));
        }

        properties.Add(new("{OriginalFormat}", duration.HasValue ? FormatWithDuration : FormatWithoutDuration));
        _properties = properties.ToArray();
    }

    public string Message { get; }

    public int Count => _properties.Length;

    public KeyValuePair<string, object?> this[int index] => _properties[index];

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
        ((IEnumerable<KeyValuePair<string, object?>>) _properties).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();
}
