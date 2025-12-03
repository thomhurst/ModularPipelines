using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("filestore", "operations", "cancel")]
public record GcloudFilestoreOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}