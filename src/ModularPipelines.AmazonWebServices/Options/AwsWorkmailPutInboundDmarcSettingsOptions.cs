using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-inbound-dmarc-settings")]
public record AwsWorkmailPutInboundDmarcSettingsOptions(
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}