using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateAwsLogSource(AwsSecuritylakeCreateAwsLogSourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateCustomLogSource(AwsSecuritylakeCreateCustomLogSourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDataLake(AwsSecuritylakeCreateDataLakeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDataLakeExceptionSubscription(AwsSecuritylakeCreateDataLakeExceptionSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDataLakeOrganizationConfiguration(AwsSecuritylakeCreateDataLakeOrganizationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSubscriber(AwsSecuritylakeCreateSubscriberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateSubscriberNotification(AwsSecuritylakeCreateSubscriberNotificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteAwsLogSource(AwsSecuritylakeDeleteAwsLogSourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteCustomLogSource(AwsSecuritylakeDeleteCustomLogSourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDataLake(AwsSecuritylakeDeleteDataLakeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDataLakeExceptionSubscription(AwsSecuritylakeDeleteDataLakeExceptionSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeDeleteDataLakeExceptionSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDataLakeOrganizationConfiguration(AwsSecuritylakeDeleteDataLakeOrganizationConfigurationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSubscriber(AwsSecuritylakeDeleteSubscriberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteSubscriberNotification(AwsSecuritylakeDeleteSubscriberNotificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeregisterDataLakeDelegatedAdministrator(AwsSecuritylakeDeregisterDataLakeDelegatedAdministratorOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeDeregisterDataLakeDelegatedAdministratorOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataLakeExceptionSubscription(AwsSecuritylakeGetDataLakeExceptionSubscriptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeExceptionSubscriptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataLakeOrganizationConfiguration(AwsSecuritylakeGetDataLakeOrganizationConfigurationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeOrganizationConfigurationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDataLakeSources(AwsSecuritylakeGetDataLakeSourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeGetDataLakeSourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetSubscriber(AwsSecuritylakeGetSubscriberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDataLakeExceptions(AwsSecuritylakeListDataLakeExceptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListDataLakeExceptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDataLakes(AwsSecuritylakeListDataLakesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListDataLakesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListLogSources(AwsSecuritylakeListLogSourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListLogSourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListSubscribers(AwsSecuritylakeListSubscribersOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSecuritylakeListSubscribersOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSecuritylakeListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RegisterDataLakeDelegatedAdministrator(AwsSecuritylakeRegisterDataLakeDelegatedAdministratorOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsSecuritylakeTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsSecuritylakeUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDataLake(AwsSecuritylakeUpdateDataLakeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateDataLakeExceptionSubscription(AwsSecuritylakeUpdateDataLakeExceptionSubscriptionOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSubscriber(AwsSecuritylakeUpdateSubscriberOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateSubscriberNotification(AwsSecuritylakeUpdateSubscriberNotificationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}