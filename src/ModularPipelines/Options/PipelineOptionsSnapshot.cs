using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;

namespace ModularPipelines.Options;

internal sealed class PipelineOptionsSnapshot :
    IOptions<PipelineOptions>,
    IOptionsSnapshot<PipelineOptions>,
    IOptionsMonitor<PipelineOptions>
{
    public PipelineOptionsSnapshot(PipelineOptions value)
    {
        Value = value;
    }

    public PipelineOptions Value { get; }

    public PipelineOptions CurrentValue => Value;

    public PipelineOptions Get(string? name) => Value;

    public IDisposable? OnChange(Action<PipelineOptions, string?> listener)
    {
        ArgumentNullException.ThrowIfNull(listener);
        return NoOpDisposable.Instance;
    }
}
