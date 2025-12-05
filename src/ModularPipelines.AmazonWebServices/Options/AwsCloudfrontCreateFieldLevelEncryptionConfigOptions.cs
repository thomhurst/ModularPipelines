using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "create-field-level-encryption-config")]
public record AwsCloudfrontCreateFieldLevelEncryptionConfigOptions(
[property: CliOption("--field-level-encryption-config")] string FieldLevelEncryptionConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}