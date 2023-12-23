using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-subscription-target")]
public record AwsDatazoneCreateSubscriptionTargetOptions(
[property: CommandSwitch("--applicable-asset-types")] string[] ApplicableAssetTypes,
[property: CommandSwitch("--authorized-principals")] string[] AuthorizedPrincipals,
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--environment-identifier")] string EnvironmentIdentifier,
[property: CommandSwitch("--manage-access-role")] string ManageAccessRole,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subscription-target-config")] string[] SubscriptionTargetConfig,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}