using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-voice", "batch-update-phone-number")]
public record AwsChimeSdkVoiceBatchUpdatePhoneNumberOptions(
[property: CommandSwitch("--update-phone-number-request-items")] string[] UpdatePhoneNumberRequestItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}