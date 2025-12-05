using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeartifact", "create-domain")]
public record AwsCodeartifactCreateDomainOptions(
[property: CliOption("--domain")] string Domain
) : AwsOptions
{
    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}