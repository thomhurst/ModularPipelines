using ModularPipelines.Models;
using ModularPipelines.Options.Linux;

namespace ModularPipelines.Context;

public interface ILinuxInstaller
{
    Task<CommandResult> InstallFromDpkg(DpkgInstallOptions options);
}