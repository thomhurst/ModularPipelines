using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "configuration-profile-assignment", "list")]
public record AzAutomanageConfigurationProfileAssignmentListOptions : AzOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--vm-name")]
    public string? VmName { get; set; }
}