using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test")]
public class GcloudFirebaseTestAndroid
{
    public GcloudFirebaseTestAndroid(
        GcloudFirebaseTestAndroidLocales locales,
        GcloudFirebaseTestAndroidModels models,
        GcloudFirebaseTestAndroidVersions versions,
        ICommand internalCommand
    )
    {
        Locales = locales;
        Models = models;
        Versions = versions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudFirebaseTestAndroidLocales Locales { get; }

    public GcloudFirebaseTestAndroidModels Models { get; }

    public GcloudFirebaseTestAndroidVersions Versions { get; }

    public async Task<CommandResult> ListDeviceCapacities(GcloudFirebaseTestAndroidListDeviceCapacitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudFirebaseTestAndroidListDeviceCapacitiesOptions(), token);
    }

    public async Task<CommandResult> Run(GcloudFirebaseTestAndroidRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}