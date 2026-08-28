namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModuleEventMetadataGeneratorTests
{
    private const string Infrastructure = """
        namespace ModularPipelines
        {
            public sealed class PipelineBuilder;
        }

        namespace ModularPipelines.Modules
        {
            public interface IModule;

            public abstract class Module<T> : IModule;
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

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(
                System.AttributeTargets.Class,
                AllowMultiple = true,
                Inherited = true)]
            public abstract class DependsOnAttribute : System.Attribute;

            public sealed class DependsOnAttribute<TModule> : DependsOnAttribute
                where TModule : ModularPipelines.Modules.IModule;
        }
        """;

    [Test]
    public async Task Registered_Closed_Generic_Module_Emits_Event_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                using ModularPipelines;

                [System.AttributeUsage(System.AttributeTargets.Class)]
                public sealed class MarkerAttribute : System.Attribute;

                [Marker]
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

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<string>)");
            await Assert.That(generated)
                .Contains("new global::Consumer.MarkerAttribute()");
        }
    }

    [Test]
    public async Task Registered_Closed_Generic_Module_With_Inaccessible_Argument_Is_Skipped()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                using ModularPipelines;

                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Registration
                {
                    private sealed class PrivatePayload;

                    public static void Configure(ModularPipelines.PipelineBuilder builder)
                    {
                        builder.AddModule<GenericModule<PrivatePayload>>();
                    }
                }
            }
            """);

        var generated = string.Join(
            Environment.NewLine,
            result.GeneratedTrees.Select(static tree => tree.GetText().ToString()));

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("GenericModule<global::Consumer.Registration.PrivatePayload>");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0007"
                                        && diagnostic.GetMessage().Contains("PrivatePayload"));
        }
    }

    [Test]
    public async Task Closed_Generic_Dependency_Emits_Event_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                [System.AttributeUsage(System.AttributeTargets.Class)]
                public sealed class MarkerAttribute : System.Attribute;

                [Marker]
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<GenericModule<int>>]
                public sealed class ParentModule : ModularPipelines.Module<bool>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<int>)");
            await Assert.That(generated)
                .Contains("new global::Consumer.MarkerAttribute()");
        }
    }

    [Test]
    public async Task Closed_Generic_Dependency_With_Inaccessible_Argument_Is_Skipped()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                public static class Container
                {
                    private sealed class PrivatePayload;

                    [ModularPipelines.DependsOn<GenericModule<PrivatePayload>>]
                    public sealed class ParentModule : ModularPipelines.Module<bool>;
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("GenericModule<global::Consumer.Container.PrivatePayload>");
            await Assert.That(generated).Contains("typeof(global::Consumer.Container.ParentModule)");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0007"
                                        && diagnostic.GetMessage().Contains("PrivatePayload"));
        }
    }

    [Test]
    public async Task Inferred_Closed_Generic_Registrations_Emit_Event_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                using ModularPipelines;

                [System.AttributeUsage(System.AttributeTargets.Class)]
                public sealed class MarkerAttribute : System.Attribute;

                [Marker]
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
                .Contains("typeof(global::Consumer.GenericModule<int>)");
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<string>)");
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<long>)");
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<decimal>)");
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<bool>)");
            await Assert.That(generated)
                .Contains("new global::Consumer.MarkerAttribute()");
        }
    }

    [Test]
    public async Task Transitive_Closed_Generic_Dependency_Emits_Event_Metadata()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                [System.AttributeUsage(System.AttributeTargets.Class)]
                public sealed class MarkerAttribute : System.Attribute;

                [Marker]
                public sealed class LeafModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<LeafModule<string>>]
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<GenericModule<int>>]
                public sealed class ParentModule : ModularPipelines.Module<bool>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.GenericModule<int>)");
            await Assert.That(generated)
                .Contains("typeof(global::Consumer.LeafModule<string>)");
            await Assert.That(generated)
                .Contains("new global::Consumer.MarkerAttribute()");
        }
    }

    [Test]
    public async Task Transitive_Closed_Generic_Dependency_Cycle_Emits_Each_Module_Once()
    {
        var result = GeneratorTestRunner.Run(
            new ModuleEventMetadataGenerator(),
            Infrastructure,
            """
            namespace Consumer
            {
                [ModularPipelines.DependsOn<GenericModule<int>>]
                public sealed class LeafModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<LeafModule<string>>]
                public sealed class GenericModule<T> : ModularPipelines.Module<T>;

                [ModularPipelines.DependsOn<GenericModule<int>>]
                public sealed class ParentModule : ModularPipelines.Module<bool>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated
                .Split(
                    "typeof(global::Consumer.GenericModule<int>)",
                    StringSplitOptions.None)
                .Length - 1).IsEqualTo(1);
            await Assert.That(generated
                .Split(
                    "typeof(global::Consumer.LeafModule<string>)",
                    StringSplitOptions.None)
                .Length - 1).IsEqualTo(1);
        }
    }
}
