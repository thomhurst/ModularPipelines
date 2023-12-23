using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-space")]
public record AwsSagemakerCreateSpaceOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--space-name")] string SpaceName
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--space-settings")]
    public string? SpaceSettings { get; set; }

    [CommandSwitch("--ownership-settings")]
    public string? OwnershipSettings { get; set; }

    [CommandSwitch("--space-sharing-settings")]
    public string? SpaceSharingSettings { get; set; }

    [CommandSwitch("--space-display-name")]
    public string? SpaceDisplayName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}