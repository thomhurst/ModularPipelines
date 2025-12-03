using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "tags", "holds", "delete")]
public record GcloudResourceManagerTagsHoldsDeleteOptions(
[property: CliArgument] string TagHoldName
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}