using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "yum", "import")]
public record GcloudArtifactsYumImportOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-source")] string[] GcsSource
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}