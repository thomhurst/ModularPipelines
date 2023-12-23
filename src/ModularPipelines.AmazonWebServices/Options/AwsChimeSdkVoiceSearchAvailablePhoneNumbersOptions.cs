using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "search-available-phone-numbers")]
public record AwsChimeSdkVoiceSearchAvailablePhoneNumbersOptions : AwsOptions
{
    [CommandSwitch("--area-code")]
    public string? AreaCode { get; set; }

    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--country")]
    public string? Country { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--toll-free-prefix")]
    public string? TollFreePrefix { get; set; }

    [CommandSwitch("--phone-number-type")]
    public string? PhoneNumberType { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}