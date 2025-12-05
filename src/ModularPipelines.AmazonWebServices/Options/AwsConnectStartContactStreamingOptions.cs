using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "start-contact-streaming")]
public record AwsConnectStartContactStreamingOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--chat-streaming-configuration")] string ChatStreamingConfiguration
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}