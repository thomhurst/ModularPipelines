using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "configuration-profile-assignment", "vm", "report", "show")]
public record AzAutomanageConfigurationProfileAssignmentVmReportShowOptions : AzOptions
{
    [CliOption("--assignment-name")]
    public string? AssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}