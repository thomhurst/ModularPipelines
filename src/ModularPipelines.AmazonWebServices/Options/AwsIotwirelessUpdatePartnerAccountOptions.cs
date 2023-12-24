using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-partner-account")]
public record AwsIotwirelessUpdatePartnerAccountOptions(
[property: CommandSwitch("--sidewalk")] string Sidewalk,
[property: CommandSwitch("--partner-account-id")] string PartnerAccountId,
[property: CommandSwitch("--partner-type")] string PartnerType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}