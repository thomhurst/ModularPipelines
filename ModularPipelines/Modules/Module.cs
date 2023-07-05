using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public abstract class Module : Module<IDictionary<string, object>>
{
}

/// <summary>
/// The base class from which all custom modules should inherit.
/// </summary>
/// <typeparam name="T">The type of result object this module will return from its ExecuteAsync method.</typeparam>
public abstract partial class Module<T> : ModuleBase<T>
{
    private readonly Stopwatch _stopwatch = new();

    internal List<DependsOnAttribute> DependentModules { get; } = new();

    private bool _initialized;
    private IModuleContext _context = null!; // Late Initialisation

    protected Module()
    {
        foreach (var customAttribute in GetType().GetCustomAttributesIncludingBaseInterfaces<DependsOnAttribute>())
        {
            AddDependency( customAttribute );
        }
    }

    private void AddDependency( DependsOnAttribute dependsOnAttribute )
    {
        var type = dependsOnAttribute.Type;

        if (type == GetType())
        {
            throw new ModuleReferencingSelfException( "A module cannot depend on itself" );
        }

        if (!type.IsAssignableTo( typeof( ModuleBase ) ))
        {
            throw new Exception( $"{type.FullName} must be a module to add as a dependency" );
        }

        DependentModules.Add( dependsOnAttribute );
    }

    internal override async Task StartAsync()
    {
        if (!_initialized)
        {
            throw new ModuleNotInitializedException( GetType() );
        }

        if (ModuleRunType != ModuleRunType.AlwaysRun)
        {
            _context.EngineCancellationToken.Token.Register( ModuleCancellationTokenSource.Cancel );
        }

        try
        {
            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            await WaitForModuleDependencies();

            var shouldSkipModule = await ShouldSkip( _context );

            if (shouldSkipModule && await UseResultFromHistoryIfSkipped( _context ))
            {
                await SetupModuleFromHistory();
                return;
            }

            if (shouldSkipModule)
            {
                SetSkipped();
                return;
            }

            ModuleCancellationTokenSource.Token.ThrowIfCancellationRequested();

            await OnBeforeExecute( _context );

            StartTask.Start( TaskScheduler.Default );

            Status = Status.Processing;
            StartTime = DateTimeOffset.UtcNow;

            var timeoutExceptionTask = Task.CompletedTask;

            if (Timeout != TimeSpan.Zero)
            {
                ModuleCancellationTokenSource.CancelAfter( Timeout );
                timeoutExceptionTask = Task.Delay( Timeout + TimeSpan.FromSeconds( 30 ), ModuleCancellationTokenSource.Token );
            }

            _stopwatch.Start();

            var executeAsyncTask = ExecuteAsync( _context, ModuleCancellationTokenSource.Token );

            // Will throw a timeout exception if configured and timeout is reached
            await Task.WhenAny( timeoutExceptionTask, executeAsyncTask );

            var moduleResult = await executeAsyncTask ?? ModuleResult.Empty<T>();
            moduleResult.ModuleName = GetType().Name;

            await _context.ModuleResultRepository.SaveResultAsync( this, moduleResult );

            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;

            Status = Status.Successful;
            EndTime = DateTimeOffset.UtcNow;

            _context.Logger.LogDebug( "Module Succeeded after {Duration}", Duration );

            TaskCompletionSource.SetResult( moduleResult );
        }
        catch (Exception exception)
        {
            _stopwatch.Stop();
            Duration = _stopwatch.Elapsed;
            EndTime = DateTimeOffset.UtcNow;

            _context.Logger.LogError( exception, "Module Failed after {Duration}", Duration );

            if (exception is TaskCanceledException or OperationCanceledException
                && ModuleCancellationTokenSource.IsCancellationRequested && !_context.EngineCancellationToken.IsCancellationRequested)
            {
                _context.Logger.LogDebug( "Module timed out: {ModuleType}", GetType().FullName );

                Status = Status.TimedOut;
            }
            else
            {
                Status = Status.Failed;
            }

            if (await ShouldIgnoreFailures( _context, exception ))
            {
                var moduleResult = ModuleResult.FromException<T>( exception );
                moduleResult.ModuleName = GetType().Name;

                await _context.ModuleResultRepository.SaveResultAsync( this, moduleResult );

                TaskCompletionSource.SetResult( moduleResult );
            }
            else
            {
                TaskCompletionSource.SetException( exception );
                throw;
            }
        }
        finally
        {
            await OnAfterExecute( _context );
        }
    }

    internal override ModuleBase Initialize( IModuleContext context )
    {
        context.FetchLogger( GetType() );
        _context = context;
        _initialized = true;
        return this;
    }

    private async Task SetupModuleFromHistory()
    {
        Status = Status.Successful;

        var result = await _context.ModuleResultRepository.GetResultAsync<T>( this );

        if (result == null)
        {
            SetSkipped();
            return;
        }

        var utcNow = DateTimeOffset.UtcNow;

        StartTime = utcNow;
        EndTime = utcNow;

        StartTask.Start( TaskScheduler.Default );
        TaskCompletionSource.SetResult( result );
    }

    protected TModule GetModule<TModule>() where TModule : ModuleBase
    {
        var module = GetModuleIfRegistered<TModule>();

        if (module is null)
        {
            throw new ModuleNotRegisteredException(
                $"The module {typeof( TModule )} has not been registered", null );
        }

        return module;
    }

    protected TModule? GetModuleIfRegistered<TModule>() where TModule : ModuleBase
    {
        if (!_initialized)
        {
            throw new ModuleNotInitializedException( GetType() );
        }

        if (typeof( TModule ) == GetType())
        {
            throw new ModuleReferencingSelfException( "A module cannot get itself" );
        }

        return _context.GetModule<TModule>();
    }

    private async Task WaitForModuleDependencies()
    {
        if (!DependentModules.Any())
        {
            return;
        }

        try
        {
            var modules = DependentModules.Select( x => _context.GetModule( x.Type ) ).ToList();

            var tasks = DependentModules.Select( dependsOnAttribute =>
            {
                var module = _context.GetModule( dependsOnAttribute.Type );

                if (dependsOnAttribute.IgnoreIfNotRegistered && module is null)
                {
                    return Task.CompletedTask;
                }

                if (module is null)
                {
                    throw new ModuleNotRegisteredException(
                        $"The module {dependsOnAttribute.Type.Name} has not been registered", null );
                }

                return module.ResultTaskInternal;
            } );

            await Task.WhenAll( tasks );

            foreach (var moduleBase in modules.OfType<ModuleBase>())
            {
                var result = await moduleBase.ResultTaskInternal;
                if (IsSkippedResult( result ))
                {
                    throw new DependsOnSkippedModuleException( this, moduleBase );
                }
            }
        }
        catch (Exception e) when (ModuleRunType == ModuleRunType.AlwaysRun)
        {
            _context.Logger.LogError( e, "Ignoring Exception due to 'AlwaysRun' set" );
        }
    }

    private bool IsSkippedResult( object result )
    {
        var resultType = result.GetType();
        return resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof( SkippedModuleResult<> );
    }

    internal override void SetSkipped()
    {
        Status = Status.Skipped;

        IgnoreTask.Start( TaskScheduler.Default );

        TaskCompletionSource.SetResult( new SkippedModuleResult<T>() );

        _context.Logger.LogInformation( "{Module} Ignored", GetType().Name );
    }
}
