using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "set-iam-policy")]
public record GcloudComputeBackendServicesSetIamPolicyOptions(
[property: PositionalArgument] string BackendServiceName,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}