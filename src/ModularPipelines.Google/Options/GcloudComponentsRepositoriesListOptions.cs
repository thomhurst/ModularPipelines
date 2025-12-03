using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "repositories", "list")]
public record GcloudComponentsRepositoriesListOptions : GcloudOptions;