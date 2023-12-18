using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("maps", "map", "list-operation")]
public record AzMapsMapListOperationOptions : AzOptions;