using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

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