using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsHealth
{
    public AwsHealth(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DescribeAffectedAccountsForOrganization(AwsHealthDescribeAffectedAccountsForOrganizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAffectedEntities(AwsHealthDescribeAffectedEntitiesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeAffectedEntitiesForOrganization(AwsHealthDescribeAffectedEntitiesForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeAffectedEntitiesForOrganizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEntityAggregates(AwsHealthDescribeEntityAggregatesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEntityAggregatesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEntityAggregatesForOrganization(AwsHealthDescribeEntityAggregatesForOrganizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventAggregates(AwsHealthDescribeEventAggregatesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventDetails(AwsHealthDescribeEventDetailsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventDetailsForOrganization(AwsHealthDescribeEventDetailsForOrganizationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventTypes(AwsHealthDescribeEventTypesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventTypesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEvents(AwsHealthDescribeEventsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeEventsForOrganization(AwsHealthDescribeEventsForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventsForOrganizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DescribeHealthServiceStatusForOrganization(AwsHealthDescribeHealthServiceStatusForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeHealthServiceStatusForOrganizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisableHealthServiceAccessForOrganization(AwsHealthDisableHealthServiceAccessForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDisableHealthServiceAccessForOrganizationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> EnableHealthServiceAccessForOrganization(AwsHealthEnableHealthServiceAccessForOrganizationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthEnableHealthServiceAccessForOrganizationOptions(), executionOptions, cancellationToken);
    }
}