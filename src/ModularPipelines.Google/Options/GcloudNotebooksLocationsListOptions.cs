using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "locations", "list")]
public record GcloudNotebooksLocationsListOptions : GcloudOptions;