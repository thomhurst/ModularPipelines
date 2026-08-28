namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModuleMetadataGeneratorTests
{
    private const string TestInfrastructure = """
        namespace ModularPipelines.Modules
        {
            public interface IModule;
            public abstract class Module<T> : IModule;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(
                System.AttributeTargets.Class | System.AttributeTargets.Interface,
                AllowMultiple = true,
                Inherited = true)]
            public class DependsOnAttribute : System.Attribute
            {
                public DependsOnAttribute(System.Type type) => Type = type;
                public System.Type Type { get; }
                public bool Optional { get; set; }
            }

            public class DependsOnAttribute<T> : DependsOnAttribute
            {
                public DependsOnAttribute() : base(typeof(T))
                {
                }
            }
        }
        """;

    [Test]
    public async Task Generates_Registration_And_Inherited_Dependency_Metadata()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOn<DependencyModule>(Optional = true)]
                public interface IHasDependency;

                public abstract class BaseModule : ModularPipelines.Module<string>, IHasDependency;

                public sealed class BuildModule : BaseModule;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.BuildModule, string>");
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.DependencyModule), true)");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.DependencyModule, string>");
            await Assert.That(generated).Contains("isComplete: false");
        }
    }

    [Test]
    public async Task Duplicate_Inherited_Dependencies_Are_Emitted_Once()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOn<DependencyModule>]
                public abstract class BaseModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOn<DependencyModule>]
                public sealed class BuildModule : BaseModule;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();
        var buildRegistration = generated[generated.IndexOf(
            "CreateRegistration<global::Consumer.BuildModule, string>",
            StringComparison.Ordinal)..];
        buildRegistration = buildRegistration[..buildRegistration.IndexOf(
            "dependenciesComplete:",
            StringComparison.Ordinal)];

        await Assert.That(CountOccurrences(
            buildRegistration,
            "new(typeof(global::Consumer.DependencyModule), false)")).IsEqualTo(1);
    }

    [Test]
    public async Task Partial_Declarations_Produce_One_Registration()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed partial class BuildModule : ModularPipelines.Module<string>;
                public sealed partial class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        await Assert.That(CountOccurrences(
            generated,
            "CreateRegistration<global::Consumer.BuildModule, string>")).IsEqualTo(1);
    }

    [Test]
    public async Task Partial_Module_Dependency_Metadata_Remains_Incomplete()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOn<DependencyModule>]
                public sealed partial class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.DependencyModule), false)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0014"
                                        && diagnostic.GetMessage()
                                            .Contains("Consumer.BuildModule"));
        }
    }

    [Test]
    public async Task Empty_Assembly_Does_Not_Emit_Registration()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public static class Marker;
            }
            """);

        await Assert.That(result.GeneratedTrees).IsEmpty();
    }

    [Test]
    public async Task Inaccessible_Module_Uses_Incomplete_Assembly_Metadata()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;

                public static class Container
                {
                    private sealed class HiddenModule : ModularPipelines.Module<string>;
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.BuildModule, string>");
            await Assert.That(generated).DoesNotContain("HiddenModule");
            await Assert.That(generated).Contains("isComplete: false");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0011"
                                        && diagnostic.GetMessage().Contains("HiddenModule"));
        }
    }

    [Test]
    public async Task Open_Generic_Module_Is_Omitted_With_Assembly_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Module<string>;
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("GenericModule");
            await Assert.That(generated).Contains("isComplete: false");
        }
    }

    [Test]
    public async Task Closed_Generic_Dependency_Is_Emitted()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn(typeof(GenericModule<string>))]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.GenericModule<string>), false)");
            await Assert.That(CountOccurrences(
                generated,
                "CreateRegistration<global::Consumer.GenericModule<string>, string>")).IsEqualTo(1);
            await Assert.That(generated).Contains("dependenciesComplete: true");
        }
    }

    [Test]
    public async Task Inaccessible_Closed_Generic_Dependency_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public static class Container
                {
                    private sealed class GenericModule<T> : ModularPipelines.Module<T>;

                    [ModularPipelines.DependsOn<GenericModule<int>>]
                    public sealed class BuildModule : ModularPipelines.Module<string>;
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("GenericModule<int>");
            await Assert.That(generated).Contains("dependenciesComplete: false");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0011"
                                        && diagnostic.GetMessage()
                                            .Contains("Container.GenericModule<int>"));
        }
    }

    [Test]
    public async Task Transitive_Closed_Generic_Dependency_Metadata_Is_Emitted()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class LeafModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<LeafModule<string>>]
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<GenericModule<int>>]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<int>, int>");
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.LeafModule<string>), false)");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.LeafModule<string>, string>");
            await Assert.That(CountOccurrences(
                generated,
                "dependenciesComplete: true")).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Registered_Closed_Generic_Module_Is_Emitted()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;

                public static class PipelineBuilderExtensions
                {
                    public static PipelineBuilder AddModule<TModule>(this PipelineBuilder builder)
                        where TModule : class, Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        builder.AddModule<GenericModule<string>>();
                    }
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        await Assert.That(CountOccurrences(
            generated,
            "CreateRegistration<global::Consumer.GenericModule<string>, string>")).IsEqualTo(1);
    }

    [Test]
    public async Task Chained_Builder_Registrations_Are_Recognized()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;
            }

            namespace ModularPipelines
            {
                public static class PipelineBuilderExtensions
                {
                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public sealed class StarterModule : ModularPipelines.Module<string>;
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        builder.AddModule<StarterModule>()
                            .AddModule<GenericModule<string>>()
                            .AddModule<ModularPipelines.Modules.IModule>();
                    }

                    public static void Register<TModule>(ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule
                    {
                        builder.AddModule<StarterModule>().AddModule<TModule>();
                    }
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<string>, string>");
            await Assert.That(result.Diagnostics.Count(diagnostic => diagnostic.Id == "MPG0013"))
                .IsEqualTo(1);
            await Assert.That(result.Diagnostics.Count(diagnostic => diagnostic.Id == "MPG0015"))
                .IsEqualTo(1);
        }
    }

    [Test]
    public async Task Inferred_Closed_Generic_Registrations_Are_Emitted()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;
            }

            namespace ModularPipelines
            {
                public static class PipelineBuilderExtensions
                {
                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;

                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder,
                        TModule module)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;

                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder,
                        System.Func<System.IServiceProvider, TModule> factory)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        builder.AddModule(new GenericModule<int>());
                        builder.AddModule(_ => new GenericModule<string>());
                        builder?.AddModule<GenericModule<long>>();
                        builder?.AddModule(new GenericModule<decimal>());
                        builder?.AddModule(_ => new GenericModule<bool>());
                    }
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<int>, int>");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<string>, string>");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<long>, long>");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<decimal>, decimal>");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.GenericModule<bool>, bool>");
        }
    }

    [Test]
    public async Task Generic_Helper_Module_Registration_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;
            }

            namespace ModularPipelines
            {
                public static class PipelineBuilderExtensions
                {
                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    public static void Register<TModule>(ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule
                    {
                        builder.AddModule<TModule>();
                    }

                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        Register<GenericModule<int>>(builder);
                    }
                }
            }
            """);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0013"
                                        && diagnostic.GetMessage().Contains("AddModule<TModule>"));
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task NonConcrete_Module_Registrations_Report_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;
            }

            namespace ModularPipelines
            {
                public static class PipelineBuilderExtensions
                {
                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder,
                        TModule module)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;

                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder,
                        System.Func<System.IServiceProvider, TModule> factory)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        ModularPipelines.Modules.IModule module = new GenericModule<int>();
                        builder.AddModule(module);
                        builder.AddModule<ModularPipelines.Modules.IModule>(
                            _ => new GenericModule<string>());
                    }
                }
            }
            """);

        using (Assert.Multiple())
        {
            await Assert.That(result.Diagnostics.Count(diagnostic => diagnostic.Id == "MPG0015"))
                .IsEqualTo(2);
            await Assert.That(result.Diagnostics)
                .All(diagnostic => diagnostic.Id != "MPG0015"
                                   || diagnostic.GetMessage().Contains("IModule"));
            await Assert.That(result.GeneratedTrees).IsEmpty();
        }
    }

    [Test]
    public async Task Inherited_Selector_Dependency_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines.Attributes
            {
                public class DependsOnAllModulesInheritingFromAttribute : System.Attribute;

                public class DependsOnAllModulesInheritingFromAttribute<TModule>
                    : DependsOnAllModulesInheritingFromAttribute;
            }

            namespace Consumer
            {
                public abstract class BaseModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOnAllModulesInheritingFrom<BaseModule>]
                public interface IHasSelectorDependency;

                public sealed class BuildModule
                    : ModularPipelines.Module<string>, IHasSelectorDependency;
            }
            """);

        await Assert.That(result.Diagnostics)
            .Contains(diagnostic => diagnostic.Id == "MPG0016"
                                    && diagnostic.GetMessage().Contains("Consumer.BuildModule")
                                    && diagnostic.Descriptor.HelpLinkUri.EndsWith("#mpg0016"));
    }

    [Test]
    public async Task Predicate_Selector_Dependencies_Report_Aot_Diagnostics()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines.Attributes
            {
                public abstract class DependsOnBaseAttribute : System.Attribute;

                public sealed class DependsOnModulesWithTagAttribute(string tag)
                    : DependsOnBaseAttribute;

                public sealed class DependsOnModulesInCategoryAttribute(string category)
                    : DependsOnBaseAttribute;

                public sealed class DependsOnModulesWithAttributeAttribute<TAttribute>
                    : DependsOnBaseAttribute
                    where TAttribute : System.Attribute;

                public sealed class CustomSelectorAttribute : DependsOnBaseAttribute;
            }

            namespace Consumer
            {
                public sealed class MarkerAttribute : System.Attribute;

                [ModularPipelines.DependsOnModulesWithTag("build")]
                public sealed class TagModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOnModulesInCategory("deploy")]
                public sealed class CategoryModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOnModulesWithAttribute<MarkerAttribute>]
                public sealed class AttributeModule : ModularPipelines.Module<string>;

                [ModularPipelines.Attributes.CustomSelector]
                public sealed class CustomModule : ModularPipelines.Module<string>;
            }
            """);

        var diagnostics = result.Diagnostics
            .Where(diagnostic => diagnostic.Id == "MPG0016")
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(diagnostics).Count().IsEqualTo(4);
            await Assert.That(diagnostics.Select(static diagnostic => diagnostic.GetMessage()))
                .Contains(message => message.Contains("Consumer.TagModule"))
                .And.Contains(message => message.Contains("Consumer.CategoryModule"))
                .And.Contains(message => message.Contains("Consumer.AttributeModule"))
                .And.Contains(message => message.Contains("Consumer.CustomModule"));
        }
    }

    [Test]
    public async Task Custom_DependsOn_Subclass_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace ModularPipelines.Attributes
            {
                public sealed class CustomDependsOnAttribute : DependsOnAttribute
                {
                    public CustomDependsOnAttribute()
                        : base(typeof(Consumer.BaseModule))
                    {
                    }
                }
            }

            namespace Consumer
            {
                public sealed class BaseModule : ModularPipelines.Module<string>;

                [ModularPipelines.Attributes.CustomDependsOn]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        await Assert.That(result.Diagnostics)
            .Contains(diagnostic => diagnostic.Id == "MPG0016"
                                    && diagnostic.GetMessage().Contains("Consumer.BuildModule")
                                    && diagnostic.Descriptor.HelpLinkUri.EndsWith("#mpg0016"));
    }

    [Test]
    public async Task Registered_External_Closed_Generic_Module_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new ModuleMetadataGenerator(),
            TestInfrastructure,
            """
            namespace ExternalModules
            {
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;
            }
            """,
            """
            namespace ModularPipelines
            {
                public sealed class PipelineBuilder;
            }

            namespace ModularPipelines
            {
                public static class PipelineBuilderExtensions
                {
                    public static ModularPipelines.PipelineBuilder AddModule<TModule>(
                        this ModularPipelines.PipelineBuilder builder)
                        where TModule : class, ModularPipelines.Modules.IModule => builder;
                }
            }

            namespace Consumer
            {
                using ModularPipelines;

                public static class Registration
                {
                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        builder.AddModule<ExternalModules.GenericModule<string>>();
                    }
                }
            }
            """);

        using (Assert.Multiple())
        {
            await Assert.That(result.GeneratedTrees).IsEmpty();
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0012"
                                        && diagnostic.GetMessage()
                                            .Contains("ExternalModules.GenericModule<string>"));
        }
    }

    [Test]
    public async Task External_Closed_Generic_Dependency_Reports_Aot_Diagnostic()
    {
        var result = GeneratorTestHarness.RunWithExternalAssembly(
            new ModuleMetadataGenerator(),
            TestInfrastructure,
            """
            namespace ExternalModules
            {
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;
            }
            """,
            """
            namespace Consumer
            {
                [ModularPipelines.DependsOn<ExternalModules.GenericModule<string>>]
                public sealed class BuildModule : ModularPipelines.Module<bool>;
            }
            """);

        await Assert.That(result.Diagnostics)
            .Contains(diagnostic => diagnostic.Id == "MPG0012"
                                    && diagnostic.GetMessage()
                                        .Contains("ExternalModules.GenericModule<string>"));
    }

    [Test]
    public async Task Direct_IModule_Implementation_Is_Registered()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DirectModule : ModularPipelines.Modules.IModule;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        await Assert.That(generated)
            .Contains("CreateRegistration<global::Consumer.DirectModule>");
    }

    [Test]
    public async Task Invalid_Legacy_Dependency_Uses_Reflection_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                [ModularPipelines.DependsOn(typeof(string))]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("new(typeof(string)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
        }
    }

    [Test]
    public async Task Custom_Generic_Dependency_Attribute_Uses_Reflection_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Module<string>;

                public sealed class OptionalDependencyAttribute
                    : ModularPipelines.DependsOnAttribute<DependencyModule>
                {
                    public OptionalDependencyAttribute()
                    {
                        Optional = true;
                    }
                }

                [OptionalDependency]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .DoesNotContain("new(typeof(global::Consumer.DependencyModule)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
        }
    }

    [Test]
    public async Task Equivalent_Compilation_Uses_Incremental_Cache()
    {
        var result = GeneratorTestHarness.RunTwiceWithStepTracking(
            new ModuleMetadataGenerator(),
            TestInfrastructure,
            """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Module<string>;

                [ModularPipelines.DependsOn<DependencyModule>]
                public sealed class BuildModule : ModularPipelines.Module<string>;
            }
            """);

        await Assert.That(GeneratorTestHarness.HasCachedOrUnchangedOutput(result)).IsTrue();
    }

    private static int CountOccurrences(string source, string value)
    {
        var count = 0;
        var startIndex = 0;
        while ((startIndex = source.IndexOf(value, startIndex, StringComparison.Ordinal)) >= 0)
        {
            count++;
            startIndex += value.Length;
        }

        return count;
    }
}
