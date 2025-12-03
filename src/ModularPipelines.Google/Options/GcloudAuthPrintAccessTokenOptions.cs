using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("auth", "print-access-token")]
public record GcloudAuthPrintAccessTokenOptions : GcloudOptions
{
    public GcloudAuthPrintAccessTokenOptions(
        string account
    )
    {
        GcloudAuthPrintAccessTokenOptionsAccount = account;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAuthPrintAccessTokenOptionsAccount { get; set; }

    [CliOption("--lifetime")]
    public string? Lifetime { get; set; }
}
