using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "disassociate-mac-sec-key")]
public record AwsDirectconnectDisassociateMacSecKeyOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--secret-arn")] string SecretArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}