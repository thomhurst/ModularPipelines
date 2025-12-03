using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "create-repository")]
public record AwsProtonCreateRepositoryOptions(
[property: CliOption("--connection-arn")] string ConnectionArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--provider")] string Provider
) : AwsOptions
{
    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}