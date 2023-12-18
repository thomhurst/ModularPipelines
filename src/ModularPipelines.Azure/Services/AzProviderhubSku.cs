using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub")]
public class AzProviderhubSku
{
    public AzProviderhubSku(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzProviderhubSkuCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzProviderhubSkuDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzProviderhubSkuListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzProviderhubSkuShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubSkuShowOptions(), token);
    }

    public async Task<CommandResult> ShowNestedResourceTypeFirst(AzProviderhubSkuShowNestedResourceTypeFirstOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubSkuShowNestedResourceTypeFirstOptions(), token);
    }

    public async Task<CommandResult> ShowNestedResourceTypeSecond(AzProviderhubSkuShowNestedResourceTypeSecondOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubSkuShowNestedResourceTypeSecondOptions(), token);
    }

    public async Task<CommandResult> ShowNestedResourceTypeThird(AzProviderhubSkuShowNestedResourceTypeThirdOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzProviderhubSkuShowNestedResourceTypeThirdOptions(), token);
    }
}