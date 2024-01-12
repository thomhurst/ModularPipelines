using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudHealthcare
{
    public GcloudHealthcare(
        GcloudHealthcareConsentStores consentStores,
        GcloudHealthcareDatasets datasets,
        GcloudHealthcareDicomStores dicomStores,
        GcloudHealthcareFhirStores fhirStores,
        GcloudHealthcareHl7v2Stores hl7v2Stores,
        GcloudHealthcareOperations operations
    )
    {
        ConsentStores = consentStores;
        Datasets = datasets;
        DicomStores = dicomStores;
        FhirStores = fhirStores;
        Hl7v2Stores = hl7v2Stores;
        Operations = operations;
    }

    public GcloudHealthcareConsentStores ConsentStores { get; }

    public GcloudHealthcareDatasets Datasets { get; }

    public GcloudHealthcareDicomStores DicomStores { get; }

    public GcloudHealthcareFhirStores FhirStores { get; }

    public GcloudHealthcareHl7v2Stores Hl7v2Stores { get; }

    public GcloudHealthcareOperations Operations { get; }
}