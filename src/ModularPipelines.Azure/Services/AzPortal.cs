using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPortal
{
    public AzPortal(
        AzPortalDashboard dashboard
    )
    {
        Dashboard = dashboard;
    }

    public AzPortalDashboard Dashboard { get; }
}