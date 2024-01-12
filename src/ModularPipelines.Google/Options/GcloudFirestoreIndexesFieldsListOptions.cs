using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "fields", "list")]
public record GcloudFirestoreIndexesFieldsListOptions : GcloudOptions
{
    [CommandSwitch("--collection-group")]
    public string? CollectionGroup { get; set; }

    [CommandSwitch("--database")]
    public string? Database { get; set; }
}