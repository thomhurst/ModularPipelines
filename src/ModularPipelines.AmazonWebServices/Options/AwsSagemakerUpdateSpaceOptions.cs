using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-space")]
public record AwsSagemakerUpdateSpaceOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--space-name")] string SpaceName
) : AwsOptions
{
    [CommandSwitch("--space-settings")]
    public string? SpaceSettings { get; set; }

    [CommandSwitch("--space-display-name")]
    public string? SpaceDisplayName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}