using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alias", "create")]
public record AzAliasCreateOptions(
[property: CliOption("--command")] string Command,
[property: CliOption("--name")] string Name
) : AzOptions;