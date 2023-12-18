using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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