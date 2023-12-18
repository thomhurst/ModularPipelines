using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logic")]
public class AzLogicIntegrationAccount
{
    public AzLogicIntegrationAccount(
        AzLogicIntegrationAccountMap map,
        ICommand internalCommand
    )
    {
        Map = map;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLogicIntegrationAccountMap Map { get; }

    public async Task<CommandResult> Create(AzLogicIntegrationAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzLogicIntegrationAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicIntegrationAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> Import(AzLogicIntegrationAccountImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzLogicIntegrationAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicIntegrationAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzLogicIntegrationAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzLogicIntegrationAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzLogicIntegrationAccountUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}