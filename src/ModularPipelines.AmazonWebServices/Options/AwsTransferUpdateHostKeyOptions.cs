using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "update-host-key")]
public record AwsTransferUpdateHostKeyOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--host-key-id")] string HostKeyId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}