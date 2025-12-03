using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "import")]
public record GcloudFirestoreImportOptions(
[property: CliArgument] string InputUriPrefix
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--collection-ids")]
    public string[]? CollectionIds { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--namespace-ids")]
    public string[]? NamespaceIds { get; set; }
}