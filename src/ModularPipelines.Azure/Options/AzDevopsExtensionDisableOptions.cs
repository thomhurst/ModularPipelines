using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "extension", "disable")]
public record AzDevopsExtensionDisableOptions(
[property: CliOption("--extension-id")] string ExtensionId,
[property: CliOption("--publisher-id")] string PublisherId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}