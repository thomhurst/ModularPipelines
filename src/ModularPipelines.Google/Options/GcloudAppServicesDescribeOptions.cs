using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "services", "describe")]
public record GcloudAppServicesDescribeOptions(
[property: CliArgument] string Service
) : GcloudOptions;