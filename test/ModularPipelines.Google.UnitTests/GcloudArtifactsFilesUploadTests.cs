using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudArtifactsFilesUploadTests
{
    [Test]
    [Arguments(false, null, null)]
    [Arguments(true, null, null)]
    [Arguments(false, "my-repository", "us-central1")]
    [Arguments(true, "projects/my-project/locations/us-central1/repositories/my-repository", null)]
    public async Task Upload_Renders_Source_And_Optional_Repository_Selectors(
        bool directory, string? repository, string? location)
    {
        var options = new GcloudArtifactsFilesUploadOptions
        {
            Source = directory ? null : "file.txt",
            SourceDirectory = directory ? "files" : null,
            Async = true,
            File = "remote-file",
            SkipExisting = true,
            Repository = repository,
            Location = location,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new ValidationContext(options), errors, true)).IsTrue();

        var expected = new List<string>
        {
            directory ? "--source-directory=files" : "--source=file.txt",
            "--async",
            "--file=remote-file",
            "--skip-existing",
        };
        if (location is not null)
        {
            expected.Add($"--location={location}");
        }

        if (repository is not null)
        {
            expected.Add($"--repository={repository}");
        }

        await AssertArguments(BuildArguments(options), expected);
    }

    [Test]
    [Arguments(null, null)]
    [Arguments(" ", "")]
    [Arguments("file.txt", "files")]
    public async Task Upload_Rejects_Missing_Or_Conflicting_Sources(string? source, string? directory)
    {
        var options = new GcloudArtifactsFilesUploadOptions
        {
            Source = source,
            SourceDirectory = directory,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new ValidationContext(options), errors, true)).IsFalse();
        await Assert.That(errors.Single().MemberNames).IsEquivalentTo(["Source", "SourceDirectory"]);
    }
}
