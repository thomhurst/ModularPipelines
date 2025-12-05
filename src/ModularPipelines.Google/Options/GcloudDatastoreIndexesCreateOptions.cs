using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "indexes", "create")]
public record GcloudDatastoreIndexesCreateOptions(
[property: CliArgument] string IndexFile
) : GcloudOptions
{
    [CliOption("--database")]
    public string? Database { get; set; }
}