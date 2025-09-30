using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Attributes;
using ModularPipelines.Extensions;
using ModularPipelines.Host;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Extensions;

/// <summary>
/// Extension methods for registering modules with OS-specific requirements in a matrix-style pattern.
/// </summary>
public static class MatrixModuleExtensions
{
    private static readonly ConcurrentDictionary<Type, int> _typeCounter = new();

    /// <summary>
    /// Registers a module that will only execute on workers with the specified operating system.
    /// Creates a dynamic derived type with the RequiresOs attribute applied.
    /// </summary>
    /// <typeparam name="TModule">The type of module to register.</typeparam>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="operatingSystem">The required operating system.</param>
    /// <returns>The pipeline host builder for chaining.</returns>
    /// <example>
    /// <code>
    /// builder.AddModuleForOs&lt;TestModule&gt;(OS.Linux);
    /// </code>
    /// </example>
    public static PipelineHostBuilder AddModuleForOs<TModule>(
        this PipelineHostBuilder builder,
        OS operatingSystem)
        where TModule : ModuleBase
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Create a dynamic derived type with the RequiresOs attribute
        var derivedType = CreateDerivedTypeWithOsRequirement<TModule>(operatingSystem);

        builder.ConfigureServices((context, services) =>
        {
            services.AddModule(derivedType);
        });

        return builder;
    }

    /// <summary>
    /// Registers multiple instances of the same module, one for each specified operating system.
    /// This creates a "matrix" of module executions across different OS platforms.
    /// </summary>
    /// <typeparam name="TModule">The type of module to register.</typeparam>
    /// <param name="builder">The pipeline host builder.</param>
    /// <param name="operatingSystems">The operating systems to run the module on.</param>
    /// <returns>The pipeline host builder for chaining.</returns>
    /// <example>
    /// <code>
    /// // This will register TestModule 3 times - once for each OS
    /// builder.AddModuleForEachOs&lt;TestModule&gt;(
    ///     OS.Windows,
    ///     OS.Linux,
    ///     OS.MacOS);
    /// </code>
    /// </example>
    public static PipelineHostBuilder AddModuleForEachOs<TModule>(
        this PipelineHostBuilder builder,
        params OS[] operatingSystems)
        where TModule : ModuleBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(operatingSystems);

        if (operatingSystems.Length == 0)
        {
            throw new ArgumentException("At least one operating system must be specified.", nameof(operatingSystems));
        }

        foreach (var os in operatingSystems)
        {
            builder.AddModuleForOs<TModule>(os);
        }

        return builder;
    }

    private static Type CreateDerivedTypeWithOsRequirement<TModule>(OS operatingSystem)
        where TModule : ModuleBase
    {
        var baseType = typeof(TModule);
        var counter = _typeCounter.AddOrUpdate(baseType, 1, (_, count) => count + 1);
        var typeName = $"{baseType.Name}_ForOs_{operatingSystem}_{counter}";

        var assemblyName = new AssemblyName($"DynamicModules_{Guid.NewGuid():N}");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

        var typeBuilder = moduleBuilder.DefineType(
            typeName,
            TypeAttributes.Public | TypeAttributes.Class,
            baseType);

        // Add RequiresOs attribute
        var attributeConstructor = typeof(RequiresOsAttribute).GetConstructor(new[] { typeof(OS) })!;
        var attributeBuilder = new CustomAttributeBuilder(
            attributeConstructor,
            new object[] { operatingSystem });
        typeBuilder.SetCustomAttribute(attributeBuilder);

        return typeBuilder.CreateType()!;
    }
}
