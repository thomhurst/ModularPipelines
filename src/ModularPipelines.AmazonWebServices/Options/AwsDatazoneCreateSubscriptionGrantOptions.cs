using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-subscription-grant")]
public record AwsDatazoneCreateSubscriptionGrantOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--granted-entity")] string GrantedEntity,
[property: CommandSwitch("--subscription-target-identifier")] string SubscriptionTargetIdentifier
) : AwsOptions
{
    [CommandSwitch("--asset-target-names")]
    public string[]? AssetTargetNames { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}