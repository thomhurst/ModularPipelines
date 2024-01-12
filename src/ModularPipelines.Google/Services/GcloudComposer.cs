using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudComposer
{
    public GcloudComposer(
        GcloudComposerEnvironments environments,
        GcloudComposerOperations operations
    )
    {
        Environments = environments;
        Operations = operations;
    }

    public GcloudComposerEnvironments Environments { get; }

    public GcloudComposerOperations Operations { get; }
}