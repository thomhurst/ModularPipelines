using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "image-version", "list")]
public record AzDevcenterAdminImageVersionListOptions(
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--image-name")] string ImageName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }
}