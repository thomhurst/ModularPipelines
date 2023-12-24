using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "update-subscription-target")]
public record AwsDatazoneUpdateSubscriptionTargetOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--identifier")] string Identifier
) : AwsOptions
{
    [CommandSwitch("--applicable-asset-types")]
    public string[]? ApplicableAssetTypes { get; set; }

    [CommandSwitch("--authorized-principals")]
    public string[]? AuthorizedPrincipals { get; set; }

    [CommandSwitch("--manage-access-role")]
    public string? ManageAccessRole { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--subscription-target-config")]
    public string[]? SubscriptionTargetConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}