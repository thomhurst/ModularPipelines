using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

