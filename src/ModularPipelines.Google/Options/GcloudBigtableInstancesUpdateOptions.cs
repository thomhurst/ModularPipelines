using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "instances", "update")]
public record GcloudBigtableInstancesUpdateOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}