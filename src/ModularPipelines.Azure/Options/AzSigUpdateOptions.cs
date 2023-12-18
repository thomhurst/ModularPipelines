using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "update")]
public record AzSigUpdateOptions(
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--eula")]
    public string? Eula { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--permissions")]
    public string? Permissions { get; set; }

    [CommandSwitch("--public-name-prefix")]
    public string? PublicNamePrefix { get; set; }

    [CommandSwitch("--publisher-email")]
    public string? PublisherEmail { get; set; }

    [CommandSwitch("--publisher-uri")]
    public string? PublisherUri { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--select")]
    public string? Select { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--soft-delete")]
    public bool? SoftDelete { get; set; }
}