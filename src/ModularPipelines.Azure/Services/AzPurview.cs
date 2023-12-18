using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPurview
{
    public AzPurview(
        AzPurviewAccount account,
        AzPurviewDefaultAccount defaultAccount
    )
    {
        Account = account;
        DefaultAccount = defaultAccount;
    }

    public AzPurviewAccount Account { get; }

    public AzPurviewDefaultAccount DefaultAccount { get; }
}