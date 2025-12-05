using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-field-level-encryption-profile")]
public record AwsCloudfrontCreateFieldLevelEncryptionProfileOptions(
[property: CliOption("--field-level-encryption-profile-config")] string FieldLevelEncryptionProfileConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}