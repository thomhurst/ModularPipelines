using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatashare
{
    public AzDatashare(
        AzDatashareAccount account,
        AzDatashareConsumerInvitation consumerInvitation,
        AzDatashareConsumerSourceDataSet consumerSourceDataSet,
        AzDatashareDataSet dataSet,
        AzDatashareDataSetMapping dataSetMapping,
        AzDatashareEmailRegistration emailRegistration,
        AzDatashareInvitation invitation,
        AzDatashareProviderShareSubscription providerShareSubscription,
        AzDatashareShareSubscription shareSubscription,
        AzDatashareSynchronizationSetting synchronizationSetting,
        AzDatashareTrigger trigger,
        ICommand internalCommand
    )
    {
        Account = account;
        ConsumerInvitation = consumerInvitation;
        ConsumerSourceDataSet = consumerSourceDataSet;
        DataSet = dataSet;
        DataSetMapping = dataSetMapping;
        EmailRegistration = emailRegistration;
        Invitation = invitation;
        ProviderShareSubscription = providerShareSubscription;
        ShareSubscription = shareSubscription;
        SynchronizationSetting = synchronizationSetting;
        Trigger = trigger;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDatashareAccount Account { get; }

    public AzDatashareConsumerInvitation ConsumerInvitation { get; }

    public AzDatashareConsumerSourceDataSet ConsumerSourceDataSet { get; }

    public AzDatashareDataSet DataSet { get; }

    public AzDatashareDataSetMapping DataSetMapping { get; }

    public AzDatashareEmailRegistration EmailRegistration { get; }

    public AzDatashareInvitation Invitation { get; }

    public AzDatashareProviderShareSubscription ProviderShareSubscription { get; }

    public AzDatashareShareSubscription ShareSubscription { get; }

    public AzDatashareSynchronizationSetting SynchronizationSetting { get; }

    public AzDatashareTrigger Trigger { get; }

    public async Task<CommandResult> Create(AzDatashareCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatashareDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatashareListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSynchronization(AzDatashareListSynchronizationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSynchronizationDetail(AzDatashareListSynchronizationDetailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatashareShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatashareWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatashareWaitOptions(), token);
    }
}