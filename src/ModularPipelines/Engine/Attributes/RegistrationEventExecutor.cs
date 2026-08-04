using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Executes registration events for module attributes.
/// </summary>
internal class RegistrationEventExecutor : IRegistrationEventExecutor
{
    private readonly object _lock = new();
    private readonly IModuleAttributeEventService _attributeEventService;
    private readonly IAttributeEventInvoker _attributeEventInvoker;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private readonly bool _planningSafeOnly;
    private Task? _invocationTask;
    private HashSet<Type>? _registeredModuleTypes;

    public RegistrationEventExecutor(
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IConfiguration configuration,
        IHostEnvironment environment)
        : this(
            attributeEventService,
            attributeEventInvoker,
            dependencyRegistry,
            metadataRegistry,
            configuration,
            environment,
            planningSafeOnly: false)
    {
    }

    internal RegistrationEventExecutor(
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IConfiguration configuration,
        IHostEnvironment environment,
        bool planningSafeOnly)
    {
        _attributeEventService = attributeEventService;
        _attributeEventInvoker = attributeEventInvoker;
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
            // Executors invoke this service again, so share the first invocation rather than running receivers twice.
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
            var receivers = GetRegistrationReceivers(moduleType);

            if (receivers.Length == 0)
            {
                continue;
            }

            var context = new ModuleRegistrationContext(
                moduleType,
                _planningSafeOnly
                    ? [.. receivers.OfType<Attribute>()]
                    : _attributeEventService.GetAttributes(moduleType),
                _configuration,
                _environment,
                registeredModuleTypes,
                _dependencyRegistry,
                _metadataRegistry);

            await _attributeEventInvoker.InvokeRegistrationReceiversAsync(receivers, context).ConfigureAwait(false);
        }
    }

    private IModuleRegistrationEventReceiver[] GetRegistrationReceivers(Type moduleType)
    {
        var receivers = _planningSafeOnly
            ? _attributeEventService.GetPlanningRegistrationReceivers(moduleType)
            : _attributeEventService.GetRegistrationReceivers(moduleType);
        return [.. receivers];
    }
}
