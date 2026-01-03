using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzScvmmVmTemplateDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzScvmmVmTemplateListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzScvmmVmTemplateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzScvmmVmTemplateUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmTemplateUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmTemplateWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}