using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appfabric", "update-app-authorization")]
public record AwsAppfabricUpdateAppAuthorizationOptions(
[property: CliOption("--app-bundle-identifier")] string AppBundleIdentifier,
[property: CliOption("--app-authorization-identifier")] string AppAuthorizationIdentifier
) : AwsOptions
{
    [CliOption("--credential")]
    public string? Credential { get; set; }

    [CliOption("--tenant")]
    public string? Tenant { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}