using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("maps", "map", "list-operation")]
public record AzMapsMapListOperationOptions : AzOptions;