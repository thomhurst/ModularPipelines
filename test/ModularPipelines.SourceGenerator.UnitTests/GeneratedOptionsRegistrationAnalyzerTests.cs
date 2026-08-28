using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace ModularPipelines.SourceGenerator.UnitTests;

public class GeneratedOptionsRegistrationAnalyzerTests
{
    private const string Infrastructure = """
        namespace ModularPipelines.Options
        {
            public abstract class CommandLineToolOptions;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(System.AttributeTargets.Property)]
            public class SecretValueAttribute : System.Attribute;
        }

        namespace Microsoft.Extensions.DependencyInjection
        {
            public interface IServiceCollection;

            public static class OptionsServiceCollectionExtensions
            {
                public static IServiceCollection Configure<TOptions>(
                    this IServiceCollection services,
                    System.Action<TOptions> configureOptions) => services;
            }
        }

        namespace ModularPipelines.Generated
        {
            public static class RuntimeMetadataRegistry
            {
                public const int CurrentCommandMetadataSchemaVersion = 3;
                public static void RegisterCommandOptions(System.Type type, object model) { }
                public static void RegisterCommandOptions(System.Type type, object model, int schemaVersion) { }
                public static void RegisterSecrets(System.Type type, object accessors) { }
            }
        }
        """;

    [Test]
    public async Task Trimmed_Host_Rejects_Peer_Generated_Options_Registration()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerOptionsGenerator(),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        var diagnostic = diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0017");
            await Assert.That(diagnostic.Severity).IsEqualTo(DiagnosticSeverity.Error);
            await Assert.That(diagnostic.GetMessage())
                .Contains("global::PeerGeneratedOptions");
            await Assert.That(diagnostic.GetMessage())
                .Contains("emitted by another source generator");
        }
    }

    [Test]
    public async Task Jit_Host_Allows_Peer_Generated_Options_Registration()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerOptionsGenerator(),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;");

        await Assert.That(diagnostics).IsEmpty();
    }

    [Test]
    public async Task Trimmed_Host_Recognizes_Generated_Tree_Without_Name_Convention()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerSourceGenerator(
                "PeerOptions.cs",
                """
                using Microsoft.Extensions.DependencyInjection;

                public sealed class ConventionFreePeerOptions;

                public static class PeerRegistration
                {
                    public static void Add(IServiceCollection services) =>
                        services.Configure<ConventionFreePeerOptions>(static _ => { });
                }
                """),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishTrimmed"] = "true",
            });

        var diagnostic = diagnostics.Single();
        await Assert.That(diagnostic.GetMessage()).Contains("ConventionFreePeerOptions");
    }

    [Test]
    public async Task Trimmed_Host_Accepts_Cooperating_Peer_Metadata()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerSourceGenerator(
                "PeerCommandOptions.cs",
                """
                using ModularPipelines.Generated;
                using ModularPipelines.Options;

                public sealed class PeerCommandOptions : CommandLineToolOptions;

                public static class PeerRegistration
                {
                    public static void Add()
                    {
                        RuntimeMetadataRegistry.RegisterCommandOptions(
                            typeof(PeerCommandOptions),
                            new object(),
                            RuntimeMetadataRegistry.CurrentCommandMetadataSchemaVersion);
                        RuntimeMetadataRegistry.RegisterSecrets(
                            typeof(PeerCommandOptions),
                            new object());
                    }
                }
                """),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        await Assert.That(diagnostics).IsEmpty();
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Legacy_Peer_Command_Metadata()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerSourceGenerator(
                "LegacyPeerCommandOptions.cs",
                """
                using ModularPipelines.Generated;
                using ModularPipelines.Options;

                public sealed class LegacyPeerCommandOptions : CommandLineToolOptions;

                public static class LegacyPeerRegistration
                {
                    public static void Add()
                    {
                        RuntimeMetadataRegistry.RegisterCommandOptions(
                            typeof(LegacyPeerCommandOptions),
                            new object());
                        RuntimeMetadataRegistry.RegisterSecrets(
                            typeof(LegacyPeerCommandOptions),
                            new object());
                    }
                }
                """),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0017");
            await Assert.That(diagnostic.GetMessage()).Contains("LegacyPeerCommandOptions");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Stale_Peer_Command_Metadata_Schema()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerSourceGenerator(
                "StalePeerCommandOptions.cs",
                """
                using ModularPipelines.Generated;
                using ModularPipelines.Options;

                public sealed class StalePeerCommandOptions : CommandLineToolOptions;

                public static class StalePeerRegistration
                {
                    public static void Add()
                    {
                        RuntimeMetadataRegistry.RegisterCommandOptions(
                            typeof(StalePeerCommandOptions),
                            new object(),
                            1);
                        RuntimeMetadataRegistry.RegisterSecrets(
                            typeof(StalePeerCommandOptions),
                            new object());
                    }
                }
                """),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = diagnostics.Single();
        using (Assert.Multiple())
        {
            await Assert.That(diagnostic.Id).IsEqualTo("MPG0017");
            await Assert.That(diagnostic.GetMessage()).Contains("StalePeerCommandOptions");
        }
    }

    [Test]
    public async Task Trimmed_Host_Rejects_Direct_Peer_Generated_Command_Options()
    {
        var diagnostics = await GeneratorTestHarness.RunWithPeerGeneratorAndAnalyzer(
            new CommandOptionsGenerator(),
            new PeerSourceGenerator(
                "DirectCommandOptions.cs",
                """
                using ModularPipelines.Options;

                public sealed class DirectPeerCommandOptions : CommandLineToolOptions;
                """),
            new GeneratedOptionsRegistrationAnalyzer(),
            Infrastructure,
            "public sealed class Host;",
            new Dictionary<string, string>
            {
                ["build_property.PublishAot"] = "true",
            });

        var diagnostic = diagnostics.Single();
        await Assert.That(diagnostic.GetMessage()).Contains("DirectPeerCommandOptions");
    }

    private sealed class PeerOptionsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(static output => output.AddSource(
                "PeerOptions.g.cs",
                SourceText.From(
                    """
                    // <auto-generated/>
                    using Microsoft.Extensions.DependencyInjection;

                    public sealed class PeerGeneratedOptions;

                    public static class PeerRegistration
                    {
                        public static void Add(IServiceCollection services) =>
                            services.Configure<PeerGeneratedOptions>(static _ => { });
                    }
                    """,
                    System.Text.Encoding.UTF8)));
        }
    }

    private sealed class PeerSourceGenerator(string hintName, string source) : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(
                context.CompilationProvider,
                (output, _) => output.AddSource(
                    hintName,
                    SourceText.From(source, System.Text.Encoding.UTF8)));
        }
    }
}
