using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzMaintenance
{
    public AzMaintenance(
        AzMaintenanceApplyupdate applyupdate,
        AzMaintenanceAssignment assignment,
        AzMaintenanceConfiguration configuration,
        AzMaintenancePublicConfiguration publicConfiguration,
        AzMaintenanceUpdate update
    )
    {
        Applyupdate = applyupdate;
        Assignment = assignment;
        Configuration = configuration;
        PublicConfiguration = publicConfiguration;
        Update = update;
    }

    public AzMaintenanceApplyupdate Applyupdate { get; }

    public AzMaintenanceAssignment Assignment { get; }

    public AzMaintenanceConfiguration Configuration { get; }

    public AzMaintenancePublicConfiguration PublicConfiguration { get; }

    public AzMaintenanceUpdate Update { get; }
}