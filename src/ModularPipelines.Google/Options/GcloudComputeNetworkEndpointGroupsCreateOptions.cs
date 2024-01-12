using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "network-endpoint-groups", "create")]
public record GcloudComputeNetworkEndpointGroupsCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--default-port")]
    public string? DefaultPort { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--network-endpoint-type")]
    public string? NetworkEndpointType { get; set; }

    [CommandSwitch("--psc-target-service")]
    public string? PscTargetService { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--cloud-function-name")]
    public string? CloudFunctionName { get; set; }

    [CommandSwitch("--cloud-function-url-mask")]
    public string? CloudFunctionUrlMask { get; set; }

    [CommandSwitch("--cloud-run-service")]
    public string? CloudRunService { get; set; }

    [CommandSwitch("--cloud-run-tag")]
    public string? CloudRunTag { get; set; }

    [CommandSwitch("--cloud-run-url-mask")]
    public string? CloudRunUrlMask { get; set; }

    [CommandSwitch("--[no-]app-engine-app")]
    public string[]? NoAppEngineApp { get; set; }

    [CommandSwitch("--app-engine-service")]
    public string? AppEngineService { get; set; }

    [CommandSwitch("--app-engine-url-mask")]
    public string? AppEngineUrlMask { get; set; }

    [CommandSwitch("--app-engine-version")]
    public string? AppEngineVersion { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}