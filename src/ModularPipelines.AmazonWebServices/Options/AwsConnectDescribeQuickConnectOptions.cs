using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-quick-connect")]
public record AwsConnectDescribeQuickConnectOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--quick-connect-id")] string QuickConnectId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}