using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-access-entry")]
public record AwsEksCreateAccessEntryOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CommandSwitch("--kubernetes-groups")]
    public string[]? KubernetesGroups { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}