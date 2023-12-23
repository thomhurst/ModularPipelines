using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "create-phone-number-order")]
public record AwsChimeSdkVoiceCreatePhoneNumberOrderOptions(
[property: CommandSwitch("--product-type")] string ProductType,
[property: CommandSwitch("--e164-phone-numbers")] string[] E164PhoneNumbers
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}