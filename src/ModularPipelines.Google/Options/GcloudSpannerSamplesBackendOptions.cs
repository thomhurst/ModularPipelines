using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "samples", "backend")]
public record GcloudSpannerSamplesBackendOptions(
[property: PositionalArgument] string Appname,
[property: CommandSwitch("--instance-id")] string InstanceId
) : GcloudOptions
{
    [CommandSwitch("--database-id")]
    public string? DatabaseId { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }
}