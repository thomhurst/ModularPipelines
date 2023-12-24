using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "list-supported-phone-number-countries")]
public record AwsChimeSdkVoiceListSupportedPhoneNumberCountriesOptions(
[property: CommandSwitch("--product-type")] string ProductType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}