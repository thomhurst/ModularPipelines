using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm")]
public class AzScvmmVmTemplate
{
    public AzScvmmVmTemplate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzScvmmVmTemplateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmVmTemplateDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzScvmmVmTemplateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateListOptions(), token);
    }

    public async Task<CommandResult> Show(AzScvmmVmTemplateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmVmTemplateUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmTemplateWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}