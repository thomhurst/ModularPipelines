using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "operations", "wait")]
public record GcloudMlEngineOperationsWaitOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}