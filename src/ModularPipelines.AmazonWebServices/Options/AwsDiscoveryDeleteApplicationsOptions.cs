using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "delete-applications")]
public record AwsDiscoveryDeleteApplicationsOptions(
[property: CommandSwitch("--configuration-ids")] string[] ConfigurationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}