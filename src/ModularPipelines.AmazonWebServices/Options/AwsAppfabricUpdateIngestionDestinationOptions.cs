using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "update-ingestion-destination")]
public record AwsAppfabricUpdateIngestionDestinationOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--ingestion-identifier")] string IngestionIdentifier,
[property: CommandSwitch("--ingestion-destination-identifier")] string IngestionDestinationIdentifier,
[property: CommandSwitch("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}