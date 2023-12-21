using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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