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
        return await _command.ExecuteCommandLineTool(options ?? new TerraformApplyOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Console(TerraformConsoleOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformConsoleOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Destroy(TerraformDestroyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformDestroyOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Fmt(TerraformFmtOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformFmtOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ForceUnlock(TerraformForceUnlockOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Get(TerraformGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Graph(TerraformGraphOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformGraphOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Import(TerraformImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Init(TerraformInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformInitOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Output(TerraformOutputOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformOutputOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Plan(TerraformPlanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformPlanOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Refresh(TerraformRefreshOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformRefreshOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Show(TerraformShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformShowOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateCommand(TerraformStateCommandOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateCommandOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateList(TerraformStateListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformStateListOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateMv(TerraformStateMvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StatePush(TerraformStatePushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateReplaceProvider(TerraformStateReplaceProviderOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateRm(TerraformStateRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> StateShow(TerraformStateShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Taint(TerraformTaintOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTaintOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersLock(TerraformTerraformProvidersLockOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersLockOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersMirror(TerraformTerraformProvidersMirrorOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersMirrorOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> ProvidersSchema(TerraformTerraformProvidersSchemaOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformTerraformProvidersSchemaOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Untaint(TerraformUntaintOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformUntaintOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Validate(TerraformValidateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformValidateOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Version(TerraformVersionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformVersionOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceDelete(TerraformWorkspaceDeleteOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceList(TerraformWorkspaceListOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new TerraformWorkspaceListOptions(), token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceNew(TerraformWorkspaceNewOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> WorkspaceSelect(TerraformWorkspaceSelectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}