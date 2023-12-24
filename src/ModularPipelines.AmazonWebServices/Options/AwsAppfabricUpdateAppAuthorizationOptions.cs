using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "update-app-authorization")]
public record AwsAppfabricUpdateAppAuthorizationOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--app-authorization-identifier")] string AppAuthorizationIdentifier
) : AwsOptions
{
    [CommandSwitch("--credential")]
    public string? Credential { get; set; }

    [CommandSwitch("--tenant")]
    public string? Tenant { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}