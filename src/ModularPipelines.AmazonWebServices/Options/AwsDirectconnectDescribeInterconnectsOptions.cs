using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-interconnects")]
public record AwsDirectconnectDescribeInterconnectsOptions : AwsOptions
{
    [CliOption("--interconnect-id")]
    public string? InterconnectId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}