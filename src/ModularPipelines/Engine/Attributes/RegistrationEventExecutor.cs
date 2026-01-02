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
    private readonly IModuleAttributeEventService _attributeEventService;
    private readonly IAttributeEventInvoker _attributeEventInvoker;
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

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

    public async Task InvokeRegistrationEventsAsync(IEnumerable<IModule> modules)
    {
        var moduleArray = modules.ToArray();
        var registeredModuleTypes = moduleArray.Select(m => m.GetType()).ToArray();

        foreach (var module in moduleArray)
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
