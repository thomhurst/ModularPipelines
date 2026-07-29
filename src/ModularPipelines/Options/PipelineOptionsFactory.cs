using Microsoft.Extensions.Options;

namespace ModularPipelines.Options;

internal sealed class PipelineOptionsFactory(
    PipelineOptions value,
    IEnumerable<IConfigureOptions<PipelineOptions>> setups,
    IEnumerable<IPostConfigureOptions<PipelineOptions>> postConfigures,
    IEnumerable<IValidateOptions<PipelineOptions>> validations)
    : IOptionsFactory<PipelineOptions>
{
    public PipelineOptions Create(string name)
    {
        foreach (var setup in setups)
        {
            if (setup is IConfigureNamedOptions<PipelineOptions> namedSetup)
            {
                namedSetup.Configure(name, value);
            }
            else if (name == Microsoft.Extensions.Options.Options.DefaultName)
            {
                setup.Configure(value);
            }
        }

        foreach (var postConfigure in postConfigures)
        {
            postConfigure.PostConfigure(name, value);
        }

        var failures = new List<string>();
        foreach (var validation in validations)
        {
            var result = validation.Validate(name, value);
            if (result.Failed)
            {
                failures.AddRange(result.Failures);
            }
        }

        return failures.Count == 0
            ? value
            : throw new OptionsValidationException(name, typeof(PipelineOptions), failures);
    }
}
