using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSupportApp
{
    public AwsSupportApp(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateSlackChannelConfiguration(AwsSupportAppCreateSlackChannelConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAccountAlias(AwsSupportAppDeleteAccountAliasOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppDeleteAccountAliasOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSlackChannelConfiguration(AwsSupportAppDeleteSlackChannelConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSlackWorkspaceConfiguration(AwsSupportAppDeleteSlackWorkspaceConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAccountAlias(AwsSupportAppGetAccountAliasOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppGetAccountAliasOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSlackChannelConfigurations(AwsSupportAppListSlackChannelConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppListSlackChannelConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSlackWorkspaceConfigurations(AwsSupportAppListSlackWorkspaceConfigurationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppListSlackWorkspaceConfigurationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAccountAlias(AwsSupportAppPutAccountAliasOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterSlackWorkspaceForOrganization(AwsSupportAppRegisterSlackWorkspaceForOrganizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSlackChannelConfiguration(AwsSupportAppUpdateSlackChannelConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}