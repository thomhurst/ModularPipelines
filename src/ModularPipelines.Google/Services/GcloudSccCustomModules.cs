using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc")]
public class GcloudSccCustomModules
{
    public GcloudSccCustomModules(
        GcloudSccCustomModulesSha sha
    )
    {
        Sha = sha;
    }

    public GcloudSccCustomModulesSha Sha { get; }
}