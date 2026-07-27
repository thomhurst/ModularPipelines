using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
    private Task? _invocationTask;

    public RegistrationEventExecutor(
        IModuleAttributeEventService attributeEventService,
        IAttributeEventInvoker attributeEventInvoker,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        _attributeEventService = attributeEventService;
        _attributeEventInvoker = attributeEventInvoker;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
        _configuration = configuration;
        _environment = environment;
    }

    public Task InvokeRegistrationEventsAsync(IEnumerable<IModule> modules)
    {
        lock (_lock)
        {
            // Discovery invokes registration events early so dynamic dependencies can affect filtering.
            // Executors invoke this service again, so share the first invocation rather than running receivers twice.
            return _invocationTask ??= InvokeRegistrationEventsInternalAsync(modules.ToArray());
        }
    }

    private async Task InvokeRegistrationEventsInternalAsync(IReadOnlyList<IModule> modules)
    {
        var registeredModuleTypes = modules.Select(module => module.GetType()).ToArray();

        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var receivers = _attributeEventService.GetRegistrationReceivers(moduleType);

            if (receivers.Count == 0)
            {
                continue;
            }

            var context = new ModuleRegistrationContext(
                moduleType,
                moduleType.GetCustomAttributes(inherit: true).OfType<Attribute>().ToArray(),
                _configuration,
                _environment,
                registeredModuleTypes,
                _dependencyRegistry,
                _metadataRegistry);

            await _attributeEventInvoker.InvokeRegistrationReceiversAsync(receivers, context).ConfigureAwait(false);
        }
    }
}
