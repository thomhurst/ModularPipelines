using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "live-output", "list")]
public record AzAmsLiveOutputListOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--live-event-name")] string LiveEventName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;