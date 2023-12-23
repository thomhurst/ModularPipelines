using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "stop-ingestion")]
public record AwsAppfabricStopIngestionOptions(
[property: CommandSwitch("--ingestion-identifier")] string IngestionIdentifier,
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}