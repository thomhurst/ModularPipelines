using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-access-entry")]
public record AwsEksCreateAccessEntryOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--kubernetes-groups")]
    public string[]? KubernetesGroups { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}