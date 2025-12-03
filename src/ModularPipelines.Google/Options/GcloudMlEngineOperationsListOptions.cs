using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "operations", "list")]
public record GcloudMlEngineOperationsListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}