using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "connect-app-authorization")]
public record AwsAppfabricConnectAppAuthorizationOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--app-authorization-identifier")] string AppAuthorizationIdentifier
) : AwsOptions
{
    [CommandSwitch("--auth-request")]
    public string? AuthRequest { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}