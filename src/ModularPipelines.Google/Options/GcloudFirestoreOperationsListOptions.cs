using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "operations", "list")]
public record GcloudFirestoreOperationsListOptions : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}