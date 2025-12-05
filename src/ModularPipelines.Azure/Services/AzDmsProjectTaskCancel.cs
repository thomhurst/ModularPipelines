using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "project", "task")]
public class AzDmsProjectTaskCancel
{
    public AzDmsProjectTaskCancel(
        AzDmsProjectTaskCancelDmsPreview dmsPreview
    )
    {
        DmsPreview = dmsPreview;
    }

    public AzDmsProjectTaskCancelDmsPreview DmsPreview { get; }
}