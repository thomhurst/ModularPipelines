using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "task")]
public class AzDmsProjectTaskCreate
{
    public AzDmsProjectTaskCreate(
        AzDmsProjectTaskCreateDmsPreview dmsPreview
    )
    {
        DmsPreview = dmsPreview;
    }

    public AzDmsProjectTaskCreateDmsPreview DmsPreview { get; }
}