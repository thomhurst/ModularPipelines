using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("signalr", "custom-domain", "list")]
public record AzSignalrCustomDomainListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--signalr-name")] string SignalrName
) : AzOptions;