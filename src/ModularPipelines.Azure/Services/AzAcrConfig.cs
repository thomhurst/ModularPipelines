using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("acr")]
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