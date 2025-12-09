using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-pod-identity-association")]
public record AwsEksCreatePodIdentityAssociationOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}