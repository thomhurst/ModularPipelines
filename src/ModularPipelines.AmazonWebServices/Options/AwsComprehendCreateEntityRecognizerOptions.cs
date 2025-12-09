using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "create-entity-recognizer")]
public record AwsComprehendCreateEntityRecognizerOptions(
[property: CliOption("--recognizer-name")] string RecognizerName,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--language-code")] string LanguageCode
) : AwsOptions
{
    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--volume-kms-key-id")]
    public string? VolumeKmsKeyId { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--model-kms-key-id")]
    public string? ModelKmsKeyId { get; set; }

    [CliOption("--model-policy")]
    public string? ModelPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}