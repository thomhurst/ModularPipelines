using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "describe-organizational-unit")]
public record AwsOrganizationsDescribeOrganizationalUnitOptions(
[property: CliOption("--organizational-unit-id")] string OrganizationalUnitId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}