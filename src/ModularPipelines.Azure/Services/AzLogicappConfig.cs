using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp")]
public class AzLogicappConfig
{
    public AzLogicappConfig(
        AzLogicappConfigAppsettings appsettings
    )
    {
        Appsettings = appsettings;
    }

    public AzLogicappConfigAppsettings Appsettings { get; }
}