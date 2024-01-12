using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments")]
public class GcloudComposerEnvironmentsStorage
{
    public GcloudComposerEnvironmentsStorage(
        GcloudComposerEnvironmentsStorageDags dags,
        GcloudComposerEnvironmentsStorageData data,
        GcloudComposerEnvironmentsStoragePlugins plugins
    )
    {
        Dags = dags;
        Data = data;
        Plugins = plugins;
    }

    public GcloudComposerEnvironmentsStorageDags Dags { get; }

    public GcloudComposerEnvironmentsStorageData Data { get; }

    public GcloudComposerEnvironmentsStoragePlugins Plugins { get; }
}