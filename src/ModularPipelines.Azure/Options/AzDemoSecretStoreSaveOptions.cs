using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("demo", "secret-store", "save")]
public record AzDemoSecretStoreSaveOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? KeyValue { get; set; }
}