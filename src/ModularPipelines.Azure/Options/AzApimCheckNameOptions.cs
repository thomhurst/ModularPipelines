using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "check-name")]
public record AzApimCheckNameOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;