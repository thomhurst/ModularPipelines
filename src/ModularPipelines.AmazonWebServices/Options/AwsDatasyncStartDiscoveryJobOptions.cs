using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "start-discovery-job")]
public record AwsDatasyncStartDiscoveryJobOptions(
[property: CommandSwitch("--storage-system-arn")] string StorageSystemArn,
[property: CommandSwitch("--collection-duration-minutes")] int CollectionDurationMinutes
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}