using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sql")]
public class AzSqlServerArc
{
    public AzSqlServerArc(
        AzSqlServerArcAvailabilityGroup availabilityGroup,
        AzSqlServerArcBackupsPolicy backupsPolicy,
        AzSqlServerArcExtension extension
    )
    {
        AvailabilityGroup = availabilityGroup;
        BackupsPolicy = backupsPolicy;
        Extension = extension;
    }

    public AzSqlServerArcAvailabilityGroup AvailabilityGroup { get; }

    public AzSqlServerArcBackupsPolicy BackupsPolicy { get; }

    public AzSqlServerArcExtension Extension { get; }
}