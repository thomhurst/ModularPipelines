using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-approved-origin")]
public record AwsConnectAssociateApprovedOriginOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--origin")] string Origin
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}