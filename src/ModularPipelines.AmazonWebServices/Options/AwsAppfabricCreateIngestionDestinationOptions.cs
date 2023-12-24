using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "create-ingestion-destination")]
public record AwsAppfabricCreateIngestionDestinationOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--ingestion-identifier")] string IngestionIdentifier,
[property: CommandSwitch("--processing-configuration")] string ProcessingConfiguration,
[property: CommandSwitch("--destination-configuration")] string DestinationConfiguration
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}