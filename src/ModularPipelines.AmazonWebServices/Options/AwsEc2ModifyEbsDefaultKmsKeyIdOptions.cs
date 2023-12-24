using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-ebs-default-kms-key-id")]
public record AwsEc2ModifyEbsDefaultKmsKeyIdOptions(
[property: CommandSwitch("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}