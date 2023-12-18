using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatadog
{
    public AzDatadog(
        AzDatadogMonitor monitor,
        AzDatadogSsoConfig ssoConfig,
        AzDatadogTagRule tagRule,
        AzDatadogTerms terms
    )
    {
        Monitor = monitor;
        SsoConfig = ssoConfig;
        TagRule = tagRule;
        Terms = terms;
    }

    public AzDatadogMonitor Monitor { get; }

    public AzDatadogSsoConfig SsoConfig { get; }

    public AzDatadogTagRule TagRule { get; }

    public AzDatadogTerms Terms { get; }
}