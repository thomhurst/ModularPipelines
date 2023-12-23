using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "validate-e911-address")]
public record AwsChimeSdkVoiceValidateE911AddressOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--street-number")] string StreetNumber,
[property: CommandSwitch("--street-info")] string StreetInfo,
[property: CommandSwitch("--city")] string City,
[property: CommandSwitch("--state")] string State,
[property: CommandSwitch("--country")] string Country,
[property: CommandSwitch("--postal-code")] string PostalCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}