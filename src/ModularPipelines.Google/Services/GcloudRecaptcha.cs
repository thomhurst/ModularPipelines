using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudRecaptcha
{
    public GcloudRecaptcha(
        GcloudRecaptchaFirewallPolicies firewallPolicies,
        GcloudRecaptchaKeys keys
    )
    {
        FirewallPolicies = firewallPolicies;
        Keys = keys;
    }

    public GcloudRecaptchaFirewallPolicies FirewallPolicies { get; }

    public GcloudRecaptchaKeys Keys { get; }
}