using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("meteringmarketplace", "register-usage")]
public record AwsMeteringmarketplaceRegisterUsageOptions(
[property: CliOption("--product-code")] string ProductCode,
[property: CliOption("--public-key-version")] int PublicKeyVersion
) : AwsOptions
{
    [CliOption("--nonce")]
    public string? Nonce { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}