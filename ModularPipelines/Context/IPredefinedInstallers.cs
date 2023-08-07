using ModularPipelines.Models;

namespace ModularPipelines.Context;

public interface IPredefinedInstallers
{
    Task<CommandResult> Chocolatey();
    Task<CommandResult> Powershell7();
    Task<CommandResult> Nvm(string? version = null);
    Task<CommandResult> Node(string version = "--lts");
}