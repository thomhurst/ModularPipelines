using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.FileSystem;

public class LinuxMountTableTests
{
    private static LinuxMountTable CreateTable() => new(
    [
        "1 0 8:1 / / rw - ext4 /dev/root rw",
        "2 1 8:1 /source/child /alias rw - ext4 /dev/root rw",
        "3 1 8:1 /source /source-alias rw - ext4 /dev/root rw",
        "4 1 8:2 / /source/mounted rw - ext4 /dev/other rw",
        "5 1 8:2 /child /mounted-alias rw - ext4 /dev/other rw",
        "6 1 8:1 /unrelated /unrelated-alias rw - ext4 /dev/root rw",
        "7 1 8:1 /source/child /output/nested rw - ext4 /dev/root rw",
        "8 1 8:1 /source\\040space/child /space\\040alias rw - ext4 /dev/root rw",
        "9 1 8:1 /source\\134040/child /literal\\134040 rw - ext4 /dev/root rw",
        "10 1 8:1 / /ancestor-output/nested rw - ext4 /dev/root rw",
    ]);

    [Test]
    [Arguments("/source", "/alias")]
    [Arguments("/source", "/alias/missing/nested")]
    [Arguments("/source-alias", "/alias/missing")]
    [Arguments("/source", "/source-alias")]
    [Arguments("/source", "/mounted-alias")]
    [Arguments("/source", "/output")]
    [Arguments("/source", "/ancestor-output")]
    [Arguments("/source space", "/space alias/missing")]
    [Arguments("/source\\040", "/literal\\040/missing")]
    public async Task RejectsAliasesIntoSource(string source, string target)
    {
        var exception = await Assert.That(() => CreateTable().ValidateCopyTarget(source, target)).Throws<ArgumentException>();
        await Assert.That(exception.ParamName).IsEqualTo("targetPath");
    }

    [Test]
    [Arguments("/source", "/unrelated-alias")]
    [Arguments("/source", "/source-copy")]
    [Arguments("/source/child", "/source")]
    [Arguments("/alias", "/unrelated-alias/missing")]
    [Arguments("/source/mounted", "/alias")]
    public void AllowsUnrelatedDestinations(string source, string target)
    {
        CreateTable().ValidateCopyTarget(source, target);
    }

    [Test]
    public void IgnoresMountsHiddenByOvermount()
    {
        var table = new LinuxMountTable(
        [
            "1 0 8:1 / / rw - ext4 /dev/root rw",
            "2 1 8:1 /source /alias rw - ext4 /dev/root rw",
            "3 2 8:1 /source/child /alias/nested rw - ext4 /dev/root rw",
            "4 2 8:2 / /alias rw - ext4 /dev/other rw",
        ]);

        table.ValidateCopyTarget("/source", "/alias/nested/missing");
    }

    [Test]
    public async Task SupportsSelfParentedRootAndRootOvermount()
    {
        var table = new LinuxMountTable(
        [
            "1 1 8:1 / / rw - ext4 /dev/root rw",
            "2 1 8:2 / / rw - ext4 /dev/other rw",
            "3 2 8:2 /source/child /alias rw - ext4 /dev/other rw",
        ]);

        await Assert.That(() => table.ValidateCopyTarget("/source", "/alias")).Throws<ArgumentException>();
    }
}
