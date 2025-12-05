using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "list-phone-numbers")]
public record AwsConnectListPhoneNumbersOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--phone-number-types")]
    public string[]? PhoneNumberTypes { get; set; }

    [CliOption("--phone-number-country-codes")]
    public string[]? PhoneNumberCountryCodes { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}