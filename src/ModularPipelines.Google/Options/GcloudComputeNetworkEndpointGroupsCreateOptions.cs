using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "network-endpoint-groups", "create")]
public record GcloudComputeNetworkEndpointGroupsCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--default-port")]
    public string? DefaultPort { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--network-endpoint-type")]
    public string? NetworkEndpointType { get; set; }

    [CliOption("--psc-target-service")]
    public string? PscTargetService { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--cloud-function-name")]
    public string? CloudFunctionName { get; set; }

    [CliOption("--cloud-function-url-mask")]
    public string? CloudFunctionUrlMask { get; set; }

    [CliOption("--cloud-run-service")]
    public string? CloudRunService { get; set; }

    [CliOption("--cloud-run-tag")]
    public string? CloudRunTag { get; set; }

    [CliOption("--cloud-run-url-mask")]
    public string? CloudRunUrlMask { get; set; }

    [CliOption("--[no-]app-engine-app")]
    public string[]? NoAppEngineApp { get; set; }

    [CliOption("--app-engine-service")]
    public string? AppEngineService { get; set; }

    [CliOption("--app-engine-url-mask")]
    public string? AppEngineUrlMask { get; set; }

    [CliOption("--app-engine-version")]
    public string? AppEngineVersion { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}