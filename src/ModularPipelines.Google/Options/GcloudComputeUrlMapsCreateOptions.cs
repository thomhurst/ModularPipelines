using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "create")]
public record GcloudComputeUrlMapsCreateOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--default-backend-bucket")] string DefaultBackendBucket,
[property: CommandSwitch("--default-service")] string DefaultService
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}