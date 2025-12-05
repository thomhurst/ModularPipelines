using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "application-default", "print-access-token")]
public record GcloudAuthApplicationDefaultPrintAccessTokenOptions : GcloudOptions
{
    [CliOption("--lifetime")]
    public string? Lifetime { get; set; }

    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }
}