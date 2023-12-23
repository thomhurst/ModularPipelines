using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "batch-delete-phone-number")]
public record AwsChimeSdkVoiceBatchDeletePhoneNumberOptions(
[property: CommandSwitch("--phone-number-ids")] string[] PhoneNumberIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}