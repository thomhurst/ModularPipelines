using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "operations", "list")]
public record GcloudFirestoreOperationsListOptions : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }
}