using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "login")]
public record AzDevopsLoginOptions : AzOptions
{
    [CliOption("--org")]
    public string? Org { get; set; }
}