using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "describe-applications")]
public record AwsWorkspacesDescribeApplicationsOptions : AwsOptions
{
    [CliOption("--application-ids")]
    public string[]? ApplicationIds { get; set; }

    [CliOption("--compute-type-names")]
    public string[]? ComputeTypeNames { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--operating-system-names")]
    public string[]? OperatingSystemNames { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}