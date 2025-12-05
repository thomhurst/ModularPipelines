using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "storage", "data", "list")]
public record GcloudComposerEnvironmentsStorageDataListOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--location")] string Location
) : GcloudOptions;