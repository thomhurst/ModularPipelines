using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "phonenumber", "list")]
public record AzCommunicationPhonenumberListOptions : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}