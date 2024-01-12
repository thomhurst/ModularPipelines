using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("auth", "print-identity-token")]
public record GcloudAuthPrintIdentityTokenOptions : GcloudOptions
{
    public GcloudAuthPrintIdentityTokenOptions(
        string account
    )
    {
        GcloudAuthPrintIdentityTokenOptionsAccount = account;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAuthPrintIdentityTokenOptionsAccount { get; set; }

    [CommandSwitch("--audiences")]
    public string? Audiences { get; set; }

    [BooleanCommandSwitch("--include-email")]
    public bool? IncludeEmail { get; set; }

    [BooleanCommandSwitch("--include-license")]
    public bool? IncludeLicense { get; set; }

    [CommandSwitch("--token-format")]
    public string? TokenFormat { get; set; }
}
