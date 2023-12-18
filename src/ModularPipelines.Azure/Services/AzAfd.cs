using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAfd
{
    public AzAfd(
        AzAfdCustomDomain customDomain,
        AzAfdEndpoint endpoint,
        AzAfdLogAnalytic logAnalytic,
        AzAfdOrigin origin,
        AzAfdOriginGroup originGroup,
        AzAfdProfile profile,
        AzAfdRoute route,
        AzAfdRule rule,
        AzAfdRuleSet ruleSet,
        AzAfdSecret secret,
        AzAfdSecurityPolicy securityPolicy,
        AzAfdWafLogAnalytic wafLogAnalytic
    )
    {
        CustomDomain = customDomain;
        Endpoint = endpoint;
        LogAnalytic = logAnalytic;
        Origin = origin;
        OriginGroup = originGroup;
        Profile = profile;
        Route = route;
        Rule = rule;
        RuleSet = ruleSet;
        Secret = secret;
        SecurityPolicy = securityPolicy;
        WafLogAnalytic = wafLogAnalytic;
    }

    public AzAfdCustomDomain CustomDomain { get; }

    public AzAfdEndpoint Endpoint { get; }

    public AzAfdLogAnalytic LogAnalytic { get; }

    public AzAfdOrigin Origin { get; }

    public AzAfdOriginGroup OriginGroup { get; }

    public AzAfdProfile Profile { get; }

    public AzAfdRoute Route { get; }

    public AzAfdRule Rule { get; }

    public AzAfdRuleSet RuleSet { get; }

    public AzAfdSecret Secret { get; }

    public AzAfdSecurityPolicy SecurityPolicy { get; }

    public AzAfdWafLogAnalytic WafLogAnalytic { get; }
}