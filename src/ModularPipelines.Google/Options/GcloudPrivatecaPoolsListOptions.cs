using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "list")]
public record GcloudPrivatecaPoolsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}