using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "delete-host-key")]
public record AwsTransferDeleteHostKeyOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--host-key-id")] string HostKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}