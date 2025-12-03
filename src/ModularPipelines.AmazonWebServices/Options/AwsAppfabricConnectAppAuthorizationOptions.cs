using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "connect-app-authorization")]
public record AwsAppfabricConnectAppAuthorizationOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--app-authorization-identifier")] string AppAuthorizationIdentifier
) : AwsOptions
{
    [CliOption("--auth-request")]
    public string? AuthRequest { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}