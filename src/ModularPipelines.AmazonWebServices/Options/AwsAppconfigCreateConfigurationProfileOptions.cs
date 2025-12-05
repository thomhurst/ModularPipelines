using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "create-configuration-profile")]
public record AwsAppconfigCreateConfigurationProfileOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--location-uri")] string LocationUri
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--retrieval-role-arn")]
    public string? RetrievalRoleArn { get; set; }

    [CliOption("--validators")]
    public string[]? Validators { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}