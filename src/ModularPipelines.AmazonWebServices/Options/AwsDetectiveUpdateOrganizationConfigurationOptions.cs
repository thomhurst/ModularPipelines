using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("detective", "update-organization-configuration")]
public record AwsDetectiveUpdateOrganizationConfigurationOptions(
[property: CliOption("--graph-arn")] string GraphArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}