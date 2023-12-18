using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "account", "login")]
public record AzBatchAccountLoginOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--shared-key-auth")]
    public bool? SharedKeyAuth { get; set; }

    [BooleanCommandSwitch("--show")]
    public bool? Show { get; set; }
}