using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Executors.ModuleHandlers;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Serialization;

namespace ModularPipelines.Modules;

/// <summary>
/// A base class for all modules.
/// </summary>
[JsonConverter(typeof(TypeDiscriminatorConverter<ModuleBase>))]
public abstract partial class ModuleBase : ITypeDiscriminator
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ModuleBase"/> class.
    /// </summary>
    [ModuleMethodMarker]
    protected ModuleBase()
    {
        TypeDiscriminator = GetType().AssemblyQualifiedName!;
    }

    /// <summary>
    /// Gets the Type Discriminator.
    /// Important this is defined at the beginning of the class.
    /// </summary>
    [JsonInclude]
    public string TypeDiscriminator { get; private set; }

    internal bool IsStarted { get; private protected set; }

    internal List<Type> DependentModules { get; } = [];

    internal abstract IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetModuleDependencies();

    internal abstract ICancellationHandler CancellationHandler { get; }

    internal abstract ISkipHandler SkipHandler { get; }

    internal abstract IHookHandler HookHandler { get; }

    internal abstract IStatusHandler StatusHandler { get; }

    internal abstract IErrorHandler ErrorHandler { get; }

    internal abstract void TryCancel();

    private IPipelineContext? _context; // Late Initialisation

    /// <summary>
    /// Gets or sets the pipeline context.
    /// </summary>
    /// <exception cref="ModuleNotInitializedException">Thrown if this object is used before Initialise is called.</exception>
    [JsonIgnore]
    protected internal IPipelineContext Context
    {
        get
        {
            if (_context == null)
            {
                throw new ModuleNotInitializedException(GetType());
            }

            return _context;
        }

        protected set
        {
            _context = value;
            OnInitialised?.Invoke(this, EventArgs.Empty);
        }
    }

    internal readonly Task StartTask = new(() => { });

    [JsonInclude]
    internal SkipDecision SkipResult { get; set; } = SkipDecision.DoNotSkip;

    internal abstract Task ExecutionTask { get; }

    internal abstract Task StartInternal();

    internal readonly CancellationTokenSource ModuleCancellationTokenSource = new();

    internal readonly Stopwatch Stopwatch = new();

    /// <summary>
    /// Gets the start time of the module.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset StartTime { get; internal set; }

    /// <summary>
    /// Gets the end time of the module.
    /// </summary>
    [JsonInclude]
    public DateTimeOffset EndTime { get; internal set; }

    /// <summary>
    /// Gets the duration of the module. This will be set after the module has finished.
    /// </summary>
    [JsonInclude]
    public TimeSpan Duration { get; internal set; }

    /// <summary>
    /// Gets the status of the module.
    /// </summary>
    [JsonInclude]
    public Status Status { get; internal set; } = Status.NotYetStarted;

    internal Exception? Exception { get; set; }

    internal abstract ModuleBase Initialize(IPipelineContext context);

    internal readonly object SubModuleBasesLock = new();
    internal readonly List<SubModuleBase> SubModuleBases = new();

    internal EventHandler<SubModuleBase>? OnSubModuleCreated;

    internal abstract Task<IModuleResult> GetModuleResult();

    /// <summary>
    /// Starts a Sub Module which will display in the pipeline progress in the console.
    /// </summary>
    /// <typeparam name="T">Any data to return from the submodule.</typeparam>
    /// <param name="name">The name of the submodule.</param>
    /// <param name="action">The delegate that the submodule should execute.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    protected Task<T> SubModule<T>(string name, Func<Task<T>> action)
    {
        lock (SubModuleBasesLock)
        {
            var existingSubModule = SubModuleBases.Find(x => x.Name == name);
            if (existingSubModule != null)
            {
                if (existingSubModule.Status == Status.Successful && existingSubModule is SubModule<T> typedSubmodule)
                {
                    return typedSubmodule.SubModuleResultTaskCompletionSource.Task;
                }

                if (existingSubModule.Status is Status.NotYetStarted or Status.Processing)
                {
                    throw new Exception("Use Distinct names for SubModules");
                }
            }

            var submodule = new SubModule<T>(GetType(), name);

            SubModuleBases.Add(submodule);

            OnSubModuleCreated?.Invoke(this, submodule);

            return submodule.Execute(action);
        }
    }

    /// <summary>
    /// Starts a Sub Module which will display in the pipeline progress in the console.
    /// </summary>
    /// <param name="name">The name of the submodule.</param>
    /// <param name="action">The delegate that the submodule should execute.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    protected async Task SubModule(string name, Action action)
    {
        await SubModule(name, () => Task.Run(action));
    }

    /// <summary>
    /// Starts a Sub Module which will display in the pipeline progress in the console.
    /// </summary>
    /// <param name="name">The name of the submodule.</param>
    /// <param name="action">The delegate that the submodule should execute.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    protected async Task<T> SubModule<T>(string name, Func<T> action)
    {
        return await SubModule(name, () => Task.Run(action));
    }

    /// <summary>
    /// Starts a Sub Module which will display in the pipeline progress in the console.
    /// </summary>
    /// <param name="name">The name of the submodule.</param>
    /// <param name="action">The delegate that the submodule should execute.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    protected Task SubModule(string name, Func<Task> action)
    {
        return SubModule(name, async () =>
        {
            await action();
            return 0;
        });
    }

    protected EventHandler? OnInitialised { get; set; }
}

/// <summary>
/// A base class for all modules.
/// </summary>
/// <typeparam name="T">Any data to return from the module.</typeparam>
public abstract class ModuleBase<T> : ModuleBase
{
    internal readonly TaskCompletionSource<ModuleResult<T>> ModuleResultTaskCompletionSource = new();

    internal override void TryCancel()
    {
        ModuleResultTaskCompletionSource.TrySetCanceled();
    }

    internal abstract IHistoryHandler<T> HistoryHandler { get; }

    /// <summary>
    /// The awaiter used to return the result of the module when awaited.
    /// </summary>
    /// <returns>The result of the ExecuteAsync method.</returns>
    public TaskAwaiter<ModuleResult<T>> GetAwaiter()
    {
        return ModuleResultTaskCompletionSource.Task.GetAwaiter();
    }

    /// <summary>
    /// Used to return no result in a module.
    /// </summary>
    /// <returns>Nothing.</returns>
    protected Task<T?> NothingAsync()
    {
        return Task.FromResult(default(T?));
    }

    /// <summary>
    /// The core logic of the module goes here.
    /// </summary>
    /// <param name="context">A pipeline context object provided by the pipeline.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline has failed, or the module timeout has exceeded.</param>
    /// <returns>{T}.</returns>
    [ModuleMethodMarker]
    protected abstract Task<T?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken);
}