using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "delete-applications")]
public record AwsDiscoveryDeleteApplicationsOptions(
[property: CliOption("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}