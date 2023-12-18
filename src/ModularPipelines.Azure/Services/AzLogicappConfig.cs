using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

