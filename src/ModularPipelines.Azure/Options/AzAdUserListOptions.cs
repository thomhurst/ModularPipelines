using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "user", "list")]
public record AzAdUserListOptions : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--upn")]
    public string? Upn { get; set; }
}