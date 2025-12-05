using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "create-phone-number-order")]
public record AwsChimeCreatePhoneNumberOrderOptions(
[property: CliOption("--product-type")] string ProductType,
[property: CliOption("--e164-phone-numbers")] string[] E164PhoneNumbers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}