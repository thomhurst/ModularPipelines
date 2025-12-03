using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml-engine", "operations", "cancel")]
public record GcloudMlEngineOperationsCancelOptions(
[property: CliArgument] string Operation
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}