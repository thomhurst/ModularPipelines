using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "update-repository-link")]
public record AwsCodestarConnectionsUpdateRepositoryLinkOptions(
[property: CliOption("--repository-link-id")] string RepositoryLinkId
) : AwsOptions
{
    [CliOption("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CliOption("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}