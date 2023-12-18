using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "va")]
public class AzSecurityVaSql
{
    public AzSecurityVaSql(
        AzSecurityVaSqlBaseline baseline,
        AzSecurityVaSqlResults results,
        AzSecurityVaSqlScans scans
    )
    {
        Baseline = baseline;
        Results = results;
        Scans = scans;
    }

    public AzSecurityVaSqlBaseline Baseline { get; }

    public AzSecurityVaSqlResults Results { get; }

    public AzSecurityVaSqlScans Scans { get; }
}

