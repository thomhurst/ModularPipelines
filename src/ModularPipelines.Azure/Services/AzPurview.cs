using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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