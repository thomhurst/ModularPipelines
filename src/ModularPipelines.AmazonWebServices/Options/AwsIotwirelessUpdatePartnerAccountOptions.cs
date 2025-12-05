using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-partner-account")]
public record AwsIotwirelessUpdatePartnerAccountOptions(
[property: CliOption("--sidewalk")] string Sidewalk,
[property: CliOption("--partner-account-id")] string PartnerAccountId,
[property: CliOption("--partner-type")] string PartnerType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}