using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "delete")]
public record GcloudHealthcareDatasetsDeleteOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;