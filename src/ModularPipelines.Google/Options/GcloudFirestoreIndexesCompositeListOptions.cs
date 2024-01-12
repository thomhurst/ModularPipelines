using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "composite", "list")]
public record GcloudFirestoreIndexesCompositeListOptions : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }
}