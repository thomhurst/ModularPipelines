using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "disassociate--account-from-partner-account")]
public record AwsIotwirelessDisassociateAwsAccountFromPartnerAccountOptions(
[property: CommandSwitch("--partner-account-id")] string PartnerAccountId,
[property: CommandSwitch("--partner-type")] string PartnerType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}