using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> DescribeAffectedAccountsForOrganization(AwsHealthDescribeAffectedAccountsForOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAffectedEntities(AwsHealthDescribeAffectedEntitiesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAffectedEntitiesForOrganization(AwsHealthDescribeAffectedEntitiesForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeAffectedEntitiesForOrganizationOptions(), token);
    }

    public async Task<CommandResult> DescribeEntityAggregates(AwsHealthDescribeEntityAggregatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEntityAggregatesOptions(), token);
    }

    public async Task<CommandResult> DescribeEntityAggregatesForOrganization(AwsHealthDescribeEntityAggregatesForOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventAggregates(AwsHealthDescribeEventAggregatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventDetails(AwsHealthDescribeEventDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventDetailsForOrganization(AwsHealthDescribeEventDetailsForOrganizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeEventTypes(AwsHealthDescribeEventTypesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventTypesOptions(), token);
    }

    public async Task<CommandResult> DescribeEvents(AwsHealthDescribeEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventsOptions(), token);
    }

    public async Task<CommandResult> DescribeEventsForOrganization(AwsHealthDescribeEventsForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeEventsForOrganizationOptions(), token);
    }

    public async Task<CommandResult> DescribeHealthServiceStatusForOrganization(AwsHealthDescribeHealthServiceStatusForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDescribeHealthServiceStatusForOrganizationOptions(), token);
    }

    public async Task<CommandResult> DisableHealthServiceAccessForOrganization(AwsHealthDisableHealthServiceAccessForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthDisableHealthServiceAccessForOrganizationOptions(), token);
    }

    public async Task<CommandResult> EnableHealthServiceAccessForOrganization(AwsHealthEnableHealthServiceAccessForOrganizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsHealthEnableHealthServiceAccessForOrganizationOptions(), token);
    }
}