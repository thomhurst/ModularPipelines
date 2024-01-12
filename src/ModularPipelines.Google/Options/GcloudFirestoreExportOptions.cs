using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "export")]
public record GcloudFirestoreExportOptions(
[property: PositionalArgument] string OutputUriPrefix
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--collection-ids")]
    public string[]? CollectionIds { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--namespace-ids")]
    public string[]? NamespaceIds { get; set; }

    [CommandSwitch("--snapshot-time")]
    public string? SnapshotTime { get; set; }
}