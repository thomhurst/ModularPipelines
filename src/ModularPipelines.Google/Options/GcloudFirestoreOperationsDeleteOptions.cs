using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "operations", "delete")]
public record GcloudFirestoreOperationsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}