using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "import-key-pair")]
public record AwsLightsailImportKeyPairOptions(
[property: CliOption("--key-pair-name")] string KeyPairName,
[property: CliOption("--public-key-base64")] string PublicKeyBase64
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}