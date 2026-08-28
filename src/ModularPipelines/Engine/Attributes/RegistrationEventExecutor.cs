using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Executes registration events for module attributes.
/// </summary>
internal class RegistrationEventExecutor : IRegistrationEventExecutor
{
    private readonly object _lock = new();
    private readonly IModuleAttributeEventService _attributeEventService;
    private readonly IEventHandlerInvoker _eventHandlerInvoker;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly bool _planningSafeOnly;
    private Task? _invocationTask;
    private HashSet<Type>? _registeredModuleTypes;

    public RegistrationEventExecutor(
        IModuleAttributeEventService attributeEventService,
        IEventHandlerInvoker eventHandlerInvoker,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IConfiguration configuration,
        IHostEnvironment environment)
        : this(
            attributeEventService,
            eventHandlerInvoker,
            dependencyRegistry,
            metadataRegistry,
            configuration,
            environment,
            planningSafeOnly: false)
    {
    }

    internal RegistrationEventExecutor(
        IModuleAttributeEventService attributeEventService,
        IEventHandlerInvoker eventHandlerInvoker,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IConfiguration configuration,
        IHostEnvironment environment,
        bool planningSafeOnly)
    {
        _attributeEventService = attributeEventService;
        _eventHandlerInvoker = eventHandlerInvoker;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _configuration = configuration;
        _environment = environment;
        _planningSafeOnly = planningSafeOnly;
    }

    public Task InvokeRegistrationEventsAsync(IEnumerable<IModule> modules)
    {
        var moduleArray = modules.ToArray();

        lock (_lock)
        {
            if (_invocationTask is not null)
            {
                var missingModuleTypes = moduleArray
                    .Select(module => module.GetType())
                    .Where(moduleType => !_registeredModuleTypes!.Contains(moduleType))
                    .Distinct()
                    .ToArray();
                if (missingModuleTypes.Length > 0)
                {
                    throw new InvalidOperationException(
                        "Registration events were initialized without these module types: "
                        + string.Join(", ", missingModuleTypes.Select(moduleType => moduleType.Name)));
                }

                return _invocationTask;
            }

            // Discovery invokes registration events early so dynamic dependencies can affect filtering.
            // Executors invoke this service again, so share the first invocation rather than running handlers twice.
            _registeredModuleTypes = moduleArray
                .Select(module => module.GetType())
                .ToHashSet();
            return _invocationTask = InvokeRegistrationEventsInternalAsync(moduleArray);
        }
    }

    private async Task InvokeRegistrationEventsInternalAsync(IReadOnlyList<IModule> modules)
    {
        var registeredModuleTypes = modules.Select(module => module.GetType()).ToArray();

        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var handlers = GetRegistrationHandlers(moduleType);

            if (handlers.Length == 0)
            {
                continue;
            }

            var context = new ModuleRegistrationContext(
                moduleType,
                _planningSafeOnly
                    ? _attributeEventService.GetPlanningAttributes(moduleType)
                    : _attributeEventService.GetAttributes(moduleType),
                _configuration,
                _environment,
                registeredModuleTypes,
                _dependencyRegistry,
                _metadataRegistry);

            await _eventHandlerInvoker.InvokeRegistrationHandlersAsync(handlers, context).ConfigureAwait(false);
        }
    }

    private IModuleRegistrationHandler[] GetRegistrationHandlers(Type moduleType)
    {
        var handlers = _planningSafeOnly
            ? _attributeEventService.GetPlanningRegistrationHandlers(moduleType)
            : _attributeEventService.GetRegistrationHandlers(moduleType);
        return [.. handlers];
    }
}
