using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "apt", "import")]
public record GcloudArtifactsAptImportOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-source")] string[] GcsSource
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}