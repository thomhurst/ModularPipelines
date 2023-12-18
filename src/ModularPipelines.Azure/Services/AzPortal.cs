using System.Diagnostics.CodeAnalysis;

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