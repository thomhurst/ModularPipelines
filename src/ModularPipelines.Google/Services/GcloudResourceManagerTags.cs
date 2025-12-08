using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource-manager")]
public class GcloudResourceManagerTags
{
    public GcloudResourceManagerTags(
        GcloudResourceManagerTagsBindings bindings,
        GcloudResourceManagerTagsHolds holds,
        GcloudResourceManagerTagsKeys keys,
        GcloudResourceManagerTagsValues values
    )
    {
        Bindings = bindings;
        Holds = holds;
        Keys = keys;
        Values = values;
    }

    public GcloudResourceManagerTagsBindings Bindings { get; }

    public GcloudResourceManagerTagsHolds Holds { get; }

    public GcloudResourceManagerTagsKeys Keys { get; }

    public GcloudResourceManagerTagsValues Values { get; }
}