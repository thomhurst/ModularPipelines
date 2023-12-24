using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage", "rotate-ingest-endpoint-credentials")]
public record AwsMediapackageRotateIngestEndpointCredentialsOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--ingest-endpoint-id")] string IngestEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}