using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "fields", "ttls", "list")]
public record GcloudFirestoreFieldsTtlsListOptions : GcloudOptions
{
    [CommandSwitch("--collection-group")]
    public string? CollectionGroup { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }
}