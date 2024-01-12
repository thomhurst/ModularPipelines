using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDataCatalog
{
    public GcloudDataCatalog(
        GcloudDataCatalogEntries entries,
        GcloudDataCatalogEntryGroups entryGroups,
        GcloudDataCatalogTagTemplates tagTemplates,
        GcloudDataCatalogTags tags,
        GcloudDataCatalogTaxonomies taxonomies,
        ICommand internalCommand
    )
    {
        Entries = entries;
        EntryGroups = entryGroups;
        TagTemplates = tagTemplates;
        Tags = tags;
        Taxonomies = taxonomies;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDataCatalogEntries Entries { get; }

    public GcloudDataCatalogEntryGroups EntryGroups { get; }

    public GcloudDataCatalogTagTemplates TagTemplates { get; }

    public GcloudDataCatalogTags Tags { get; }

    public GcloudDataCatalogTaxonomies Taxonomies { get; }

    public async Task<CommandResult> Search(GcloudDataCatalogSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}