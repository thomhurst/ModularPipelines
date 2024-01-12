using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "apt", "upload")]
public record GcloudArtifactsAptUploadOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--source")] string Source
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}