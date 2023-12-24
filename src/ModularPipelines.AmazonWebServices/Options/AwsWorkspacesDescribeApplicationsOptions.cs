using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "describe-applications")]
public record AwsWorkspacesDescribeApplicationsOptions : AwsOptions
{
    [CommandSwitch("--application-ids")]
    public string[]? ApplicationIds { get; set; }

    [CommandSwitch("--compute-type-names")]
    public string[]? ComputeTypeNames { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--operating-system-names")]
    public string[]? OperatingSystemNames { get; set; }

    [CommandSwitch("--owner")]
    public string? Owner { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}