using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-access-entry")]
public record AwsEksUpdateAccessEntryOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--kubernetes-groups")]
    public string[]? KubernetesGroups { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}