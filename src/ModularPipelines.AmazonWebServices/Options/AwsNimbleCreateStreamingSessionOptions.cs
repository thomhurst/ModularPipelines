using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "create-streaming-session")]
public record AwsNimbleCreateStreamingSessionOptions(
[property: CliOption("--launch-profile-id")] string LaunchProfileId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--ec2-instance-type")]
    public string? Ec2InstanceType { get; set; }

    [CliOption("--owned-by")]
    public string? OwnedBy { get; set; }

    [CliOption("--streaming-image-id")]
    public string? StreamingImageId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}