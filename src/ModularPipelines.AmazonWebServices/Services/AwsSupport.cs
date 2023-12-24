using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSupport
{
    public AwsSupport(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AddAttachmentsToSet(AwsSupportAddAttachmentsToSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddCommunicationToCase(AwsSupportAddCommunicationToCaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCase(AwsSupportCreateCaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAttachment(AwsSupportDescribeAttachmentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCases(AwsSupportDescribeCasesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportDescribeCasesOptions(), token);
    }

    public async Task<CommandResult> DescribeCommunications(AwsSupportDescribeCommunicationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCreateCaseOptions(AwsSupportDescribeCreateCaseOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeServices(AwsSupportDescribeServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportDescribeServicesOptions(), token);
    }

    public async Task<CommandResult> DescribeSeverityLevels(AwsSupportDescribeSeverityLevelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportDescribeSeverityLevelsOptions(), token);
    }

    public async Task<CommandResult> DescribeSupportedLanguages(AwsSupportDescribeSupportedLanguagesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustedAdvisorCheckRefreshStatuses(AwsSupportDescribeTrustedAdvisorCheckRefreshStatusesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustedAdvisorCheckResult(AwsSupportDescribeTrustedAdvisorCheckResultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustedAdvisorCheckSummaries(AwsSupportDescribeTrustedAdvisorCheckSummariesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeTrustedAdvisorChecks(AwsSupportDescribeTrustedAdvisorChecksOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RefreshTrustedAdvisorCheck(AwsSupportRefreshTrustedAdvisorCheckOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ResolveCase(AwsSupportResolveCaseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSupportResolveCaseOptions(), token);
    }
}