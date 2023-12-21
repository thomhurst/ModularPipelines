using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops")]
public class AzDevopsSecurity
{
    public AzDevopsSecurity(
        AzDevopsSecurityGroup group,
        AzDevopsSecurityPermission permission
    )
    {
        Group = group;
        Permission = permission;
    }

    public AzDevopsSecurityGroup Group { get; }

    public AzDevopsSecurityPermission Permission { get; }
}