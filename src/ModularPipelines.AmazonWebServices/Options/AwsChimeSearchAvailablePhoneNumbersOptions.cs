using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "search-available-phone-numbers")]
public record AwsChimeSearchAvailablePhoneNumbersOptions : AwsOptions
{
    [CliOption("--area-code")]
    public string? AreaCode { get; set; }

    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--country")]
    public string? Country { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--toll-free-prefix")]
    public string? TollFreePrefix { get; set; }

    [CliOption("--phone-number-type")]
    public string? PhoneNumberType { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}