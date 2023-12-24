using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "update-addon")]
public record AwsEksUpdateAddonOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--addon-name")] string AddonName
) : AwsOptions
{
    [CommandSwitch("--addon-version")]
    public string? AddonVersion { get; set; }

    [CommandSwitch("--service-account-role-arn")]
    public string? ServiceAccountRoleArn { get; set; }

    [CommandSwitch("--resolve-conflicts")]
    public string? ResolveConflicts { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--configuration-values")]
    public string? ConfigurationValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}