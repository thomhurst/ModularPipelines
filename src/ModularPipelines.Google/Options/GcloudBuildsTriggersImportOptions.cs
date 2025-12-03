using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "import")]
public record GcloudBuildsTriggersImportOptions(
[property: CliOption("--source")] string Source
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}