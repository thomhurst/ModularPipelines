using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "create-repository-link")]
public record AwsCodestarConnectionsCreateRepositoryLinkOptions(
[property: CliOption("--connection-arn")] string ConnectionArn,
[property: CliOption("--owner-id")] string OwnerId,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--encryption-key-arn")]
    public string? EncryptionKeyArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}