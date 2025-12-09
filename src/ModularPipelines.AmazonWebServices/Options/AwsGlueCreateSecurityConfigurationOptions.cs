using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-security-configuration")]
public record AwsGlueCreateSecurityConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--encryption-configuration")] string EncryptionConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}