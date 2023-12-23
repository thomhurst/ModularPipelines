using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "check-if-phone-number-is-opted-out")]
public record AwsSnsCheckIfPhoneNumberIsOptedOutOptions(
[property: CommandSwitch("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}