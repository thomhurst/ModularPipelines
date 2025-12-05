using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-thin-client", "deregister-device")]
public record AwsWorkspacesThinClientDeregisterDeviceOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--target-device-status")]
    public string? TargetDeviceStatus { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}