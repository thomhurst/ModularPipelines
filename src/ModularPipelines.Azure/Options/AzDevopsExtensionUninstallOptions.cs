using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "extension", "uninstall")]
public record AzDevopsExtensionUninstallOptions(
[property: CliOption("--extension-id")] string ExtensionId,
[property: CliOption("--publisher-id")] string PublisherId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}