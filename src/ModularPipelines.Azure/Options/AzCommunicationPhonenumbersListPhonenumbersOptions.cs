using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("communication", "phonenumbers", "list-phonenumbers")]
public record AzCommunicationPhonenumbersListPhonenumbersOptions(
[property: CommandSwitch("--phonenumber")] string Phonenumber
) : AzOptions
{
    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }
}

