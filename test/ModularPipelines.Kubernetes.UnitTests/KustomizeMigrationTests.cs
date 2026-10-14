using ModularPipelines.Kubernetes.Extensions;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Kubernetes.Services;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KustomizeMigrationTests : TestBase
{
    [Test]
    public async Task Release_Note_Operand_Examples_Render_Required_Values()
    {
        var (kustomize, _) = await GetService<IKustomize>(services => services.RegisterKustomizeContext());
        var execution = new CommandExecutionOptions { InternalDryRun = true };

        var namespaceResult = await kustomize.Edit.Set.NamespaceAsync(
            new KustomizeEditSetNamespaceOptions("production"), execution);
        var baseResult = await kustomize.Edit.Add.BaseAsync(
            new KustomizeEditAddBaseOptions("../base"), execution);

        await Assert.That(namespaceResult.CommandInput).IsEqualTo("kustomize edit set namespace production");
        await Assert.That(baseResult.CommandInput).IsEqualTo("kustomize edit add base ../base");
    }

    [Test]
    public async Task Patch_Commands_Keep_Optional_Options()
    {
        var (kustomize, _) = await GetService<IKustomize>(services => services.RegisterKustomizeContext());
        var execution = new CommandExecutionOptions { InternalDryRun = true };

        var addResult = await kustomize.Edit.Add.PatchAsync(executionOptions: execution);
        var removeResult = await kustomize.Edit.Remove.PatchAsync(executionOptions: execution);

        await Assert.That(addResult.CommandInput).IsEqualTo("kustomize edit add patch");
        await Assert.That(removeResult.CommandInput).IsEqualTo("kustomize edit remove patch");
    }
}
