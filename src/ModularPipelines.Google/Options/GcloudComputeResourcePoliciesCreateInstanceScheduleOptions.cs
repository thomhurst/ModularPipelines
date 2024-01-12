using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "resource-policies", "create", "instance-schedule")]
public record GcloudComputeResourcePoliciesCreateInstanceScheduleOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--end-date")]
    public string? EndDate { get; set; }

    [CommandSwitch("--initiation-date")]
    public string? InitiationDate { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--timezone")]
    public string? Timezone { get; set; }

    [CommandSwitch("--vm-start-schedule")]
    public string? VmStartSchedule { get; set; }

    [CommandSwitch("--vm-stop-schedule")]
    public string? VmStopSchedule { get; set; }
}