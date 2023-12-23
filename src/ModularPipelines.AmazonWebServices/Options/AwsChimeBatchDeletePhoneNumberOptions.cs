using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-delete-phone-number")]
public record AwsChimeBatchDeletePhoneNumberOptions(
[property: CommandSwitch("--phone-number-ids")] string[] PhoneNumberIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}