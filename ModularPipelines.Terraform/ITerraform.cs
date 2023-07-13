using ModularPipelines.Models;
using ModularPipelines.Terraform.Options;

namespace ModularPipelines.Terraform;

public interface ITerraform
{
    Task<CommandResult> Apply(TerraformApplyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Console(TerraformConsoleOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Destroy(TerraformDestroyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Fmt(TerraformFmtOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ForceUnlock(TerraformForceUnlockOptions options, CancellationToken token = default);

    Task<CommandResult> Get(TerraformGetOptions options, CancellationToken token = default);

    Task<CommandResult> Graph(TerraformGraphOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Import(TerraformImportOptions options, CancellationToken token = default);

    Task<CommandResult> Init(TerraformInitOptions? options = default, CancellationToken token = default);
    
    Task<CommandResult> Output(TerraformOutputOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Plan(TerraformPlanOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Refresh(TerraformRefreshOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Show(TerraformShowOptions? options = default, CancellationToken token = default);

    Task<CommandResult> StateCommand(TerraformStateCommandOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> StateList(TerraformStateListOptions? options = default, CancellationToken token = default);

    Task<CommandResult> StateMv(TerraformStateMvOptions options, CancellationToken token = default);

    Task<CommandResult> StatePush(TerraformStatePushOptions options, CancellationToken token = default);

    Task<CommandResult> StateReplaceProvider(TerraformStateReplaceProviderOptions options,
        CancellationToken token = default);

    Task<CommandResult> StateRm(TerraformStateRmOptions options, CancellationToken token = default);

    Task<CommandResult> StateShow(TerraformStateShowOptions options, CancellationToken token = default);

    Task<CommandResult> Taint(TerraformTaintOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ProvidersLock(TerraformTerraformProvidersLockOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ProvidersMirror(TerraformTerraformProvidersMirrorOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> ProvidersSchema(TerraformTerraformProvidersSchemaOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> Untaint(TerraformUntaintOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Validate(TerraformValidateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Version(TerraformVersionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> WorkspaceDelete(TerraformWorkspaceDeleteOptions options, CancellationToken token = default);

    Task<CommandResult> WorkspaceList(TerraformWorkspaceListOptions? options = default,
        CancellationToken token = default);

    Task<CommandResult> WorkspaceNew(TerraformWorkspaceNewOptions options, CancellationToken token = default);

    Task<CommandResult> WorkspaceSelect(TerraformWorkspaceSelectOptions options, CancellationToken token = default);
}