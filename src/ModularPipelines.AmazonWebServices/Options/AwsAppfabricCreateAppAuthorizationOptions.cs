using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appfabric", "create-app-authorization")]
public record AwsAppfabricCreateAppAuthorizationOptions(
[property: CommandSwitch("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--credential")] string Credential,
[property: CommandSwitch("--tenant")] string Tenant,
[property: CommandSwitch("--auth-type")] string AuthType
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}