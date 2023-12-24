using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "delete-app-bundle")]
public record AwsAppfabricDeleteAppBundleOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}