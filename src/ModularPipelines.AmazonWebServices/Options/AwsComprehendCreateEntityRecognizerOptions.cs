using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "create-entity-recognizer")]
public record AwsComprehendCreateEntityRecognizerOptions(
[property: CommandSwitch("--recognizer-name")] string RecognizerName,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn,
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--language-code")] string LanguageCode
) : AwsOptions
{
    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--volume-kms-key-id")]
    public string? VolumeKmsKeyId { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--model-kms-key-id")]
    public string? ModelKmsKeyId { get; set; }

    [CommandSwitch("--model-policy")]
    public string? ModelPolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}