using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDls
{
    public AzDls(
        AzDlsAccount account,
        AzDlsFs fs
    )
    {
        Account = account;
        Fs = fs;
    }

    public AzDlsAccount Account { get; }

    public AzDlsFs Fs { get; }
}