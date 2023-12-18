using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware")]
public class AzConnectedvmwareVmTemplate
{
    public AzConnectedvmwareVmTemplate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzConnectedvmwareVmTemplateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedvmwareVmTemplateDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmTemplateDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedvmwareVmTemplateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmTemplateListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConnectedvmwareVmTemplateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVmTemplateShowOptions(), token);
    }
}