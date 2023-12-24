using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "put-kms-encryption-key")]
public record AwsFrauddetectorPutKmsEncryptionKeyOptions(
[property: CommandSwitch("--kms-encryption-key-arn")] string KmsEncryptionKeyArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}