# Enhanced Attribute System Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Implement a composable attribute system where attributes implement lifecycle interfaces to receive events and dynamically configure modules.

**Architecture:** Attributes implement event receiver interfaces (e.g., `IModuleFailureEventReceiver`). At registration time, `IModuleRegistrationEventReceiver` attributes can dynamically add dependencies. At runtime, other receivers fire at appropriate lifecycle points.

**Tech Stack:** C# 12, .NET 9/10, TUnit for testing, Microsoft.Extensions.DependencyInjection

---

## Task 1: Create Event Receiver Interfaces

**Files:**
- Create: `src/ModularPipelines/Attributes/Events/IModuleRegistrationEventReceiver.cs`
- Create: `src/ModularPipelines/Attributes/Events/IModuleStartEventReceiver.cs`
- Create: `src/ModularPipelines/Attributes/Events/IModuleEndEventReceiver.cs`
- Create: `src/ModularPipelines/Attributes/Events/IModuleFailureEventReceiver.cs`
- Create: `src/ModularPipelines/Attributes/Events/IModuleSkippedEventReceiver.cs`

**Step 1: Create the Events directory and IModuleRegistrationEventReceiver**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleRegistrationEventReceiver.cs
namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive registration events.
/// Invoked during module registration in the DI container.
/// Use for dynamic dependency configuration and service registration.
/// </summary>
public interface IModuleRegistrationEventReceiver
{
    /// <summary>
    /// Called when the module is being registered.
    /// </summary>
    /// <param name="context">The registration context providing access to dependencies and services.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnRegistrationAsync(IModuleRegistrationContext context);
}
```

**Step 2: Create IModuleStartEventReceiver**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleStartEventReceiver.cs
namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module start events.
/// Invoked immediately before a module starts executing.
/// </summary>
public interface IModuleStartEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module is about to start executing.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleStartAsync(IModuleEventContext context);
}
```

**Step 3: Create IModuleEndEventReceiver**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleEndEventReceiver.cs
using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module end events.
/// Invoked after a module completes (success or failure).
/// </summary>
public interface IModuleEndEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has finished executing.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="result">The result of the module execution.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result);
}
```

**Step 4: Create IModuleFailureEventReceiver**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleFailureEventReceiver.cs
namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module failure events.
/// Invoked when a module fails with an exception.
/// Called before OnModuleEndAsync.
/// </summary>
public interface IModuleFailureEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has failed with an exception.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleFailureAsync(IModuleEventContext context, Exception exception);
}
```

**Step 5: Create IModuleSkippedEventReceiver**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleSkippedEventReceiver.cs
using ModularPipelines.Models;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Implement this interface on an attribute to receive module skipped events.
/// Invoked when a module is skipped.
/// </summary>
public interface IModuleSkippedEventReceiver
{
    /// <summary>
    /// Gets whether to continue execution if this receiver throws an exception.
    /// Default is false (propagate exceptions).
    /// </summary>
    bool ContinueOnError => false;

    /// <summary>
    /// Called when the module has been skipped.
    /// </summary>
    /// <param name="context">The event context providing module information and control flow.</param>
    /// <param name="reason">The reason the module was skipped.</param>
    /// <returns>A task representing the async operation.</returns>
    Task OnModuleSkippedAsync(IModuleEventContext context, SkipDecision reason);
}
```

**Step 6: Build to verify no compilation errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 7: Commit**

```bash
git add src/ModularPipelines/Attributes/Events/
git commit -m "feat(attributes): add event receiver interfaces for composable attributes"
```

---

## Task 2: Create IModuleRegistrationContext Interface

**Files:**
- Create: `src/ModularPipelines/Attributes/Events/IModuleRegistrationContext.cs`

**Step 1: Create the interface**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleRegistrationContext.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Context provided to <see cref="IModuleRegistrationEventReceiver.OnRegistrationAsync"/>.
/// Provides access to pipeline configuration and enables dynamic dependency manipulation.
/// </summary>
public interface IModuleRegistrationContext
{
    /// <summary>
    /// Gets the type of the module being registered.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the attributes declared on the module.
    /// </summary>
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    /// <summary>
    /// Gets the application configuration.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Gets the host environment information.
    /// </summary>
    IHostEnvironment Environment { get; }

    /// <summary>
    /// Gets the types of all registered modules.
    /// </summary>
    IReadOnlyList<Type> RegisteredModuleTypes { get; }

    /// <summary>
    /// Checks if a specific module type is registered.
    /// </summary>
    bool IsModuleRegistered<TModule>() where TModule : IModule;

    /// <summary>
    /// Checks if a specific module type is registered.
    /// </summary>
    bool IsModuleRegistered(Type moduleType);

    /// <summary>
    /// Gets all registered module types that are assignable to the specified base type.
    /// </summary>
    IEnumerable<Type> GetModulesAssignableTo<TBase>() where TBase : IModule;

    /// <summary>
    /// Gets all registered module types that have the specified attribute.
    /// </summary>
    IEnumerable<Type> GetModulesWithAttribute<TAttribute>() where TAttribute : Attribute;

    /// <summary>
    /// Adds a dependency on another module.
    /// </summary>
    void AddDependency<TModule>() where TModule : IModule;

    /// <summary>
    /// Adds a dependency on another module.
    /// </summary>
    void AddDependency(Type moduleType);

    /// <summary>
    /// Adds dependencies on all modules assignable to the specified base type.
    /// </summary>
    void AddDependencyOnAll<TBase>() where TBase : IModule;

    /// <summary>
    /// Adds dependencies on all modules matching the specified predicate.
    /// </summary>
    void AddDependencyOnAll(Func<Type, bool> predicate);

    /// <summary>
    /// Removes a dependency on another module.
    /// </summary>
    void RemoveDependency<TModule>() where TModule : IModule;

    /// <summary>
    /// Gets the service collection for registering additional services.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Sets metadata that can be retrieved during module execution.
    /// </summary>
    void SetMetadata(string key, object value);

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);
}
```

**Step 2: Build to verify no compilation errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Attributes/Events/IModuleRegistrationContext.cs
git commit -m "feat(attributes): add IModuleRegistrationContext interface"
```

---

## Task 3: Create IModuleEventContext Interface

**Files:**
- Create: `src/ModularPipelines/Attributes/Events/IModuleEventContext.cs`

**Step 1: Create the interface**

```csharp
// src/ModularPipelines/Attributes/Events/IModuleEventContext.cs
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes.Events;

/// <summary>
/// Context provided to runtime event receivers (start, end, failure, skipped).
/// Provides access to module information, services, and control flow.
/// </summary>
public interface IModuleEventContext
{
    /// <summary>
    /// Gets the type of the module.
    /// </summary>
    Type ModuleType { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Gets the module instance.
    /// </summary>
    IModule ModuleInstance { get; }

    /// <summary>
    /// Gets the attributes declared on the module.
    /// </summary>
    IReadOnlyList<Attribute> ModuleAttributes { get; }

    /// <summary>
    /// Gets the time when the module started executing.
    /// </summary>
    DateTimeOffset StartTime { get; }

    /// <summary>
    /// Gets the elapsed time since the module started.
    /// </summary>
    TimeSpan ElapsedTime { get; }

    /// <summary>
    /// Gets the current status of the module.
    /// </summary>
    Status Status { get; }

    /// <summary>
    /// Gets the pipeline context.
    /// </summary>
    IPipelineContext PipelineContext { get; }

    /// <summary>
    /// Gets the cancellation token.
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    /// Resolves a service from the DI container.
    /// </summary>
    T GetService<T>() where T : notnull;

    /// <summary>
    /// Resolves a service from the DI container, or null if not registered.
    /// </summary>
    T? GetServiceOrDefault<T>() where T : class;

    /// <summary>
    /// Gets metadata that was set during registration.
    /// </summary>
    T? GetMetadata<T>(string key);

    /// <summary>
    /// Requests that the module be retried.
    /// Only works if the module supports retry.
    /// </summary>
    void RequestRetry(TimeSpan? delay = null);

    /// <summary>
    /// Marks dependent modules to be skipped.
    /// </summary>
    void SkipDependentModules(string reason);

    /// <summary>
    /// Requests that the pipeline fail after current modules complete.
    /// </summary>
    void FailPipeline(string reason);
}
```

**Step 2: Build to verify no compilation errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Attributes/Events/IModuleEventContext.cs
git commit -m "feat(attributes): add IModuleEventContext interface"
```

---

## Task 4: Create ModuleDependencyRegistry

**Files:**
- Create: `src/ModularPipelines/Engine/Dependencies/ModuleDependencyRegistry.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/ModuleDependencyRegistryTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/ModuleDependencyRegistryTests.cs
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleDependencyRegistryTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    private class ModuleB : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("B");
    }

    private class ModuleC : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("C");
    }

    [Test]
    public async Task AddDynamicDependency_AddsDependency()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).Contains(typeof(ModuleB));
    }

    [Test]
    public async Task AddDynamicDependency_MultipleDependencies_AddsAll()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));
        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleC));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies.Count()).IsEqualTo(2);
        await Assert.That(dependencies).Contains(typeof(ModuleB));
        await Assert.That(dependencies).Contains(typeof(ModuleC));
    }

    [Test]
    public async Task RemoveDependency_RemovesDependency()
    {
        var registry = new ModuleDependencyRegistry();

        registry.AddDynamicDependency(typeof(ModuleA), typeof(ModuleB));
        registry.RemoveDependency(typeof(ModuleA), typeof(ModuleB));

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).IsEmpty();
    }

    [Test]
    public async Task GetDynamicDependencies_NoDependencies_ReturnsEmpty()
    {
        var registry = new ModuleDependencyRegistry();

        var dependencies = registry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).IsEmpty();
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleDependencyRegistryTests"`
Expected: FAIL with "type or namespace 'ModuleDependencyRegistry' could not be found"

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Engine/Dependencies/ModuleDependencyRegistry.cs
using System.Collections.Concurrent;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores dynamic dependencies added via attribute event receivers.
/// </summary>
internal class ModuleDependencyRegistry : IModuleDependencyRegistry
{
    private readonly ConcurrentDictionary<Type, HashSet<Type>> _dynamicDependencies = new();
    private readonly object _lock = new();

    /// <summary>
    /// Adds a dynamic dependency from one module to another.
    /// </summary>
    public void AddDynamicDependency(Type module, Type dependency)
    {
        lock (_lock)
        {
            var deps = _dynamicDependencies.GetOrAdd(module, _ => new HashSet<Type>());
            deps.Add(dependency);
        }
    }

    /// <summary>
    /// Removes a dependency from a module.
    /// </summary>
    public void RemoveDependency(Type module, Type dependency)
    {
        lock (_lock)
        {
            if (_dynamicDependencies.TryGetValue(module, out var deps))
            {
                deps.Remove(dependency);
            }
        }
    }

    /// <summary>
    /// Gets all dynamic dependencies for a module.
    /// </summary>
    public IEnumerable<Type> GetDynamicDependencies(Type module)
    {
        if (_dynamicDependencies.TryGetValue(module, out var deps))
        {
            lock (_lock)
            {
                return deps.ToList();
            }
        }

        return Enumerable.Empty<Type>();
    }

    /// <summary>
    /// Checks if a module has any dynamic dependencies.
    /// </summary>
    public bool HasDynamicDependencies(Type module)
    {
        return _dynamicDependencies.TryGetValue(module, out var deps) && deps.Count > 0;
    }
}
```

**Step 4: Create the interface**

```csharp
// src/ModularPipelines/Engine/Dependencies/IModuleDependencyRegistry.cs
namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for dynamic module dependencies added at registration time.
/// </summary>
internal interface IModuleDependencyRegistry
{
    void AddDynamicDependency(Type module, Type dependency);
    void RemoveDependency(Type module, Type dependency);
    IEnumerable<Type> GetDynamicDependencies(Type module);
    bool HasDynamicDependencies(Type module);
}
```

**Step 5: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleDependencyRegistryTests"`
Expected: PASS

**Step 6: Commit**

```bash
git add src/ModularPipelines/Engine/Dependencies/ test/ModularPipelines.UnitTests/Attributes/ModuleDependencyRegistryTests.cs
git commit -m "feat(attributes): add ModuleDependencyRegistry for dynamic dependencies"
```

---

## Task 5: Create ModuleMetadataRegistry

**Files:**
- Create: `src/ModularPipelines/Engine/Dependencies/ModuleMetadataRegistry.cs`
- Create: `src/ModularPipelines/Engine/Dependencies/IModuleMetadataRegistry.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/ModuleMetadataRegistryTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/ModuleMetadataRegistryTests.cs
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleMetadataRegistryTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    [Test]
    public async Task SetMetadata_GetMetadata_ReturnsValue()
    {
        var registry = new ModuleMetadataRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value");
    }

    [Test]
    public async Task GetMetadata_NotSet_ReturnsNull()
    {
        var registry = new ModuleMetadataRegistry();

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task SetMetadata_OverwritesExisting()
    {
        var registry = new ModuleMetadataRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value1");
        registry.SetMetadata(typeof(ModuleA), "key", "value2");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value2");
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleMetadataRegistryTests"`
Expected: FAIL

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Engine/Dependencies/IModuleMetadataRegistry.cs
namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Registry for module metadata set during registration.
/// </summary>
internal interface IModuleMetadataRegistry
{
    void SetMetadata(Type moduleType, string key, object value);
    T? GetMetadata<T>(Type moduleType, string key);
}
```

```csharp
// src/ModularPipelines/Engine/Dependencies/ModuleMetadataRegistry.cs
using System.Collections.Concurrent;

namespace ModularPipelines.Engine.Dependencies;

/// <summary>
/// Stores metadata for modules set during registration.
/// </summary>
internal class ModuleMetadataRegistry : IModuleMetadataRegistry
{
    private readonly ConcurrentDictionary<(Type, string), object> _metadata = new();

    public void SetMetadata(Type moduleType, string key, object value)
    {
        _metadata[(moduleType, key)] = value;
    }

    public T? GetMetadata<T>(Type moduleType, string key)
    {
        if (_metadata.TryGetValue((moduleType, key), out var value) && value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }
}
```

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleMetadataRegistryTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Engine/Dependencies/ModuleMetadataRegistry.cs src/ModularPipelines/Engine/Dependencies/IModuleMetadataRegistry.cs test/ModularPipelines.UnitTests/Attributes/ModuleMetadataRegistryTests.cs
git commit -m "feat(attributes): add ModuleMetadataRegistry for cross-phase data"
```

---

## Task 6: Implement ModuleRegistrationContext

**Files:**
- Create: `src/ModularPipelines/Context/ModuleRegistrationContext.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/ModuleRegistrationContextTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/ModuleRegistrationContextTests.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
using NSubstitute;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleRegistrationContextTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    private class ModuleB : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("B");
    }

    [Test]
    public async Task ModuleType_ReturnsCorrectType()
    {
        var context = CreateContext(typeof(ModuleA));

        await Assert.That(context.ModuleType).IsEqualTo(typeof(ModuleA));
    }

    [Test]
    public async Task AddDependency_AddsToDependencyRegistry()
    {
        var dependencyRegistry = new ModuleDependencyRegistry();
        var context = CreateContext(typeof(ModuleA), dependencyRegistry: dependencyRegistry);

        context.AddDependency<ModuleB>();

        var dependencies = dependencyRegistry.GetDynamicDependencies(typeof(ModuleA));
        await Assert.That(dependencies).Contains(typeof(ModuleB));
    }

    [Test]
    public async Task IsModuleRegistered_RegisteredModule_ReturnsTrue()
    {
        var registeredModules = new List<Type> { typeof(ModuleA), typeof(ModuleB) };
        var context = CreateContext(typeof(ModuleA), registeredModules: registeredModules);

        await Assert.That(context.IsModuleRegistered<ModuleB>()).IsTrue();
    }

    [Test]
    public async Task IsModuleRegistered_UnregisteredModule_ReturnsFalse()
    {
        var registeredModules = new List<Type> { typeof(ModuleA) };
        var context = CreateContext(typeof(ModuleA), registeredModules: registeredModules);

        await Assert.That(context.IsModuleRegistered<ModuleB>()).IsFalse();
    }

    [Test]
    public async Task SetMetadata_GetMetadata_RoundTrips()
    {
        var metadataRegistry = new ModuleMetadataRegistry();
        var context = CreateContext(typeof(ModuleA), metadataRegistry: metadataRegistry);

        context.SetMetadata("key", "value");

        await Assert.That(context.GetMetadata<string>("key")).IsEqualTo("value");
    }

    private static ModuleRegistrationContext CreateContext(
        Type moduleType,
        IModuleDependencyRegistry? dependencyRegistry = null,
        IModuleMetadataRegistry? metadataRegistry = null,
        IReadOnlyList<Type>? registeredModules = null)
    {
        var configuration = Substitute.For<IConfiguration>();
        var environment = Substitute.For<IHostEnvironment>();
        var services = new ServiceCollection();

        return new ModuleRegistrationContext(
            moduleType,
            moduleType.GetCustomAttributes(true).OfType<Attribute>().ToList(),
            configuration,
            environment,
            registeredModules ?? new List<Type> { moduleType },
            services,
            dependencyRegistry ?? new ModuleDependencyRegistry(),
            metadataRegistry ?? new ModuleMetadataRegistry());
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleRegistrationContextTests"`
Expected: FAIL

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Context/ModuleRegistrationContext.cs
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleRegistrationContext"/>.
/// </summary>
internal class ModuleRegistrationContext : IModuleRegistrationContext
{
    private readonly IModuleDependencyRegistry _dependencyRegistry;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IReadOnlyList<Type> _registeredModuleTypes;

    public ModuleRegistrationContext(
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        IConfiguration configuration,
        IHostEnvironment environment,
        IReadOnlyList<Type> registeredModuleTypes,
        IServiceCollection services,
        IModuleDependencyRegistry dependencyRegistry,
        IModuleMetadataRegistry metadataRegistry)
    {
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        Configuration = configuration;
        Environment = environment;
        _registeredModuleTypes = registeredModuleTypes;
        Services = services;
        _dependencyRegistry = dependencyRegistry;
        _metadataRegistry = metadataRegistry;
    }

    public Type ModuleType { get; }

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public IConfiguration Configuration { get; }

    public IHostEnvironment Environment { get; }

    public IReadOnlyList<Type> RegisteredModuleTypes => _registeredModuleTypes;

    public IServiceCollection Services { get; }

    public bool IsModuleRegistered<TModule>() where TModule : IModule
        => IsModuleRegistered(typeof(TModule));

    public bool IsModuleRegistered(Type moduleType)
        => _registeredModuleTypes.Contains(moduleType);

    public IEnumerable<Type> GetModulesAssignableTo<TBase>() where TBase : IModule
        => _registeredModuleTypes.Where(t => typeof(TBase).IsAssignableFrom(t) && t != ModuleType);

    public IEnumerable<Type> GetModulesWithAttribute<TAttribute>() where TAttribute : Attribute
        => _registeredModuleTypes.Where(t => t.GetCustomAttribute<TAttribute>() != null);

    public void AddDependency<TModule>() where TModule : IModule
        => AddDependency(typeof(TModule));

    public void AddDependency(Type moduleType)
        => _dependencyRegistry.AddDynamicDependency(ModuleType, moduleType);

    public void AddDependencyOnAll<TBase>() where TBase : IModule
    {
        foreach (var moduleType in GetModulesAssignableTo<TBase>())
        {
            AddDependency(moduleType);
        }
    }

    public void AddDependencyOnAll(Func<Type, bool> predicate)
    {
        foreach (var moduleType in _registeredModuleTypes.Where(t => t != ModuleType && predicate(t)))
        {
            AddDependency(moduleType);
        }
    }

    public void RemoveDependency<TModule>() where TModule : IModule
        => _dependencyRegistry.RemoveDependency(ModuleType, typeof(TModule));

    public void SetMetadata(string key, object value)
        => _metadataRegistry.SetMetadata(ModuleType, key, value);

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);
}
```

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleRegistrationContextTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Context/ModuleRegistrationContext.cs test/ModularPipelines.UnitTests/Attributes/ModuleRegistrationContextTests.cs
git commit -m "feat(attributes): implement ModuleRegistrationContext"
```

---

## Task 7: Implement ModuleEventContext

**Files:**
- Create: `src/ModularPipelines/Context/ModuleEventContext.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/ModuleEventContextTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/ModuleEventContextTests.cs
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using NSubstitute;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleEventContextTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    [Test]
    public async Task ModuleType_ReturnsCorrectType()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        await Assert.That(context.ModuleType).IsEqualTo(typeof(ModuleA));
    }

    [Test]
    public async Task ModuleName_ReturnsTypeName()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        await Assert.That(context.ModuleName).IsEqualTo("ModuleA");
    }

    [Test]
    public async Task GetService_ResolvesFromServiceProvider()
    {
        var module = new ModuleA();
        var services = new ServiceCollection();
        services.AddSingleton<string>("test-value");
        var serviceProvider = services.BuildServiceProvider();
        var context = CreateContext(module, serviceProvider: serviceProvider);

        var result = context.GetService<string>();
        await Assert.That(result).IsEqualTo("test-value");
    }

    [Test]
    public async Task GetMetadata_ReturnsValueFromRegistry()
    {
        var module = new ModuleA();
        var metadataRegistry = new ModuleMetadataRegistry();
        metadataRegistry.SetMetadata(typeof(ModuleA), "key", "value");
        var context = CreateContext(module, metadataRegistry: metadataRegistry);

        var result = context.GetMetadata<string>("key");
        await Assert.That(result).IsEqualTo("value");
    }

    [Test]
    public async Task RequestRetry_SetsRetryRequested()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.RequestRetry(TimeSpan.FromSeconds(5));

        await Assert.That(context.RetryRequested).IsTrue();
        await Assert.That(context.RetryDelay).IsEqualTo(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task SkipDependentModules_SetsFlag()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.SkipDependentModules("test reason");

        await Assert.That(context.ShouldSkipDependents).IsTrue();
        await Assert.That(context.SkipDependentsReason).IsEqualTo("test reason");
    }

    [Test]
    public async Task FailPipeline_SetsFlag()
    {
        var module = new ModuleA();
        var context = CreateContext(module);

        context.FailPipeline("test reason");

        await Assert.That(context.ShouldFailPipeline).IsTrue();
        await Assert.That(context.FailPipelineReason).IsEqualTo("test reason");
    }

    private static ModuleEventContext CreateContext(
        IModule module,
        IServiceProvider? serviceProvider = null,
        IModuleMetadataRegistry? metadataRegistry = null)
    {
        var pipelineContext = Substitute.For<IPipelineContext>();
        serviceProvider ??= new ServiceCollection().BuildServiceProvider();

        return new ModuleEventContext(
            module,
            module.GetType(),
            module.GetType().GetCustomAttributes(true).OfType<Attribute>().ToList(),
            DateTimeOffset.UtcNow,
            Status.Processing,
            pipelineContext,
            serviceProvider,
            metadataRegistry ?? new ModuleMetadataRegistry(),
            CancellationToken.None);
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleEventContextTests"`
Expected: FAIL

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Context/ModuleEventContext.cs
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleEventContext"/>.
/// </summary>
internal class ModuleEventContext : IModuleEventContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly DateTimeOffset _startTime;

    public ModuleEventContext(
        IModule moduleInstance,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        Status status,
        IPipelineContext pipelineContext,
        IServiceProvider serviceProvider,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken)
    {
        ModuleInstance = moduleInstance;
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        _startTime = startTime;
        Status = status;
        PipelineContext = pipelineContext;
        _serviceProvider = serviceProvider;
        _metadataRegistry = metadataRegistry;
        CancellationToken = cancellationToken;
    }

    public Type ModuleType { get; }

    public string ModuleName => ModuleType.Name;

    public IModule ModuleInstance { get; }

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public DateTimeOffset StartTime => _startTime;

    public TimeSpan ElapsedTime => DateTimeOffset.UtcNow - _startTime;

    public Status Status { get; }

    public IPipelineContext PipelineContext { get; }

    public CancellationToken CancellationToken { get; }

    // Control flow state
    public bool RetryRequested { get; private set; }
    public TimeSpan? RetryDelay { get; private set; }
    public bool ShouldSkipDependents { get; private set; }
    public string? SkipDependentsReason { get; private set; }
    public bool ShouldFailPipeline { get; private set; }
    public string? FailPipelineReason { get; private set; }

    public T GetService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();

    public T? GetServiceOrDefault<T>() where T : class
        => _serviceProvider.GetService<T>();

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);

    public void RequestRetry(TimeSpan? delay = null)
    {
        RetryRequested = true;
        RetryDelay = delay;
    }

    public void SkipDependentModules(string reason)
    {
        ShouldSkipDependents = true;
        SkipDependentsReason = reason;
    }

    public void FailPipeline(string reason)
    {
        ShouldFailPipeline = true;
        FailPipelineReason = reason;
    }
}
```

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleEventContextTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Context/ModuleEventContext.cs test/ModularPipelines.UnitTests/Attributes/ModuleEventContextTests.cs
git commit -m "feat(attributes): implement ModuleEventContext"
```

---

## Task 8: Create AttributeEventInvoker

**Files:**
- Create: `src/ModularPipelines/Engine/Attributes/IAttributeEventInvoker.cs`
- Create: `src/ModularPipelines/Engine/Attributes/AttributeEventInvoker.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/AttributeEventInvokerTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/AttributeEventInvokerTests.cs
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Attributes;
using NSubstitute;

namespace ModularPipelines.UnitTests.Attributes;

public class AttributeEventInvokerTests
{
    private class SuccessfulReceiver : IModuleStartEventReceiver
    {
        public bool WasCalled { get; private set; }
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    private class FailingReceiver : IModuleStartEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    private class FailingReceiverWithContinue : IModuleStartEventReceiver
    {
        public bool ContinueOnError => true;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    [Test]
    public async Task InvokeAsync_CallsAllReceivers()
    {
        var receiver1 = new SuccessfulReceiver();
        var receiver2 = new SuccessfulReceiver();
        var receivers = new List<IModuleStartEventReceiver> { receiver1, receiver2 };
        var invoker = new AttributeEventInvoker(Substitute.For<ILogger<AttributeEventInvoker>>());
        var context = Substitute.For<IModuleEventContext>();

        await invoker.InvokeStartReceiversAsync(receivers, context);

        await Assert.That(receiver1.WasCalled).IsTrue();
        await Assert.That(receiver2.WasCalled).IsTrue();
    }

    [Test]
    public async Task InvokeAsync_ReceiverThrows_ContinueOnErrorFalse_Propagates()
    {
        var receiver = new FailingReceiver();
        var receivers = new List<IModuleStartEventReceiver> { receiver };
        var invoker = new AttributeEventInvoker(Substitute.For<ILogger<AttributeEventInvoker>>());
        var context = Substitute.For<IModuleEventContext>();

        await Assert.That(async () => await invoker.InvokeStartReceiversAsync(receivers, context))
            .ThrowsException()
            .WithMessage("Test exception");
    }

    [Test]
    public async Task InvokeAsync_ReceiverThrows_ContinueOnErrorTrue_Continues()
    {
        var failingReceiver = new FailingReceiverWithContinue();
        var successReceiver = new SuccessfulReceiver();
        var receivers = new List<IModuleStartEventReceiver> { failingReceiver, successReceiver };
        var invoker = new AttributeEventInvoker(Substitute.For<ILogger<AttributeEventInvoker>>());
        var context = Substitute.For<IModuleEventContext>();

        await invoker.InvokeStartReceiversAsync(receivers, context);

        await Assert.That(successReceiver.WasCalled).IsTrue();
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "AttributeEventInvokerTests"`
Expected: FAIL

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Engine/Attributes/IAttributeEventInvoker.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event receivers with error handling.
/// </summary>
internal interface IAttributeEventInvoker
{
    Task InvokeRegistrationReceiversAsync(IEnumerable<IModuleRegistrationEventReceiver> receivers, IModuleRegistrationContext context);
    Task InvokeStartReceiversAsync(IEnumerable<IModuleStartEventReceiver> receivers, IModuleEventContext context);
    Task InvokeEndReceiversAsync(IEnumerable<IModuleEndEventReceiver> receivers, IModuleEventContext context, ModuleResult result);
    Task InvokeFailureReceiversAsync(IEnumerable<IModuleFailureEventReceiver> receivers, IModuleEventContext context, Exception exception);
    Task InvokeSkippedReceiversAsync(IEnumerable<IModuleSkippedEventReceiver> receivers, IModuleEventContext context, SkipDecision reason);
}
```

```csharp
// src/ModularPipelines/Engine/Attributes/AttributeEventInvoker.cs
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event receivers with configurable error handling.
/// </summary>
internal class AttributeEventInvoker : IAttributeEventInvoker
{
    private readonly ILogger<AttributeEventInvoker> _logger;

    public AttributeEventInvoker(ILogger<AttributeEventInvoker> logger)
    {
        _logger = logger;
    }

    public async Task InvokeRegistrationReceiversAsync(
        IEnumerable<IModuleRegistrationEventReceiver> receivers,
        IModuleRegistrationContext context)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnRegistrationAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Registration receiver {Type} failed", receiver.GetType().Name);
                throw;
            }
        }
    }

    public async Task InvokeStartReceiversAsync(
        IEnumerable<IModuleStartEventReceiver> receivers,
        IModuleEventContext context)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleStartAsync(context);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Start receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeEndReceiversAsync(
        IEnumerable<IModuleEndEventReceiver> receivers,
        IModuleEventContext context,
        ModuleResult result)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleEndAsync(context, result);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "End receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeFailureReceiversAsync(
        IEnumerable<IModuleFailureEventReceiver> receivers,
        IModuleEventContext context,
        Exception exception)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleFailureAsync(context, exception);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Failure receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    public async Task InvokeSkippedReceiversAsync(
        IEnumerable<IModuleSkippedEventReceiver> receivers,
        IModuleEventContext context,
        SkipDecision reason)
    {
        foreach (var receiver in receivers)
        {
            try
            {
                await receiver.OnModuleSkippedAsync(context, reason);
            }
            catch (Exception ex)
            {
                if (receiver.ContinueOnError)
                {
                    _logger.LogWarning(ex, "Skipped receiver {Type} failed, continuing", receiver.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
```

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "AttributeEventInvokerTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Engine/Attributes/ test/ModularPipelines.UnitTests/Attributes/AttributeEventInvokerTests.cs
git commit -m "feat(attributes): add AttributeEventInvoker for receiver invocation"
```

---

## Task 9: Create ModuleAttributeEventService

**Files:**
- Create: `src/ModularPipelines/Engine/Attributes/IModuleAttributeEventService.cs`
- Create: `src/ModularPipelines/Engine/Attributes/ModuleAttributeEventService.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/ModuleAttributeEventServiceTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/ModuleAttributeEventServiceTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleAttributeEventServiceTests
{
    public class TestStartAttribute : Attribute, IModuleStartEventReceiver
    {
        public bool ContinueOnError => false;
        public Task OnModuleStartAsync(IModuleEventContext context) => Task.CompletedTask;
    }

    public class TestFailureAttribute : Attribute, IModuleFailureEventReceiver
    {
        public bool ContinueOnError => false;
        public Task OnModuleFailureAsync(IModuleEventContext context, Exception exception) => Task.CompletedTask;
    }

    [TestStart]
    [TestFailure]
    private class ModuleWithAttributes : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    private class ModuleWithoutAttributes : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    [Test]
    public async Task GetStartReceivers_ModuleWithAttribute_ReturnsReceiver()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithAttributes));

        await Assert.That(receivers.Count).IsEqualTo(1);
        await Assert.That(receivers[0]).IsTypeOf<TestStartAttribute>();
    }

    [Test]
    public async Task GetFailureReceivers_ModuleWithAttribute_ReturnsReceiver()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetFailureReceivers(typeof(ModuleWithAttributes));

        await Assert.That(receivers.Count).IsEqualTo(1);
        await Assert.That(receivers[0]).IsTypeOf<TestFailureAttribute>();
    }

    [Test]
    public async Task GetStartReceivers_ModuleWithoutAttributes_ReturnsEmpty()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithoutAttributes));

        await Assert.That(receivers).IsEmpty();
    }

    [Test]
    public async Task GetReceivers_CachesResults()
    {
        var service = new ModuleAttributeEventService();

        var receivers1 = service.GetStartReceivers(typeof(ModuleWithAttributes));
        var receivers2 = service.GetStartReceivers(typeof(ModuleWithAttributes));

        await Assert.That(ReferenceEquals(receivers1, receivers2)).IsTrue();
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleAttributeEventServiceTests"`
Expected: FAIL

**Step 3: Write minimal implementation**

```csharp
// src/ModularPipelines/Engine/Attributes/IModuleAttributeEventService.cs
using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Service for discovering and caching attribute event receivers on modules.
/// </summary>
internal interface IModuleAttributeEventService
{
    IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType);
    IReadOnlyList<IModuleStartEventReceiver> GetStartReceivers(Type moduleType);
    IReadOnlyList<IModuleEndEventReceiver> GetEndReceivers(Type moduleType);
    IReadOnlyList<IModuleFailureEventReceiver> GetFailureReceivers(Type moduleType);
    IReadOnlyList<IModuleSkippedEventReceiver> GetSkippedReceivers(Type moduleType);
}
```

```csharp
// src/ModularPipelines/Engine/Attributes/ModuleAttributeEventService.cs
using System.Collections.Concurrent;
using System.Reflection;
using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Discovers and caches attribute event receivers on modules.
/// Receivers are returned in declaration order.
/// </summary>
internal class ModuleAttributeEventService : IModuleAttributeEventService
{
    private readonly ConcurrentDictionary<Type, AttributeReceiverCache> _cache = new();

    public IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType)
        => GetCache(moduleType).RegistrationReceivers;

    public IReadOnlyList<IModuleStartEventReceiver> GetStartReceivers(Type moduleType)
        => GetCache(moduleType).StartReceivers;

    public IReadOnlyList<IModuleEndEventReceiver> GetEndReceivers(Type moduleType)
        => GetCache(moduleType).EndReceivers;

    public IReadOnlyList<IModuleFailureEventReceiver> GetFailureReceivers(Type moduleType)
        => GetCache(moduleType).FailureReceivers;

    public IReadOnlyList<IModuleSkippedEventReceiver> GetSkippedReceivers(Type moduleType)
        => GetCache(moduleType).SkippedReceivers;

    private AttributeReceiverCache GetCache(Type moduleType)
        => _cache.GetOrAdd(moduleType, DiscoverReceivers);

    private static AttributeReceiverCache DiscoverReceivers(Type moduleType)
    {
        // Get attributes in declaration order
        var attributes = moduleType.GetCustomAttributes(inherit: true);

        var registrationReceivers = new List<IModuleRegistrationEventReceiver>();
        var startReceivers = new List<IModuleStartEventReceiver>();
        var endReceivers = new List<IModuleEndEventReceiver>();
        var failureReceivers = new List<IModuleFailureEventReceiver>();
        var skippedReceivers = new List<IModuleSkippedEventReceiver>();

        foreach (var attribute in attributes)
        {
            if (attribute is IModuleRegistrationEventReceiver registration)
            {
                registrationReceivers.Add(registration);
            }

            if (attribute is IModuleStartEventReceiver start)
            {
                startReceivers.Add(start);
            }

            if (attribute is IModuleEndEventReceiver end)
            {
                endReceivers.Add(end);
            }

            if (attribute is IModuleFailureEventReceiver failure)
            {
                failureReceivers.Add(failure);
            }

            if (attribute is IModuleSkippedEventReceiver skipped)
            {
                skippedReceivers.Add(skipped);
            }
        }

        return new AttributeReceiverCache(
            registrationReceivers,
            startReceivers,
            endReceivers,
            failureReceivers,
            skippedReceivers);
    }

    private sealed record AttributeReceiverCache(
        IReadOnlyList<IModuleRegistrationEventReceiver> RegistrationReceivers,
        IReadOnlyList<IModuleStartEventReceiver> StartReceivers,
        IReadOnlyList<IModuleEndEventReceiver> EndReceivers,
        IReadOnlyList<IModuleFailureEventReceiver> FailureReceivers,
        IReadOnlyList<IModuleSkippedEventReceiver> SkippedReceivers);
}
```

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "ModuleAttributeEventServiceTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Engine/Attributes/ test/ModularPipelines.UnitTests/Attributes/ModuleAttributeEventServiceTests.cs
git commit -m "feat(attributes): add ModuleAttributeEventService for receiver discovery"
```

---

## Task 10: Register New Services in DI

**Files:**
- Modify: `src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs`

**Step 1: Build to ensure current state compiles**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 2: Add new service registrations**

Add these lines to the singletons section in `DependencyInjectionSetup.cs`:

```csharp
// Attribute event system
.AddSingleton<IModuleDependencyRegistry, ModuleDependencyRegistry>()
.AddSingleton<IModuleMetadataRegistry, ModuleMetadataRegistry>()
.AddSingleton<IModuleAttributeEventService, ModuleAttributeEventService>()
.AddSingleton<IAttributeEventInvoker, AttributeEventInvoker>()
```

Add the using statements at the top:

```csharp
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
```

**Step 3: Build to verify changes**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs
git commit -m "feat(attributes): register attribute event system services"
```

---

## Task 11: Integrate Dynamic Dependencies into ModuleDependencyResolver

**Files:**
- Modify: `src/ModularPipelines/Engine/ModuleDependencyResolver.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/DynamicDependencyIntegrationTests.cs`

**Step 1: Write the failing integration test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/DynamicDependencyIntegrationTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class DynamicDependencyIntegrationTests : TestBase
{
    public static readonly List<string> ExecutionOrder = new();

    public class AddDependencyAttribute : Attribute, IModuleRegistrationEventReceiver
    {
        private readonly Type _dependencyType;

        public AddDependencyAttribute(Type dependencyType)
        {
            _dependencyType = dependencyType;
        }

        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(_dependencyType);
            return Task.CompletedTask;
        }
    }

    private class ModuleA : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("A");
            await Task.Yield();
            return "A";
        }
    }

    [AddDependency(typeof(ModuleA))]
    private class ModuleB : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("B");
            await Task.Yield();
            return "B";
        }
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        ExecutionOrder.Clear();
    }

    [Test]
    public async Task DynamicDependency_ModuleBWaitsForModuleA()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(ExecutionOrder).IsEquivalentTo(new[] { "A", "B" });
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "DynamicDependencyIntegrationTests"`
Expected: FAIL (ModuleB may run before ModuleA because dynamic dependencies aren't wired up yet)

**Step 3: Modify ModuleDependencyResolver to include dynamic dependencies**

Update `ModuleDependencyResolver.cs` to accept `IModuleDependencyRegistry`:

```csharp
// Add method that includes dynamic dependencies
public static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetAllDependencies(
    Type moduleType,
    IEnumerable<Type> availableModuleTypes,
    IModuleDependencyRegistry? dynamicRegistry = null)
{
    // Static dependencies from attributes
    foreach (var dep in GetDependencies(moduleType, availableModuleTypes))
    {
        yield return dep;
    }

    // Dynamic dependencies from registration
    if (dynamicRegistry != null)
    {
        foreach (var dynamicDep in dynamicRegistry.GetDynamicDependencies(moduleType))
        {
            yield return (dynamicDep, false);
        }
    }
}
```

**Step 4: Wire up registration event invocation in ModuleRetriever or PipelineInitializer**

This requires modifying the pipeline initialization to invoke `IModuleRegistrationEventReceiver` on module attributes before dependency resolution.

Add to the initialization flow (in `PipelineInitializer.cs` or create new service):

```csharp
// During initialization, before modules are organized:
foreach (var module in modules)
{
    var moduleType = module.GetType();
    var receivers = _attributeEventService.GetRegistrationReceivers(moduleType);

    if (receivers.Count > 0)
    {
        var context = CreateRegistrationContext(moduleType);
        await _attributeEventInvoker.InvokeRegistrationReceiversAsync(receivers, context);
    }
}
```

**Step 5: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "DynamicDependencyIntegrationTests"`
Expected: PASS

**Step 6: Commit**

```bash
git add src/ModularPipelines/Engine/ test/ModularPipelines.UnitTests/Attributes/DynamicDependencyIntegrationTests.cs
git commit -m "feat(attributes): integrate dynamic dependencies into resolver"
```

---

## Task 12: Integrate Lifecycle Events into ModuleExecutor

**Files:**
- Modify: `src/ModularPipelines/Engine/ModuleExecutor.cs`
- Test: `test/ModularPipelines.UnitTests/Attributes/LifecycleEventIntegrationTests.cs`

**Step 1: Write the failing test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/LifecycleEventIntegrationTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class LifecycleEventIntegrationTests : TestBase
{
    public static readonly List<string> Events = new();

    public class TrackStartAttribute : Attribute, IModuleStartEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            Events.Add($"Start:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class TrackEndAttribute : Attribute, IModuleEndEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result)
        {
            Events.Add($"End:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    [TrackStart]
    [TrackEnd]
    private class TrackedModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Events.Add($"Execute:{GetType().Name}");
            await Task.Yield();
            return "done";
        }
    }

    [Before(Test)]
    public void ClearEvents()
    {
        Events.Clear();
    }

    [Test]
    public async Task LifecycleEvents_FireInCorrectOrder()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<TrackedModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(Events).IsEquivalentTo(new[]
        {
            "Start:TrackedModule",
            "Execute:TrackedModule",
            "End:TrackedModule"
        });
    }
}
```

**Step 2: Run test to verify it fails**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "LifecycleEventIntegrationTests"`
Expected: FAIL

**Step 3: Modify ModuleExecutor to invoke lifecycle events**

In `ExecuteModuleWithPipeline`, add:
- Before execution: invoke `IModuleStartEventReceiver`
- After execution (success): invoke `IModuleEndEventReceiver`
- On failure: invoke `IModuleFailureEventReceiver` then `IModuleEndEventReceiver`
- On skip: invoke `IModuleSkippedEventReceiver`

**Step 4: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "LifecycleEventIntegrationTests"`
Expected: PASS

**Step 5: Commit**

```bash
git add src/ModularPipelines/Engine/ModuleExecutor.cs test/ModularPipelines.UnitTests/Attributes/LifecycleEventIntegrationTests.cs
git commit -m "feat(attributes): integrate lifecycle events into ModuleExecutor"
```

---

## Task 13: Add Failure Event Integration Test

**Files:**
- Test: `test/ModularPipelines.UnitTests/Attributes/FailureEventIntegrationTests.cs`

**Step 1: Write the test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/FailureEventIntegrationTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class FailureEventIntegrationTests : TestBase
{
    public static Exception? CapturedExceptionByFailure;
    public static bool FailureReceiverCalled;

    public class CaptureFailureAttribute : Attribute, IModuleFailureEventReceiver
    {
        public bool ContinueOnError => true;

        public Task OnModuleFailureAsync(IModuleEventContext context, Exception exception)
        {
            FailureReceiverCalled = true;
            CapturedExceptionByFailure = exception;
            return Task.CompletedTask;
        }
    }

    [CaptureFailure]
    private class FailingModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Test failure");
        }
    }

    [Before(Test)]
    public void ResetState()
    {
        CapturedExceptionByFailure = null;
        FailureReceiverCalled = false;
    }

    [Test]
    public async Task FailureEvent_CapturesException()
    {
        await Assert.That(async () => await TestPipelineHostBuilder.Create()
            .AddModule<FailingModule>()
            .ExecutePipelineAsync())
            .ThrowsException();

        await Assert.That(FailureReceiverCalled).IsTrue();
        await Assert.That(CapturedExceptionByFailure).IsNotNull();
        await Assert.That(CapturedExceptionByFailure!.Message).IsEqualTo("Test failure");
    }
}
```

**Step 2: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "FailureEventIntegrationTests"`
Expected: PASS

**Step 3: Commit**

```bash
git add test/ModularPipelines.UnitTests/Attributes/FailureEventIntegrationTests.cs
git commit -m "test(attributes): add failure event integration test"
```

---

## Task 14: Add Skip Event Integration Test

**Files:**
- Test: `test/ModularPipelines.UnitTests/Attributes/SkipEventIntegrationTests.cs`

**Step 1: Write the test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/SkipEventIntegrationTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class SkipEventIntegrationTests : TestBase
{
    public static bool SkipReceiverCalled;
    public static SkipDecision? CapturedReason;

    public class CaptureSkipAttribute : Attribute, IModuleSkippedEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleSkippedAsync(IModuleEventContext context, SkipDecision reason)
        {
            SkipReceiverCalled = true;
            CapturedReason = reason;
            return Task.CompletedTask;
        }
    }

    [CaptureSkip]
    private class SkippableModule : Module<string>, ISkippable
    {
        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Test skip reason"));
        }

        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("should not run");
        }
    }

    [Before(Test)]
    public void ResetState()
    {
        SkipReceiverCalled = false;
        CapturedReason = null;
    }

    [Test]
    public async Task SkipEvent_CapturesReason()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SkippableModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(SkipReceiverCalled).IsTrue();
        await Assert.That(CapturedReason).IsNotNull();
        await Assert.That(CapturedReason!.Reason).IsEqualTo("Test skip reason");
    }
}
```

**Step 2: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "SkipEventIntegrationTests"`
Expected: PASS

**Step 3: Commit**

```bash
git add test/ModularPipelines.UnitTests/Attributes/SkipEventIntegrationTests.cs
git commit -m "test(attributes): add skip event integration test"
```

---

## Task 15: Add Metadata Cross-Phase Integration Test

**Files:**
- Test: `test/ModularPipelines.UnitTests/Attributes/MetadataCrossPhaseTests.cs`

**Step 1: Write the test**

```csharp
// test/ModularPipelines.UnitTests/Attributes/MetadataCrossPhaseTests.cs
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class MetadataCrossPhaseTests : TestBase
{
    public static string? MetadataFromEnd;

    public class SetMetadataAttribute : Attribute, IModuleRegistrationEventReceiver
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.SetMetadata("test-key", "test-value");
            return Task.CompletedTask;
        }
    }

    public class ReadMetadataAttribute : Attribute, IModuleEndEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleEndAsync(IModuleEventContext context, ModularPipelines.Models.ModuleResult result)
        {
            MetadataFromEnd = context.GetMetadata<string>("test-key");
            return Task.CompletedTask;
        }
    }

    [SetMetadata]
    [ReadMetadata]
    private class MetadataModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("done");
        }
    }

    [Before(Test)]
    public void ResetState()
    {
        MetadataFromEnd = null;
    }

    [Test]
    public async Task Metadata_SetAtRegistration_AvailableAtEnd()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<MetadataModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(MetadataFromEnd).IsEqualTo("test-value");
    }
}
```

**Step 2: Run test to verify it passes**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "MetadataCrossPhaseTests"`
Expected: PASS

**Step 3: Commit**

```bash
git add test/ModularPipelines.UnitTests/Attributes/MetadataCrossPhaseTests.cs
git commit -m "test(attributes): add metadata cross-phase integration test"
```

---

## Task 16: Run Full Test Suite and Final Verification

**Step 1: Build entire solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeded

**Step 2: Run all attribute-related tests**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0 -- --filter "Attributes"`
Expected: All tests pass

**Step 3: Run full unit test suite**

Run: `dotnet run --project test/ModularPipelines.UnitTests --framework net10.0`
Expected: All tests pass

**Step 4: Final commit**

```bash
git add .
git commit -m "feat(attributes): complete enhanced attribute system implementation

Implements issue #1403 - Enhanced Attribute System with Composition
and Lifecycle Event Receivers.

New Features:
- IModuleRegistrationEventReceiver for dynamic dependency configuration
- IModuleStartEventReceiver, IModuleEndEventReceiver for lifecycle hooks
- IModuleFailureEventReceiver, IModuleSkippedEventReceiver for error handling
- IModuleRegistrationContext for registration-time configuration
- IModuleEventContext for runtime event handling
- Metadata support across registration and execution phases
- Declaration-order receiver execution
- Configurable ContinueOnError per receiver

All new features complement existing hook system (IPipelineModuleHooks)
without breaking backward compatibility."
```

---

## Summary

This plan implements the enhanced attribute system in 16 tasks:

1. **Tasks 1-3**: Create public interfaces (event receivers, contexts)
2. **Tasks 4-7**: Create internal implementations (registries, contexts)
3. **Tasks 8-9**: Create invocation infrastructure
4. **Task 10**: Wire up DI registrations
5. **Tasks 11-12**: Integrate into pipeline execution
6. **Tasks 13-15**: Add comprehensive integration tests
7. **Task 16**: Final verification

Each task follows TDD: write failing test, implement, verify, commit.

---

Plan complete and saved to `docs/plans/2025-12-22-attribute-system-implementation.md`. Two execution options:

**1. Subagent-Driven (this session)** - I dispatch fresh subagent per task, review between tasks, fast iteration

**2. Parallel Session (separate)** - Open new session with executing-plans, batch execution with checkpoints

Which approach?
