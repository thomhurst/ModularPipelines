using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> CreateSlackChannelConfiguration(AwsSupportAppCreateSlackChannelConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAccountAlias(AwsSupportAppDeleteAccountAliasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppDeleteAccountAliasOptions(), token);
    }

    public async Task<CommandResult> DeleteSlackChannelConfiguration(AwsSupportAppDeleteSlackChannelConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSlackWorkspaceConfiguration(AwsSupportAppDeleteSlackWorkspaceConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountAlias(AwsSupportAppGetAccountAliasOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppGetAccountAliasOptions(), token);
    }

    public async Task<CommandResult> ListSlackChannelConfigurations(AwsSupportAppListSlackChannelConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppListSlackChannelConfigurationsOptions(), token);
    }

    public async Task<CommandResult> ListSlackWorkspaceConfigurations(AwsSupportAppListSlackWorkspaceConfigurationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportAppListSlackWorkspaceConfigurationsOptions(), token);
    }

    public async Task<CommandResult> PutAccountAlias(AwsSupportAppPutAccountAliasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterSlackWorkspaceForOrganization(AwsSupportAppRegisterSlackWorkspaceForOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSlackChannelConfiguration(AwsSupportAppUpdateSlackChannelConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}