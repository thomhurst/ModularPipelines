using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks")]
public class AzAksTrustedaccess
{
    public AzAksTrustedaccess(
        AzAksTrustedaccessRole role,
        AzAksTrustedaccessRolebinding rolebinding
    )
    {
        Role = role;
        Rolebinding = rolebinding;
    }

    public AzAksTrustedaccessRole Role { get; }

    public AzAksTrustedaccessRolebinding Rolebinding { get; }
}