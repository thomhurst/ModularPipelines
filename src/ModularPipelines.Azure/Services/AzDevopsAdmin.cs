using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops")]
public class AzDevopsAdmin
{
    public AzDevopsAdmin(
        AzDevopsAdminBanner banner
    )
    {
        Banner = banner;
    }

    public AzDevopsAdminBanner Banner { get; }
}