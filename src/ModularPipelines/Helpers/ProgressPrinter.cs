using System.Diagnostics.CodeAnalysis;
using Mediator;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

/// <summary>
/// Coordinates progress display and handles module execution notifications.
/// Delegates to <see cref="IProgressDisplay"/> for rendering and <see cref="IResultsPrinter"/> for results.
/// </summary>
/// <remarks>
/// <para>
/// <b>Architecture:</b> This class follows the Single Responsibility Principle by acting
/// as a coordinator that connects the notification system to display components.
/// The actual rendering logic is handled by injected dependencies.
/// </para>
/// <para>
/// <b>Thread Safety:</b> Thread safety is managed by the underlying <see cref="IProgressDisplay"/>
/// implementation. Notification handlers can be called concurrently from multiple threads.
/// </para>
/// </remarks>
[ExcludeFromCodeCoverage]
internal class ProgressPrinter : IProgressPrinter,
    INotificationHandler<ModuleStartedNotification>,
    INotificationHandler<ModuleCompletedNotification>,
    INotificationHandler<ModuleSkippedNotification>,
    INotificationHandler<SubModuleCreatedNotification>,
    INotificationHandler<SubModuleCompletedNotification>
{
    private readonly IProgressDisplay _progressDisplay;
    private readonly IResultsPrinter _resultsPrinter;

    public ProgressPrinter(
        IProgressDisplay progressDisplay,
        IResultsPrinter resultsPrinter)
    {
        _progressDisplay = progressDisplay;
        _resultsPrinter = resultsPrinter;
    }

    public Task PrintProgress(OrganizedModules organizedModules, CancellationToken cancellationToken)
    {
        return _progressDisplay.RunAsync(organizedModules, cancellationToken);
    }

    public void PrintResults(PipelineSummary pipelineSummary)
    {
        _resultsPrinter.PrintResults(pipelineSummary);
    }

    public ValueTask Handle(ModuleStartedNotification notification, CancellationToken cancellationToken)
    {
        _progressDisplay.OnModuleStarted(notification.ModuleState, notification.EstimatedDuration);
        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(ModuleCompletedNotification notification, CancellationToken cancellationToken)
    {
        _progressDisplay.OnModuleCompleted(notification.ModuleState, notification.IsSuccessful);
        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(ModuleSkippedNotification notification, CancellationToken cancellationToken)
    {
        _progressDisplay.OnModuleSkipped(notification.ModuleState);
        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(SubModuleCreatedNotification notification, CancellationToken cancellationToken)
    {
        _progressDisplay.OnSubModuleCreated(
            notification.ParentModule,
            notification.SubModule,
            notification.EstimatedDuration);
        return ValueTask.CompletedTask;
    }

    public ValueTask Handle(SubModuleCompletedNotification notification, CancellationToken cancellationToken)
    {
        _progressDisplay.OnSubModuleCompleted(notification.SubModule, notification.IsSuccessful);
        return ValueTask.CompletedTask;
    }
}
