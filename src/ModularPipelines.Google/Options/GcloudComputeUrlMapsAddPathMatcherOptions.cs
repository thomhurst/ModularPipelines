using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "add-path-matcher")]
public record GcloudComputeUrlMapsAddPathMatcherOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--path-matcher-name")] string PathMatcherName,
[property: CommandSwitch("--default-backend-bucket")] string DefaultBackendBucket,
[property: CommandSwitch("--default-service")] string DefaultService
) : GcloudOptions
{
    [CommandSwitch("--backend-bucket-path-rules")]
    public string[]? BackendBucketPathRules { get; set; }

    [CommandSwitch("--backend-service-path-rules")]
    public string[]? BackendServicePathRules { get; set; }

    [BooleanCommandSwitch("--delete-orphaned-path-matcher")]
    public bool? DeleteOrphanedPathMatcher { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--path-rules")]
    public string[]? PathRules { get; set; }

    [CommandSwitch("--existing-host")]
    public string? ExistingHost { get; set; }

    [CommandSwitch("--new-hosts")]
    public string[]? NewHosts { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}