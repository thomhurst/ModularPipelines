using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "search-available-phone-numbers")]
public record AwsConnectSearchAvailablePhoneNumbersOptions(
[property: CliOption("--phone-number-country-code")] string PhoneNumberCountryCode,
[property: CliOption("--phone-number-type")] string PhoneNumberType
) : AwsOptions
{
    [CliOption("--target-arn")]
    public string? TargetArn { get; set; }

    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--phone-number-prefix")]
    public string? PhoneNumberPrefix { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}