using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "dags", "list")]
public record GcloudComposerEnvironmentsStorageDagsListOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions;