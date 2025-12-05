using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "update")]
public record AzSigUpdateOptions(
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--eula")]
    public string? Eula { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--public-name-prefix")]
    public string? PublicNamePrefix { get; set; }

    [CliOption("--publisher-email")]
    public string? PublisherEmail { get; set; }

    [CliOption("--publisher-uri")]
    public string? PublisherUri { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--select")]
    public string? Select { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--soft-delete")]
    public bool? SoftDelete { get; set; }
}