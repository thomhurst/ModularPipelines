using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "gallery-application", "version", "update")]
public record AzSigGalleryApplicationVersionUpdateOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--package-file-link")] string PackageFileLink,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-file-link")]
    public string? DefaultFileLink { get; set; }

    [CommandSwitch("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [BooleanCommandSwitch("--exclude-from")]
    public bool? ExcludeFrom { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--target-regions")]
    public string? TargetRegions { get; set; }
}

