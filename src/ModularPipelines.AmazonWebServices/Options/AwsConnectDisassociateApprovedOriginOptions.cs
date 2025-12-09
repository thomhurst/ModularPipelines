using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-approved-origin")]
public record AwsConnectDisassociateApprovedOriginOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--origin")] string Origin
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}