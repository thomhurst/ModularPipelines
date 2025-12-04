using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "subscription", "rename")]
public record AzAccountSubscriptionRenameOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}