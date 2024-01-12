using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("anthos")]
public class GcloudAnthosConfig
{
    public GcloudAnthosConfig(
        GcloudAnthosConfigController controller,
        GcloudAnthosConfigOperations operations
    )
    {
        Controller = controller;
        Operations = operations;
    }

    public GcloudAnthosConfigController Controller { get; }

    public GcloudAnthosConfigOperations Operations { get; }
}