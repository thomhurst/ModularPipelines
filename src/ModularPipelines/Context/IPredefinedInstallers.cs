using ModularPipelines.Models;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IPredefinedInstallers
{
    Task<CommandResult> Chocolatey();
    Task<CommandResult> Powershell7();
    Task<File?> Nvm(string? version = null);
    Task<CommandResult> Node(string version = "--lts");
}