using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "import-key-pair")]
public record AwsLightsailImportKeyPairOptions(
[property: CommandSwitch("--key-pair-name")] string KeyPairName,
[property: CommandSwitch("--public-key-base64")] string PublicKeyBase64
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}