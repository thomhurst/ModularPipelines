using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "print-access-token")]
public record GcloudAuthPrintAccessTokenOptions : GcloudOptions
{
    public GcloudAuthPrintAccessTokenOptions(
        string account
    )
    {
        GcloudAuthPrintAccessTokenOptionsAccount = account;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAuthPrintAccessTokenOptionsAccount { get; set; }

    [CommandSwitch("--lifetime")]
    public string? Lifetime { get; set; }
}
