using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "runtimes", "list")]
public record GcloudAppRuntimesListOptions(
[property: CliOption("--environment")] string Environment
) : GcloudOptions;