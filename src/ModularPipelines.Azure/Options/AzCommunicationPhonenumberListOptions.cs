using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("communication", "phonenumber", "list")]
public record AzCommunicationPhonenumberListOptions : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}