using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Buildah.Options;
using TUnit.Assertions;
using TUnit.Core;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Buildah.UnitTests;

public class GlobalOptionsTests
{
    [Test]
    public async Task Persistent_Storage_Settings_Render_After_Command_With_Repeated_Values()
    {
        var options = new BuildahImagesOptions
        {
            Root = "storage-root",
            Runroot = "storage-state",
            StorageOpt = ["overlay.mount_program=/usr/bin/fuse-overlayfs", "overlay.ignore_chown_errors=true"],
        };
        await AssertArguments(BuildArguments(options),
            ["--root=storage-root", "--runroot=storage-state", "--storage-opt=overlay.mount_program=/usr/bin/fuse-overlayfs", "--storage-opt=overlay.ignore_chown_errors=true"]);
        await Assert.That(await RenderCommand(options)).IsEqualTo(
            "buildah images --root=storage-root --runroot=storage-state --storage-opt=overlay.mount_program=/usr/bin/fuse-overlayfs --storage-opt=overlay.ignore_chown_errors=true");
    }

    [Test]
    public async Task Local_Mapping_Override_Uses_One_Shared_Value_With_Base_Assignments()
    {
        var options = new BuildahFromOptions { UsernsUidMap = ["0:1000:1"] };
        ((BuildahOptions) options).UsernsUidMap = ["0:2000:1", "1:3000:2"];
        await AssertArguments(BuildArguments(options), ["--userns-uid-map=0:2000:1", "--userns-uid-map=1:3000:2"]);
        await Assert.That(options.UsernsUidMap).IsEquivalentTo(["0:2000:1", "1:3000:2"]);
        await Assert.That(typeof(BuildahFromOptions).GetProperty(nameof(BuildahOptions.UsernsUidMap))!.DeclaringType)
            .IsEqualTo(typeof(BuildahFromOptions));
    }

    [Test]
    public async Task Inheritance_Preserves_Local_Flags_And_Excludes_Hidden_Root_Settings()
    {
        await Assert.That(typeof(BuildahOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Count().IsEqualTo(11);
        await Assert.That(typeof(BuildahImagesOptions).GetProperty(nameof(BuildahOptions.Root))!.DeclaringType)
            .IsEqualTo(typeof(BuildahOptions));
        await Assert.That(typeof(BuildahImagesOptions).GetProperty(nameof(BuildahImagesOptions.All))!
            .GetCustomAttribute<CliFlagAttribute>()!.ShortForm).IsEqualTo("-a");
        foreach (var property in new[] { "Debug", "CpuProfile", "MemoryProfile", "DefaultMountsFile", "All", "Format" })
        {
            await Assert.That(typeof(BuildahOptions).GetProperty(property)).IsNull();
        }
    }
}
