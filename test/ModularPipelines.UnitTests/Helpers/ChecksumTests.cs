using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using ModularPipelines.FileSystem;

namespace ModularPipelines.UnitTests.Helpers;

public class HashFileTests : TestBase
{
    [Test]
    [Arguments("Md5", "90eaef2db61dd9b2af2b27f57785141e")]
    [Arguments("Sha1", "511149a32f67708d2926e88f018ba8675368fb51")]
    [Arguments("Sha256", "5571435802e3225a0e6e1029590ce13bb9722cecf8eda4430ccad8ce8bf3eccf")]
    [Arguments("Sha384", "972bfdbe7f2b71ce198e8cc404de823b61cbd1bb05c17362cd89927258c7228f3743f96f9526f1384bcc9e074a8256de")]
    [Arguments("Sha512", "cd0eb1ef8b0f664a6a2bfa736275c66a17933492968128c6e8f07c8fdff5634dd16e9b18d3cffe7b17e02953529294cbe594df1b8a61647cbc59e60e35cdcf5d")]
    public async Task FileHashProducesExpectedOutput(string algorithm, string expected)
    {
        var file = new FilePath(Path.Combine(TestContext.OutputDirectory!, "Data", "Foo.txt"));

        var hash = await GetService<IHashContext>();

        var result = algorithm switch
        {
            "Md5" => hash.Md5File(file),
            "Sha1" => hash.Sha1File(file),
            "Sha256" => hash.Sha256File(file),
            "Sha384" => hash.Sha384File(file),
            "Sha512" => hash.Sha512File(file),
            _ => throw new ArgumentException($"Unknown algorithm: {algorithm}", nameof(algorithm)),
        };

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("")]
    [Arguments(" ")]
    public async Task Md5_Blank_Path_Throws_FileNotFoundException(string filePath)
    {
        var hash = await GetService<IHashContext>();

        await Assert.That(() => hash.Md5File(filePath)).Throws<FileNotFoundException>();
    }
}
