using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-streaming-url")]
public record AwsAppstreamCreateStreamingUrlOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--application-id")]
    public string? ApplicationId { get; set; }

    [CliOption("--validity")]
    public long? Validity { get; set; }

    [CliOption("--session-context")]
    public string? SessionContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}