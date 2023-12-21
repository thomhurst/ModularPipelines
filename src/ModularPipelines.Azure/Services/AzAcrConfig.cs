using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr")]
public class AzAcrConfig
{
    public AzAcrConfig(
        AzAcrConfigAuthenticationAsArm authenticationAsArm,
        AzAcrConfigContentTrust contentTrust,
        AzAcrConfigRetention retention,
        AzAcrConfigSoftDelete softDelete
    )
    {
        AuthenticationAsArm = authenticationAsArm;
        ContentTrust = contentTrust;
        Retention = retention;
        SoftDelete = softDelete;
    }

    public AzAcrConfigAuthenticationAsArm AuthenticationAsArm { get; }

    public AzAcrConfigContentTrust ContentTrust { get; }

    public AzAcrConfigRetention Retention { get; }

    public AzAcrConfigSoftDelete SoftDelete { get; }
}