using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "update-subscription-grant-status")]
public record AwsDatazoneUpdateSubscriptionGrantStatusOptions(
[property: CommandSwitch("--asset-identifier")] string AssetIdentifier,
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--failure-cause")]
    public string? FailureCause { get; set; }

    [CommandSwitch("--target-name")]
    public string? TargetName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}