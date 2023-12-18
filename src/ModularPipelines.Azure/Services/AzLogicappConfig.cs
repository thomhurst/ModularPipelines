using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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