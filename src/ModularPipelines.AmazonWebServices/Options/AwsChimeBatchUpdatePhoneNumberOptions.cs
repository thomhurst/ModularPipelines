using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "batch-update-phone-number")]
public record AwsChimeBatchUpdatePhoneNumberOptions(
[property: CliOption("--update-phone-number-request-items")] string[] UpdatePhoneNumberRequestItems
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}