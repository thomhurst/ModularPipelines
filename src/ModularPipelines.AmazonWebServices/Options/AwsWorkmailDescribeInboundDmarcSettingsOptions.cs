using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "describe-inbound-dmarc-settings")]
public record AwsWorkmailDescribeInboundDmarcSettingsOptions(
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}