using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("demo", "secret-store", "load")]
public record AzDemoSecretStoreLoadOptions : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? KeyValue { get; set; }
}