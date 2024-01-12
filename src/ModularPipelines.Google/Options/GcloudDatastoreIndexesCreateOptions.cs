using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastore", "indexes", "create")]
public record GcloudDatastoreIndexesCreateOptions(
[property: PositionalArgument] string IndexFile
) : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }
}