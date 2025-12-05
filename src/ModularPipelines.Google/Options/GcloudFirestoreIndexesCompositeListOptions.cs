using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "composite", "list")]
public record GcloudFirestoreIndexesCompositeListOptions : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}