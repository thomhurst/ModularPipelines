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

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Apply(TerraformApplyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformApplyOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Console(TerraformConsoleOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformConsoleOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Destroy(TerraformDestroyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformDestroyOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fmt(TerraformFmtOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformFmtOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ForceUnlock(TerraformForceUnlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Get(TerraformGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Graph(TerraformGraphOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformGraphOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Import(TerraformImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Init(TerraformInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformInitOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Output(TerraformOutputOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformOutputOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Plan(TerraformPlanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformPlanOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Refresh(TerraformRefreshOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformRefreshOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Show(TerraformShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformShowOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateCommand(TerraformStateCommandOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateCommandOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateList(TerraformStateListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateListOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateMv(TerraformStateMvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StatePush(TerraformStatePushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateReplaceProvider(TerraformStateReplaceProviderOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateRm(TerraformStateRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateShow(TerraformStateShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Taint(TerraformTaintOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTaintOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersLock(TerraformTerraformProvidersLockOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersLockOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersMirror(TerraformTerraformProvidersMirrorOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersMirrorOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersSchema(TerraformTerraformProvidersSchemaOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersSchemaOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Untaint(TerraformUntaintOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformUntaintOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Validate(TerraformValidateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformValidateOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Version(TerraformVersionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformVersionOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceDelete(TerraformWorkspaceDeleteOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceList(TerraformWorkspaceListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformWorkspaceListOptions(), null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceNew(TerraformWorkspaceNewOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceSelect(TerraformWorkspaceSelectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token);
    }
}