using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "project")]
public class AzDmsProjectCreate
{
    public AzDmsProjectCreate(
        AzDmsProjectCreateDmsPreview dmsPreview
    )
    {
        DmsPreview = dmsPreview;
    }

    public AzDmsProjectCreateDmsPreview DmsPreview { get; }
}