using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSavingsplans
{
    public AwsSavingsplans(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateSavingsPlan(AwsSavingsplansCreateSavingsPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteQueuedSavingsPlan(AwsSavingsplansDeleteQueuedSavingsPlanOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSavingsPlanRates(AwsSavingsplansDescribeSavingsPlanRatesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSavingsPlans(AwsSavingsplansDescribeSavingsPlansOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSavingsplansDescribeSavingsPlansOptions(), token);
    }

    public async Task<CommandResult> DescribeSavingsPlansOfferingRates(AwsSavingsplansDescribeSavingsPlansOfferingRatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSavingsplansDescribeSavingsPlansOfferingRatesOptions(), token);
    }

    public async Task<CommandResult> DescribeSavingsPlansOfferings(AwsSavingsplansDescribeSavingsPlansOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSavingsplansDescribeSavingsPlansOfferingsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSavingsplansListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSavingsplansTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSavingsplansUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}