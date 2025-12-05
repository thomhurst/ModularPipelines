using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "dags", "delete")]
public record GcloudComposerEnvironmentsStorageDagsDeleteOptions(
[property: CliArgument] string Target,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions;