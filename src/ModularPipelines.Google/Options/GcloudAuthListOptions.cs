using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "list")]
public record GcloudAuthListOptions : GcloudOptions
{
    [CliOption("--filter-account")]
    public string? FilterAccount { get; set; }
}