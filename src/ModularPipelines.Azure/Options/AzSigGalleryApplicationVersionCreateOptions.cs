using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "gallery-application", "version", "create")]
public record AzSigGalleryApplicationVersionCreateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--install-command")] string InstallCommand,
[property: CliOption("--name")] string Name,
[property: CliOption("--package-file-link")] string PackageFileLink,
[property: CliOption("--remove-command")] string RemoveCommand,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--config-file-name")]
    public string? ConfigFileName { get; set; }

    [CliOption("--default-file-link")]
    public string? DefaultFileLink { get; set; }

    [CliOption("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [CliFlag("--exclude-from")]
    public bool? ExcludeFrom { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--package-file-name")]
    public string? PackageFileName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--target-regions")]
    public string? TargetRegions { get; set; }

    [CliOption("--update-command")]
    public string? UpdateCommand { get; set; }
}