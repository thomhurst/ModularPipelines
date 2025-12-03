using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "subnets", "list-usable")]
public record GcloudContainerSubnetsListUsableOptions : GcloudOptions
{
    [CliOption("--network-project")]
    public string? NetworkProject { get; set; }
}