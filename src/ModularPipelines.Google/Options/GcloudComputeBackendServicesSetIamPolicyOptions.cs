using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "set-iam-policy")]
public record GcloudComputeBackendServicesSetIamPolicyOptions(
[property: CliArgument] string BackendServiceName,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}