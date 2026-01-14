using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.DependencyInjection;
using ModularPipelines.Engine;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;

namespace ModularPipelines.Extensions;

/// <summary>
/// Extensions for adding pipeline services to the service provider.
/// </summary>
/// <remarks>
/// <para>
/// These extension methods provide a fluent API for registering modules, requirements, and hooks
/// within the dependency injection container. The most common usage is through
/// <see cref="Host.PipelineHostBuilder.ConfigureServices"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// PipelineHostBuilder.Create()
///     .ConfigureServices((context, services) =>
///     {
///         services.AddModule&lt;BuildModule&gt;()
///             .AddModule&lt;TestModule&gt;()
///             .AddModule&lt;DeployModule&gt;();
///     })
///     .ExecutePipelineAsync();
/// </code>
/// </example>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a Module to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A builder for configuring the module registration with tags or categories.</returns>
    /// <remarks>
    /// <para>
    /// The returned <see cref="IModuleRegistrationBuilder"/> allows chaining additional module registrations
    /// or configuring the module with metadata such as tags and categories.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Basic registration
    /// services.AddModule&lt;BuildModule&gt;();
    ///
    /// // With category metadata
    /// services.AddModule&lt;DeployModule&gt;()
    ///     .WithCategory("Production");
    ///
    /// // Chained registration
    /// services.AddModule&lt;BuildModule&gt;()
    ///     .AddModule&lt;TestModule&gt;()
    ///     .AddModule&lt;DeployModule&gt;();
    /// </code>
    /// </example>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IServiceCollection services)
        where TModule : class, IModule
    {
        services.AddSingleton<IModule, TModule>();
        return new ModuleRegistrationBuilder(services, typeof(TModule));
    }

    /// <summary>
    /// Adds a pre-created Module instance to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tModule">The module instance to add.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <returns>A builder for configuring the module registration with tags or categories.</returns>
    /// <remarks>
    /// <para>
    /// This overload is useful when you need to pass constructor arguments to the module
    /// or when you want to share a module instance.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var buildModule = new BuildModule(buildConfiguration);
    /// services.AddModule(buildModule);
    /// </code>
    /// </example>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IServiceCollection services, TModule tModule)
        where TModule : class, IModule
    {
        services.AddSingleton<IModule>(tModule);
        return new ModuleRegistrationBuilder(services, typeof(TModule));
    }

    /// <summary>
    /// Adds a Module to the pipeline using a factory method.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModule">The type of Module to add.</typeparam>
    /// <param name="tModuleFactory">A factory method for creating the module.</param>
    /// <returns>A builder for configuring the module registration with tags or categories.</returns>
    /// <remarks>
    /// <para>
    /// This overload is useful when the module requires dependencies from the service provider
    /// or when you need custom initialization logic.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddModule&lt;BuildModule&gt;(sp =>
    /// {
    ///     var config = sp.GetRequiredService&lt;IBuildConfiguration&gt;();
    ///     return new BuildModule(config);
    /// });
    /// </code>
    /// </example>
    public static IModuleRegistrationBuilder AddModule<TModule>(this IServiceCollection services, Func<IServiceProvider, TModule> tModuleFactory)
        where TModule : class, IModule
    {
        services.AddSingleton<IModule>(tModuleFactory);
        return new ModuleRegistrationBuilder(services, typeof(TModule));
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement, TRequirement>();
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tRequirement">The requirement to add.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, TRequirement tRequirement)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirement);
    }

    /// <summary>
    /// Adds a requirement to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="tRequirementFactory">A factory method for creating the requirement.</param>
    /// <typeparam name="TRequirement">The type of requirement to add.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddRequirement<TRequirement>(this IServiceCollection services, Func<IServiceProvider, TRequirement> tRequirementFactory)
        where TRequirement : class, IPipelineRequirement
    {
        return services.AddSingleton<IPipelineRequirement>(tRequirementFactory);
    }

    /// <summary>
    /// Adds a requirement instance to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="requirement">The requirement instance to add.</param>
    /// <returns>The pipeline's same service collection.</returns>
    /// <remarks>
    /// This overload is useful with the <see cref="Require"/> factory class.
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddRequirement(Require.EnvironmentVariable("API_KEY"));
    /// services.AddRequirement(Require.That(ctx => Environment.Is64BitProcess, "64-bit required"));
    /// </code>
    /// </example>
    public static IServiceCollection AddRequirement(this IServiceCollection services, IPipelineRequirement requirement)
    {
        return services.AddSingleton(requirement);
    }

    /// <summary>
    /// Adds all modules from an assembly to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="T">Any type from the assembly to scan for modules.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    /// <remarks>
    /// <para>
    /// This method discovers and registers all concrete, non-abstract classes that implement
    /// <see cref="IModule"/> from the assembly containing the specified type.
    /// </para>
    /// <para>
    /// Circular dependencies are validated at registration time to provide early feedback.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Registers all modules from the assembly containing BuildModule
    /// services.AddModulesFromAssemblyContainingType&lt;BuildModule&gt;();
    /// </code>
    /// </example>
    /// <exception cref="Exceptions.CircularDependencyException">Thrown when a circular dependency is detected among the modules.</exception>
    public static IServiceCollection AddModulesFromAssemblyContainingType<T>(this IServiceCollection services)
    {
        return AddModulesFromAssembly(services, typeof(T).Assembly);
    }

    /// <summary>
    /// Adds all modules from an assembly to the pipeline.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <param name="assembly">The assembly to scan for modules.</param>
    /// <returns>The pipeline's same service collection.</returns>
    /// <remarks>
    /// <para>
    /// This method discovers and registers all concrete, non-abstract classes that implement
    /// <see cref="IModule"/> from the specified assembly.
    /// </para>
    /// <para>
    /// Circular dependencies are validated at registration time. When combined with existing
    /// module registrations, the validation ensures the entire dependency graph is acyclic.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // Registers all modules from a specific assembly
    /// var modulesAssembly = typeof(BuildModule).Assembly;
    /// services.AddModulesFromAssembly(modulesAssembly);
    /// </code>
    /// </example>
    /// <exception cref="Exceptions.CircularDependencyException">Thrown when a circular dependency is detected among the modules.</exception>
    public static IServiceCollection AddModulesFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var modules = assembly.GetTypes()
            .Where(type => type.IsAssignableTo(typeof(IModule)))
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract)
            .Where(type => !type.IsGenericTypeDefinition) // Skip open generic types - DI cannot instantiate them
            .ToList();

        // Get already registered module types from the service collection
        var existingModuleTypes = services
            .Where(sd => sd.ServiceType == typeof(IModule))
            .Select(sd => sd.ImplementationType ?? sd.ImplementationInstance?.GetType())
            .Where(t => t != null)
            .Cast<Type>()
            .ToList();

        // Combine existing modules with new modules for cycle detection
        var allModuleTypes = existingModuleTypes.Concat(modules).Distinct().ToList();

        // Validate for circular dependencies before registration
        DependencyGraphValidator.ValidateNoCycles(allModuleTypes);

        foreach (var module in modules)
        {
            services.AddSingleton(typeof(IModule), module);
        }

        return services;
    }

    /// <summary>
    /// Adds global hooks to run before or after all the modules have executed.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TGlobalSetup">The type of hook class.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddPipelineGlobalHooks<TGlobalSetup>(this IServiceCollection services)
        where TGlobalSetup : class, IPipelineGlobalHooks
    {
        return services.AddSingleton<IPipelineGlobalHooks, TGlobalSetup>();
    }

    /// <summary>
    /// Adds module hooks to run before or after each module has executed.
    /// </summary>
    /// <param name="services">The pipeline's service collection.</param>
    /// <typeparam name="TModuleHooks">The type of hook class.</typeparam>
    /// <returns>The pipeline's same service collection.</returns>
    public static IServiceCollection AddPipelineModuleHooks<TModuleHooks>(this IServiceCollection services)
        where TModuleHooks : class, IPipelineModuleHooks
    {
        return services.AddSingleton<IPipelineModuleHooks, TModuleHooks>();
    }

    internal static IServiceCollection AddServiceCollection(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IPipelineServiceContainerWrapper>(new PipelineServiceContainerWrapper(serviceCollection));
    }
}