using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "create-organization")]
public record AwsOrganizationsCreateOrganizationOptions : AwsOptions
{
    [CliOption("--feature-set")]
    public string? FeatureSet { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}