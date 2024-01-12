using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "fetch-database-properties")]
public record GcloudComposerEnvironmentsFetchDatabasePropertiesOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location
) : GcloudOptions;