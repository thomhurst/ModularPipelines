using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "list-phone-numbers-v2")]
public record AwsConnectListPhoneNumbersV2Options : AwsOptions
{
    [CommandSwitch("--target-arn")]
    public string? TargetArn { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--phone-number-country-codes")]
    public string[]? PhoneNumberCountryCodes { get; set; }

    [CommandSwitch("--phone-number-types")]
    public string[]? PhoneNumberTypes { get; set; }

    [CommandSwitch("--phone-number-prefix")]
    public string? PhoneNumberPrefix { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}