using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Kubernetes.Services;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KustomizeGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, "kustomize --stack-trace")]
    [Arguments(false, "kustomize")]
    [Arguments(null, "kustomize")]
    public async Task Root_Renders_Only_Enabled_Stack_Trace(bool? stackTrace, string expected)
    {
        var rendered = await RenderCommand(new KustomizeRootOptions { StackTrace = stackTrace });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    [Arguments(true, "kustomize --stack-trace edit")]
    [Arguments(false, "kustomize edit")]
    [Arguments(null, "kustomize edit")]
    public async Task Stack_Trace_Precedes_Group(bool? stackTrace, string expected)
    {
        var rendered = await RenderCommand(new KustomizeEditOptions { StackTrace = stackTrace });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    [Arguments(true, "kustomize --stack-trace build overlays/test")]
    [Arguments(false, "kustomize build overlays/test")]
    [Arguments(null, "kustomize build overlays/test")]
    public async Task Stack_Trace_Precedes_Command_And_Operand(bool? stackTrace, string expected)
    {
        var rendered = await RenderCommand(new KustomizeBuildOptions { StackTrace = stackTrace, Dir = "overlays/test" });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    public async Task Nested_Command_Keeps_Repeated_Values_Separate()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new KustomizeEditAddConfigmapOptions("settings")
        {
            StackTrace = true,
            FromLiteral = ["first=hello world", "second=value"],
        });
        await Assert.That(string.Join("|", command.Arguments))
            .IsEqualTo("--stack-trace|edit|add|configmap|settings|--from-literal=first=hello world|--from-literal=second=value");
    }

    [Test]
    public async Task Edit_Wrappers_Render_Required_Operands_And_Inherited_Stack_Trace()
    {
        var command = await GetService<ICommandContext>();
        var edit = new KustomizeEdit(command);
        var execution = new CommandExecutionOptions { InternalDryRun = true };

        var annotation = await edit.Add.AnnotationAsync(
            new KustomizeEditAddAnnotationOptions(["team:platform", "owner:build"]) { StackTrace = true }, execution);
        var configuration = await edit.Add.ConfigurationAsync(
            new KustomizeEditAddConfigurationOptions(["first.yaml", "second.yaml"]) { StackTrace = true }, execution);
        var component = await edit.Remove.ComponentAsync(
            new KustomizeEditRemoveComponentOptions(["components/first", "components/second"]) { StackTrace = true }, execution);
        var namespaceResult = await edit.Set.NamespaceAsync(
            new KustomizeEditSetNamespaceOptions("integration") { StackTrace = true }, execution);

        await Assert.That(annotation.CommandInput)
            .IsEqualTo("kustomize --stack-trace edit add annotation team:platform owner:build");
        await Assert.That(configuration.CommandInput)
            .IsEqualTo("kustomize --stack-trace edit add configuration first.yaml second.yaml");
        await Assert.That(component.CommandInput)
            .IsEqualTo("kustomize --stack-trace edit remove component components/first components/second");
        await Assert.That(namespaceResult.CommandInput).IsEqualTo("kustomize --stack-trace edit set namespace integration");
    }

    [Test]
    public async Task Required_Edit_Operands_Reject_Null_And_Empty_Collections()
    {
        await Assert.That(() => new KustomizeEditAddAnnotationOptions(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => new KustomizeEditAddAnnotationOptions([])).Throws<ArgumentException>();
        await Assert.That(() => new KustomizeEditAddConfigurationOptions([])).Throws<ArgumentException>();
        await Assert.That(() => new KustomizeEditRemoveComponentOptions([])).Throws<ArgumentException>();
        await Assert.That(() => new KustomizeEditSetNamespaceOptions(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Only_Stack_Trace_Is_Inherited()
    {
        await Assert.That(typeof(KustomizeOptions)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(property => property.Name))
            .IsEquivalentTo(["StackTrace"]);

        foreach (var type in typeof(KustomizeOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(KustomizeOptions).IsAssignableFrom(type)))
        {
            var property = type.GetProperties().Single(property => property.Name == "StackTrace");
            await Assert.That(property.DeclaringType).IsEqualTo(typeof(KustomizeOptions));
        }

        await Assert.That(typeof(KubernetesOptions).GetProperty("StackTrace")).IsNull();
    }

    private record KustomizeRootOptions : KustomizeOptions;
}
