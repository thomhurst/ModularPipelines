using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "operations", "cancel")]
public record GcloudFirestoreOperationsCancelOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }
}