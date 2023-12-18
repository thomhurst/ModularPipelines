using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzAlertsManagement
{
    public AzAlertsManagement(
        AzAlertsManagementPrometheusRuleGroup prometheusRuleGroup
    )
    {
        PrometheusRuleGroup = prometheusRuleGroup;
    }

    public AzAlertsManagementPrometheusRuleGroup PrometheusRuleGroup { get; }
}