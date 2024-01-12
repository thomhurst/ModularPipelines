using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "datascans", "run")]
public record GcloudDataplexDatascansRunOptions(
[property: PositionalArgument] string Datascan,
[property: PositionalArgument] string Location
) : GcloudOptions;