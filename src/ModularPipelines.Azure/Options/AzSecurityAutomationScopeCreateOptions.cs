using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "automation-scope", "create")]
public record AzSecurityAutomationScopeCreateOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--scope-path")] string ScopePath
) : AzOptions;