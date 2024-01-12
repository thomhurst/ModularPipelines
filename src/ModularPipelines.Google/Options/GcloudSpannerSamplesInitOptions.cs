using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "samples", "init")]
public record GcloudSpannerSamplesInitOptions(
[property: PositionalArgument] string Appname,
[property: CommandSwitch("--instance-id")] string InstanceId
) : GcloudOptions
{
    [CommandSwitch("--database-id")]
    public string? DatabaseId { get; set; }
}