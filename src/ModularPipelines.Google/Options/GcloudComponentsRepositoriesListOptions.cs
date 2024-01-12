using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "repositories", "list")]
public record GcloudComponentsRepositoriesListOptions : GcloudOptions;