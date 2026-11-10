// Only messages authored here or by the artifact helper may reach action logs.
// SDK errors can contain signed storage URLs, including inside their causes.
export class ArtifactValidationError extends Error {}

export function artifactErrorMessage(error) {
  return error instanceof ArtifactValidationError
    ? error.message
    : 'GitHub build-output artifact transfer failed.';
}
