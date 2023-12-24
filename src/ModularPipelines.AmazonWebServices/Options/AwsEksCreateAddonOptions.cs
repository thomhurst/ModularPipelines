using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-addon")]
public record AwsEksCreateAddonOptions(
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

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--configuration-values")]
    public string? ConfigurationValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}