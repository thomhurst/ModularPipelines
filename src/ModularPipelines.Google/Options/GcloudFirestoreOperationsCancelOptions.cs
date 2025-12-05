using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "operations", "cancel")]
public record GcloudFirestoreOperationsCancelOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}