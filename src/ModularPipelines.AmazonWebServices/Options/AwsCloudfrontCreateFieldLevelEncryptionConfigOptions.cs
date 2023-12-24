using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-field-level-encryption-config")]
public record AwsCloudfrontCreateFieldLevelEncryptionConfigOptions(
[property: CommandSwitch("--field-level-encryption-config")] string FieldLevelEncryptionConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}