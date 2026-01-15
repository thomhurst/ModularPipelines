using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules.LocalMachine;

[DependsOn<CreateLocalNugetFolderModule>]
public class AddLocalNugetSourceModule : Module<CommandResult>
{
    private readonly IOptions<LocalNuGetSettings> _localNuGetSettings;

    public AddLocalNugetSourceModule(IOptions<LocalNuGetSettings> localNuGetSettings)
    {
        _localNuGetSettings = localNuGetSettings;
    }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithIgnoreFailuresWhen((_, ex) =>
            ex is CommandException commandException &&
            commandException.StandardOutput.Contains("The name specified has already been added to the list of available package sources"))
        .Build();

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var localNugetPathResult = context.GetModule<CreateLocalNugetFolderModule, Folder>();

        return await context.DotNet().Nuget.Add.Source(new DotNetNugetAddSourceOptions
        {
            Name = _localNuGetSettings.Value.SourceName,
            Packagesourcepath = localNugetPathResult.ValueOrDefault.AssertExists(),
        }, cancellationToken: cancellationToken);
    }
}