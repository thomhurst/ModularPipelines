using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-organization-conformance-pack")]
public record AwsConfigserviceDeleteOrganizationConformancePackOptions(
[property: CliOption("--organization-conformance-pack-name")] string OrganizationConformancePackName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}