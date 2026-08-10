using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

public class ChecksumTests : TestBase
{
    [Test]
    public async Task Md5_Checksum()
    {
        var file = new File(Path.Combine(TestContext.OutputDirectory!, "Data", "Foo.txt"));

        var checksum = await GetService<IChecksumContext>();

        var calculatedMd5 = checksum.Md5(file);
        await Assert.That(calculatedMd5).IsEqualTo("90EAEF2DB61DD9B2AF2B27F57785141E");
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Md5_Blank_Path_Throws_FileNotFoundException(string filePath)
    {
        var checksum = await GetService<IChecksumContext>();

        await Assert.That(() => checksum.Md5(filePath)).Throws<FileNotFoundException>();
    }
}
