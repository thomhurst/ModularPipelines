using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test")]
public class GcloudFirebaseTestIos
{
    public GcloudFirebaseTestIos(
        GcloudFirebaseTestIosLocales locales,
        GcloudFirebaseTestIosModels models,
        GcloudFirebaseTestIosVersions versions,
        ICommand internalCommand
    )
    {
        Locales = locales;
        Models = models;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudFirebaseTestIosLocales Locales { get; }

    public GcloudFirebaseTestIosModels Models { get; }

    public GcloudFirebaseTestIosVersions Versions { get; }

    public async Task<CommandResult> ListDeviceCapacities(GcloudFirebaseTestIosListDeviceCapacitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudFirebaseTestIosListDeviceCapacitiesOptions(), token);
    }

    public async Task<CommandResult> Run(GcloudFirebaseTestIosRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}