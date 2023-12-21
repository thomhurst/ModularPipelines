using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzLogz
{
    public AzLogz(
        AzLogzMonitor monitor,
        AzLogzRule rule,
        AzLogzSso sso,
        AzLogzSubAccount subAccount,
        AzLogzSubRule subRule
    )
    {
        Monitor = monitor;
        Rule = rule;
        Sso = sso;
        SubAccount = subAccount;
        SubRule = subRule;
    }

    public AzLogzMonitor Monitor { get; }

    public AzLogzRule Rule { get; }

    public AzLogzSso Sso { get; }

    public AzLogzSubAccount SubAccount { get; }

    public AzLogzSubRule SubRule { get; }
}