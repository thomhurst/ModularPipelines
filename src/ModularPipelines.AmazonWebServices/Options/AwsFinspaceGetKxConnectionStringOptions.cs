using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("finspace", "get-kx-connection-string")]
public record AwsFinspaceGetKxConnectionStringOptions(
[property: CliOption("--user-arn")] string UserArn,
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--cluster-name")] string ClusterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}