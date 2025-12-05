using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "delete-organizational-unit")]
public record AwsOrganizationsDeleteOrganizationalUnitOptions(
[property: CliOption("--organizational-unit-id")] string OrganizationalUnitId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}