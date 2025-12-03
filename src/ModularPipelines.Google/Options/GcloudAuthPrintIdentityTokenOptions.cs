using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "print-identity-token")]
public record GcloudAuthPrintIdentityTokenOptions : GcloudOptions
{
    public GcloudAuthPrintIdentityTokenOptions(
        string account
    )
    {
        GcloudAuthPrintIdentityTokenOptionsAccount = account;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAuthPrintIdentityTokenOptionsAccount { get; set; }

    [CliOption("--audiences")]
    public string? Audiences { get; set; }

    [CliFlag("--include-email")]
    public bool? IncludeEmail { get; set; }

    [CliFlag("--include-license")]
    public bool? IncludeLicense { get; set; }

    [CliOption("--token-format")]
    public string? TokenFormat { get; set; }
}
