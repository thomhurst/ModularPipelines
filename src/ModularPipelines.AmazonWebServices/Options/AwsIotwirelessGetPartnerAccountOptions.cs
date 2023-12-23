using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "get-partner-account")]
public record AwsIotwirelessGetPartnerAccountOptions(
[property: CommandSwitch("--partner-account-id")] string PartnerAccountId,
[property: CommandSwitch("--partner-type")] string PartnerType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}