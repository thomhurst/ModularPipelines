using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp")]
public class AzFunctionappCreate
{
    public AzFunctionappCreate(
        AzFunctionappCreateAppserviceKube appserviceKube
    )
    {
        AppserviceKube = appserviceKube;
    }

    public AzFunctionappCreateAppserviceKube AppserviceKube { get; }
}