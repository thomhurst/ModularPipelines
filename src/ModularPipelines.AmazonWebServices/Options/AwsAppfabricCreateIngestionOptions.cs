using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "create-ingestion")]
public record AwsAppfabricCreateIngestionOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--tenant-id")] string TenantId,
[property: CommandSwitch("--ingestion-type")] string IngestionType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}