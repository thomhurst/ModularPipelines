using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "delete-sms-sandbox-phone-number")]
public record AwsSnsDeleteSmsSandboxPhoneNumberOptions(
[property: CommandSwitch("--phone-number")] string PhoneNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}