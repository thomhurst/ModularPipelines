using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp")]
public class AzFunctionappConfig
{
    public AzFunctionappConfig(
        AzFunctionappConfigAccessRestriction accessRestriction,
        AzFunctionappConfigAppsettings appsettings,
        AzFunctionappConfigContainer container,
        AzFunctionappConfigHostname hostname,
        AzFunctionappConfigSsl ssl,
        ICommand internalCommand
    )
    {
        AccessRestriction = accessRestriction;
        Appsettings = appsettings;
        Container = container;
        Hostname = hostname;
        Ssl = ssl;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappConfigAccessRestriction AccessRestriction { get; }

    public AzFunctionappConfigAppsettings Appsettings { get; }

    public AzFunctionappConfigContainer Container { get; }

    public AzFunctionappConfigHostname Hostname { get; }

    public AzFunctionappConfigSsl Ssl { get; }

    public async Task<CommandResult> Set(AzFunctionappConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigSetOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappConfigShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConfigShowOptions(), token);
    }
}