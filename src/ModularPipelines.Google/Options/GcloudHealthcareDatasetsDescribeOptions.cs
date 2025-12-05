using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "describe")]
public record GcloudHealthcareDatasetsDescribeOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions;