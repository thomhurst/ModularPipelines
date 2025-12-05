using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "update-connection")]
public record AwsDirectconnectUpdateConnectionOptions(
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliOption("--encryption-mode")]
    public string? EncryptionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}