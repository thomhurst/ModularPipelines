using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "create-field-level-encryption-profile")]
public record AwsCloudfrontCreateFieldLevelEncryptionProfileOptions(
[property: CommandSwitch("--field-level-encryption-profile-config")] string FieldLevelEncryptionProfileConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}