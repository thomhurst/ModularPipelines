using ModularPipelines.Context;

namespace ModularPipelines.Installer.Extensions;

public static class InstallerExtensions
{
    public static IInstaller Installer(this IModuleContext context) => context.Get<Installer>();
}