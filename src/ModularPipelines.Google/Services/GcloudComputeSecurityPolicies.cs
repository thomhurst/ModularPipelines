using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute")]
public class GcloudComputeSecurityPolicies
{
    public GcloudComputeSecurityPolicies(
        GcloudComputeSecurityPoliciesRules rules,
        ICommand internalCommand
    )
    {
        Rules = rules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeSecurityPoliciesRules Rules { get; }

    public async Task<CommandResult> AddLayer7DdosDefenseThresholdConfig(GcloudComputeSecurityPoliciesAddLayer7DdosDefenseThresholdConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddUserDefinedField(GcloudComputeSecurityPoliciesAddUserDefinedFieldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(GcloudComputeSecurityPoliciesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeSecurityPoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeSecurityPoliciesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(GcloudComputeSecurityPoliciesExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudComputeSecurityPoliciesImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeSecurityPoliciesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPreconfiguredExpressionSets(GcloudComputeSecurityPoliciesListPreconfiguredExpressionSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeSecurityPoliciesListPreconfiguredExpressionSetsOptions(), token);
    }

    public async Task<CommandResult> RemoveLayer7DdosDefenseThresholdConfig(GcloudComputeSecurityPoliciesRemoveLayer7DdosDefenseThresholdConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveUserDefinedField(GcloudComputeSecurityPoliciesRemoveUserDefinedFieldOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudComputeSecurityPoliciesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}