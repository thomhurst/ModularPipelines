using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "batch-delete-phone-number")]
public record AwsChimeBatchDeletePhoneNumberOptions(
[property: CliOption("--phone-number-ids")] string[] PhoneNumberIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}