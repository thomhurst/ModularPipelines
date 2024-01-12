using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "subnets", "list-usable")]
public record GcloudContainerSubnetsListUsableOptions : GcloudOptions
{
    [CommandSwitch("--network-project")]
    public string? NetworkProject { get; set; }
}