using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "disassociate-configuration-items-from-application")]
public record AwsDiscoveryDisassociateConfigurationItemsFromApplicationOptions(
[property: CliOption("--application-configuration-id")] string ApplicationConfigurationId,
[property: CliOption("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}