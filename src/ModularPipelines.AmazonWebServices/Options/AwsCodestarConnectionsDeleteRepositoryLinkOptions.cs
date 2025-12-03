using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "delete-repository-link")]
public record AwsCodestarConnectionsDeleteRepositoryLinkOptions(
[property: CliOption("--repository-link-id")] string RepositoryLinkId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}