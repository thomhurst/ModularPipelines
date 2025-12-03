using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "fetch-database-properties")]
public record GcloudComposerEnvironmentsFetchDatabasePropertiesOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions;