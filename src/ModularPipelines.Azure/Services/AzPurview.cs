using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

