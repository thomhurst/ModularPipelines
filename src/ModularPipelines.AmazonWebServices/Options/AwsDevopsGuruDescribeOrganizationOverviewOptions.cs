using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "describe-organization-overview")]
public record AwsDevopsGuruDescribeOrganizationOverviewOptions(
[property: CliOption("--from-time")] long FromTime
) : AwsOptions
{
    [CliOption("--to-time")]
    public long? ToTime { get; set; }

    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}