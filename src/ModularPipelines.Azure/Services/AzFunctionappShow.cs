using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp")]
public class AzFunctionappShow
{
    public AzFunctionappShow(
        AzFunctionappShowAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzFunctionappShowAppserviceKube AppserviceKube { get; }
}