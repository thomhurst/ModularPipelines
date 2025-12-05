using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "describe-host-key")]
public record AwsTransferDescribeHostKeyOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--host-key-id")] string HostKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}