using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "databases", "roles", "list")]
public record GcloudSpannerDatabasesRolesListOptions(
[property: CliOption("--database")] string Database,
[property: CliOption("--instance")] string Instance
) : GcloudOptions;