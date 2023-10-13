*   Yarn CLI helpers
*   Breaking: Refactored `KeyValueVariables` into an `IEnumerable<KeyValue>`
*   Better npm Models
*   Breaking: HttpLoggingType enum has changed from RequestOnly, RequestAndResponse, ResponseOnly, to a combination enum via Bitwise flags. The logging type can now be built in a more fluid and customisable way. E.g. `HttpLoggingType.Request | HttpLoggingType.Response | HttpLoggingType.StatusCode | HttpLoggingType.Duration`  