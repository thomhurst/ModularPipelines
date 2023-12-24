using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "put-default-encryption-configuration")]
public record AwsIotsitewisePutDefaultEncryptionConfigurationOptions(
[property: CommandSwitch("--encryption-type")] string EncryptionType
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}