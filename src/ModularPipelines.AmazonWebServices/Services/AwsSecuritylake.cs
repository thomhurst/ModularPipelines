using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSecuritylake
{
    public AwsSecuritylake(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateAwsLogSource(AwsSecuritylakeCreateAwsLogSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomLogSource(AwsSecuritylakeCreateCustomLogSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataLake(AwsSecuritylakeCreateDataLakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataLakeExceptionSubscription(AwsSecuritylakeCreateDataLakeExceptionSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDataLakeOrganizationConfiguration(AwsSecuritylakeCreateDataLakeOrganizationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubscriber(AwsSecuritylakeCreateSubscriberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSubscriberNotification(AwsSecuritylakeCreateSubscriberNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteAwsLogSource(AwsSecuritylakeDeleteAwsLogSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomLogSource(AwsSecuritylakeDeleteCustomLogSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataLake(AwsSecuritylakeDeleteDataLakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDataLakeExceptionSubscription(AwsSecuritylakeDeleteDataLakeExceptionSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeDeleteDataLakeExceptionSubscriptionOptions(), token);
    }

    public async Task<CommandResult> DeleteDataLakeOrganizationConfiguration(AwsSecuritylakeDeleteDataLakeOrganizationConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubscriber(AwsSecuritylakeDeleteSubscriberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSubscriberNotification(AwsSecuritylakeDeleteSubscriberNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeregisterDataLakeDelegatedAdministrator(AwsSecuritylakeDeregisterDataLakeDelegatedAdministratorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeDeregisterDataLakeDelegatedAdministratorOptions(), token);
    }

    public async Task<CommandResult> GetDataLakeExceptionSubscription(AwsSecuritylakeGetDataLakeExceptionSubscriptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeExceptionSubscriptionOptions(), token);
    }

    public async Task<CommandResult> GetDataLakeOrganizationConfiguration(AwsSecuritylakeGetDataLakeOrganizationConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeOrganizationConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetDataLakeSources(AwsSecuritylakeGetDataLakeSourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeSourcesOptions(), token);
    }

    public async Task<CommandResult> GetSubscriber(AwsSecuritylakeGetSubscriberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDataLakeExceptions(AwsSecuritylakeListDataLakeExceptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListDataLakeExceptionsOptions(), token);
    }

    public async Task<CommandResult> ListDataLakes(AwsSecuritylakeListDataLakesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListDataLakesOptions(), token);
    }

    public async Task<CommandResult> ListLogSources(AwsSecuritylakeListLogSourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListLogSourcesOptions(), token);
    }

    public async Task<CommandResult> ListSubscribers(AwsSecuritylakeListSubscribersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListSubscribersOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSecuritylakeListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterDataLakeDelegatedAdministrator(AwsSecuritylakeRegisterDataLakeDelegatedAdministratorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSecuritylakeTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSecuritylakeUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataLake(AwsSecuritylakeUpdateDataLakeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDataLakeExceptionSubscription(AwsSecuritylakeUpdateDataLakeExceptionSubscriptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubscriber(AwsSecuritylakeUpdateSubscriberOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSubscriberNotification(AwsSecuritylakeUpdateSubscriberNotificationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}