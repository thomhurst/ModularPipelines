using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "associate-configuration-items-to-application")]
public record AwsDiscoveryAssociateConfigurationItemsToApplicationOptions(
[property: CliOption("--application-configuration-id")] string ApplicationConfigurationId,
[property: CliOption("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}