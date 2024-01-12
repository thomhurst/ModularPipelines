using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "set-default-service")]
public record GcloudComputeUrlMapsSetDefaultServiceOptions(
[property: PositionalArgument] string UrlMap,
[property: CommandSwitch("--default-backend-bucket")] string DefaultBackendBucket,
[property: CommandSwitch("--default-service")] string DefaultService
) : GcloudOptions
{
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}