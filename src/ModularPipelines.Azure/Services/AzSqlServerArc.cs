using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql")]
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