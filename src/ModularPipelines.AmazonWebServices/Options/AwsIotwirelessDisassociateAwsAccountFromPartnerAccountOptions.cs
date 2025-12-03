using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "disassociate--account-from-partner-account")]
public record AwsIotwirelessDisassociateAwsAccountFromPartnerAccountOptions(
[property: CliOption("--partner-account-id")] string PartnerAccountId,
[property: CliOption("--partner-type")] string PartnerType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}