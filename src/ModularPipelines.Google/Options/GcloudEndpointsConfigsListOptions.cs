using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "configs", "list")]
public record GcloudEndpointsConfigsListOptions(
[property: CliOption("--service")] string Service
) : GcloudOptions;