using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-update-phone-number")]
public record AwsChimeBatchUpdatePhoneNumberOptions(
[property: CommandSwitch("--update-phone-number-request-items")] string[] UpdatePhoneNumberRequestItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}