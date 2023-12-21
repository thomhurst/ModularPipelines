using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "plan")]
public class AzAppservicePlanCreate
{
    public AzAppservicePlanCreate(
        AzAppservicePlanCreateAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzAppservicePlanCreateAppserviceKube AppserviceKube { get; }
}