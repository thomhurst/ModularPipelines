using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "instances", "update")]
public record GcloudBigtableInstancesUpdateOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}