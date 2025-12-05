using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies", "create", "instance-schedule")]
public record GcloudComputeResourcePoliciesCreateInstanceScheduleOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--end-date")]
    public string? EndDate { get; set; }

    [CliOption("--initiation-date")]
    public string? InitiationDate { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--timezone")]
    public string? Timezone { get; set; }

    [CliOption("--vm-start-schedule")]
    public string? VmStartSchedule { get; set; }

    [CliOption("--vm-stop-schedule")]
    public string? VmStopSchedule { get; set; }
}