using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "application", "set")]
public record AzBatchApplicationSetOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-updates")]
    public bool? AllowUpdates { get; set; }

    [CliOption("--default-version")]
    public string? DefaultVersion { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}