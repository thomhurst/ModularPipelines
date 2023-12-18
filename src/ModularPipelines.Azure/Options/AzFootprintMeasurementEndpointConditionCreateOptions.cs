using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("footprint", "measurement-endpoint-condition", "create")]
public record AzFootprintMeasurementEndpointConditionCreateOptions(
[property: CommandSwitch("--constant")] string Constant,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operator")] string Operator,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--variable")] string Variable
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

