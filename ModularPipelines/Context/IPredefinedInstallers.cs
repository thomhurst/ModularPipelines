using ModularPipelines.Models;

namespace ModularPipelines.Context;

public interface IPredefinedInstallers
{
    Task<CommandResult> Chocolatey();
    Task<CommandResult> Powershell7();
}