using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsCodeguruReviewer
{
    public AwsCodeguruReviewer(
        AwsCodeguruReviewerWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsCodeguruReviewerWait Wait { get; }

    public async Task<CommandResult> AssociateRepository(AwsCodeguruReviewerAssociateRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCodeReview(AwsCodeguruReviewerCreateCodeReviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeCodeReview(AwsCodeguruReviewerDescribeCodeReviewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRecommendationFeedback(AwsCodeguruReviewerDescribeRecommendationFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeRepositoryAssociation(AwsCodeguruReviewerDescribeRepositoryAssociationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateRepository(AwsCodeguruReviewerDisassociateRepositoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCodeReviews(AwsCodeguruReviewerListCodeReviewsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecommendationFeedback(AwsCodeguruReviewerListRecommendationFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecommendations(AwsCodeguruReviewerListRecommendationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRepositoryAssociations(AwsCodeguruReviewerListRepositoryAssociationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsCodeguruReviewerListRepositoryAssociationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsCodeguruReviewerListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutRecommendationFeedback(AwsCodeguruReviewerPutRecommendationFeedbackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsCodeguruReviewerTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsCodeguruReviewerUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}