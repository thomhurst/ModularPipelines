import { createReadStream } from 'node:fs';
import { mkdtemp, readFile, rename, rm, stat, writeFile } from 'node:fs/promises';
import { createHash } from 'node:crypto';
import path from 'node:path';
import { pathToFileURL } from 'node:url';

const archiveName = '_build-output.tar.gz';
const manifestName = `${archiveName}.json`;

class ArtifactValidationError extends Error {}

async function describe(file) {
  const hash = createHash('sha256');
  for await (const chunk of createReadStream(file)) {
    hash.update(chunk);
  }
  return { size: (await stat(file)).size, sha256: hash.digest('hex') };
}

export async function transfer(client, operation, runId, repositoryRoot) {
  if (typeof runId !== 'string' || !/^[a-zA-Z0-9._-]+$/.test(runId)) {
    throw new ArtifactValidationError('A safe, attempt-scoped run ID is required.');
  }
  const name = `build-output-${runId}`;
  const archive = path.join(repositoryRoot, archiveName);
  if (operation === 'upload') {
    const manifest = path.join(repositoryRoot, manifestName);
    try {
      await writeFile(manifest, JSON.stringify(await describe(archive)));
      const result = await client.uploadArtifact(name, [archive, manifest], repositoryRoot, {
        retentionDays: 1,
        compressionLevel: 0, // The TAR is already compressed.
      });
      if (!result.id) {
        throw new ArtifactValidationError('GitHub did not publish the build-output artifact.');
      }
    } finally {
      await rm(manifest, { force: true });
    }
    return;
  }
  if (operation !== 'download') {
    throw new ArtifactValidationError('Expected upload or download.');
  }

  const temporary = await mkdtemp(path.join(repositoryRoot, '.pipeline-artifact-'));
  try {
    const { artifact } = await client.getArtifact(name);
    const result = await client.downloadArtifact(artifact.id, { path: temporary, expectedHash: artifact.digest });
    if (result.digestMismatch) {
      throw new ArtifactValidationError('GitHub build-output artifact digest mismatch.');
    }
    const downloaded = path.join(temporary, archiveName);
    const expected = JSON.parse(await readFile(path.join(temporary, manifestName), 'utf8'));
    const actual = await describe(downloaded);
    if (actual.size !== expected.size || actual.sha256 !== expected.sha256) {
      throw new ArtifactValidationError('Incomplete or corrupt build-output artifact.');
    }
    // Validate before exposing the archive to TAR extraction.
    await rename(downloaded, archive);
  } finally {
    await rm(temporary, { recursive: true, force: true });
  }
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  try {
    const { DefaultArtifactClient } = await import('@actions/artifact');
    await transfer(new DefaultArtifactClient(), ...process.argv.slice(2));
  } catch (error) {
    // SDK exceptions may include signed storage URLs. Keep credentials out of logs.
    console.error(error instanceof ArtifactValidationError ? error.message : 'GitHub build-output artifact transfer failed.');
    process.exitCode = 1;
  }
}
