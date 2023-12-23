using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("meteringmarketplace", "register-usage")]
public record AwsMeteringmarketplaceRegisterUsageOptions(
[property: CommandSwitch("--product-code")] string ProductCode,
[property: CommandSwitch("--public-key-version")] int PublicKeyVersion
) : AwsOptions
{
    [CommandSwitch("--nonce")]
    public string? Nonce { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}