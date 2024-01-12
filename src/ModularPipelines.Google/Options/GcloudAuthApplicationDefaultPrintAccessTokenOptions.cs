using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "application-default", "print-access-token")]
public record GcloudAuthApplicationDefaultPrintAccessTokenOptions : GcloudOptions
{
    [CommandSwitch("--lifetime")]
    public string? Lifetime { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }
}