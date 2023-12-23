using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "disassociate-configuration-items-from-application")]
public record AwsDiscoveryDisassociateConfigurationItemsFromApplicationOptions(
[property: CommandSwitch("--application-configuration-id")] string ApplicationConfigurationId,
[property: CommandSwitch("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}