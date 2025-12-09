using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-voice", "list-phone-numbers")]
public record AwsChimeSdkVoiceListPhoneNumbersOptions : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--product-type")]
    public string? ProductType { get; set; }

    [CliOption("--filter-name")]
    public string? FilterName { get; set; }

    [CliOption("--filter-value")]
    public string? FilterValue { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}