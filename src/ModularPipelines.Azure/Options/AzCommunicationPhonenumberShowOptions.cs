using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("communication", "phonenumber", "show")]
public record AzCommunicationPhonenumberShowOptions(
[property: CliOption("--phonenumber")] string Phonenumber
) : AzOptions
{
    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }
}