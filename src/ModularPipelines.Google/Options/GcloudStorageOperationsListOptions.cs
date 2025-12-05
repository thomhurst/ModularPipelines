using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "operations", "list")]
public record GcloudStorageOperationsListOptions(
[property: CliArgument] string ParentResourceName
) : GcloudOptions
{
    [CliOption("--server-filter")]
    public string? ServerFilter { get; set; }
}