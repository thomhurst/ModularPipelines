using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Terraform.Options;

namespace ModularPipelines.Terraform;

[ExcludeFromCodeCoverage]
public class Terraform : ITerraform
{
    private readonly ICommand _command;

    public Terraform(ICommand command)
    {
        _command = command;
    }

    public async Task<CommandResult> Apply(TerraformApplyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformApplyOptions(), token);
    }

    public async Task<CommandResult> Console(TerraformConsoleOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformConsoleOptions(), token);
    }

    public async Task<CommandResult> Destroy(TerraformDestroyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformDestroyOptions(), token);
    }

    public async Task<CommandResult> Fmt(TerraformFmtOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformFmtOptions(), token);
    }

    public async Task<CommandResult> ForceUnlock(TerraformForceUnlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Get(TerraformGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Graph(TerraformGraphOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformGraphOptions(), token);
    }

    public async Task<CommandResult> Import(TerraformImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Init(TerraformInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformInitOptions(), token);
    }

    public async Task<CommandResult> Output(TerraformOutputOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformOutputOptions(), token);
    }

    public async Task<CommandResult> Plan(TerraformPlanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformPlanOptions(), token);
    }

    public async Task<CommandResult> Refresh(TerraformRefreshOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformRefreshOptions(), token);
    }

    public async Task<CommandResult> Show(TerraformShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformShowOptions(), token);
    }

    public async Task<CommandResult> StateCommand(TerraformStateCommandOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateCommandOptions(), token);
    }

    public async Task<CommandResult> StateList(TerraformStateListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateListOptions(), token);
    }

    public async Task<CommandResult> StateMv(TerraformStateMvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StatePush(TerraformStatePushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StateReplaceProvider(TerraformStateReplaceProviderOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StateRm(TerraformStateRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StateShow(TerraformStateShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Taint(TerraformTaintOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTaintOptions(), token);
    }

    public async Task<CommandResult> ProvidersLock(TerraformTerraformProvidersLockOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersLockOptions(), token);
    }

    public async Task<CommandResult> ProvidersMirror(TerraformTerraformProvidersMirrorOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersMirrorOptions(), token);
    }

    public async Task<CommandResult> ProvidersSchema(TerraformTerraformProvidersSchemaOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersSchemaOptions(), token);
    }

    public async Task<CommandResult> Untaint(TerraformUntaintOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformUntaintOptions(), token);
    }

    public async Task<CommandResult> Validate(TerraformValidateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformValidateOptions(), token);
    }

    public async Task<CommandResult> Version(TerraformVersionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformVersionOptions(), token);
    }

    public async Task<CommandResult> WorkspaceDelete(TerraformWorkspaceDeleteOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WorkspaceList(TerraformWorkspaceListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformWorkspaceListOptions(), token);
    }

    public async Task<CommandResult> WorkspaceNew(TerraformWorkspaceNewOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> WorkspaceSelect(TerraformWorkspaceSelectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
