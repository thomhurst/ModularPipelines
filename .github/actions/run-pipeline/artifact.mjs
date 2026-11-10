import { createReadStream } from 'node:fs';
import { mkdtemp, readFile, rename, rm, stat, writeFile } from 'node:fs/promises';
import { createHash } from 'node:crypto';
import path from 'node:path';
import { setTimeout } from 'node:timers/promises';
import { pathToFileURL } from 'node:url';
import { ArtifactNotFoundError } from '@actions/artifact';
import { ArtifactValidationError, artifactErrorMessage } from './artifact-errors.mjs';

const archiveName = '_build-output.tar.gz';
const manifestName = `${archiveName}.json`;

// Seven attempts, with at most 61 seconds of additional waiting after publication.
const visibilityDelays = [1000, 2000, 4000, 8000, 16000, 30000];

async function atStage(stage, action) {
  try {
    return await action();
  } catch (error) {
    if (error instanceof ArtifactValidationError) throw error;
    throw new ArtifactValidationError(`GitHub build-output artifact ${stage} failed.`, { cause: error });
  }
}

async function download(client, name, temporary, waitForVisibility, wait) {
  for (let attempt = 0; ; attempt++) {
    let stage = 'lookup';
    try {
      const { artifact } = await client.getArtifact(name);
      stage = 'download';
      // The SDK performs another ListArtifacts call inside downloadArtifact.
      return await client.downloadArtifact(artifact.id, { path: temporary, expectedHash: artifact.digest });
    } catch (error) {
      if (!(error instanceof ArtifactNotFoundError)) {
        throw new ArtifactValidationError(`GitHub build-output artifact ${stage} failed.`, { cause: error });
      }
      if (!waitForVisibility) {
        throw new ArtifactValidationError(`GitHub build-output artifact ${stage} failed: artifact not found.`);
      }
      if (attempt === visibilityDelays.length) {
        throw new ArtifactValidationError(`GitHub build-output artifact visibility check failed after 7 attempts during ${stage} (61 seconds of waiting).`);
      }
      console.log(`Waiting for GitHub artifact visibility after ${stage} (${attempt + 1}/6).`);
      await wait(visibilityDelays[attempt]);
    }
  }
}

async function describe(file) {
  const hash = createHash('sha256');
  for await (const chunk of createReadStream(file)) {
    hash.update(chunk);
  }
  return { size: (await stat(file)).size, sha256: hash.digest('hex') };
}

export async function transfer(client, operation, runId, repositoryRoot, { waitForVisibility = false, wait = setTimeout } = {}) {
  if (typeof runId !== 'string' || !/^[a-zA-Z0-9._-]+$/.test(runId)) {
    throw new ArtifactValidationError('A safe, attempt-scoped run ID is required.');
  }
  const name = `build-output-${runId}`;
  const archive = path.join(repositoryRoot, archiveName);
  if (operation === 'upload') {
    const manifest = path.join(repositoryRoot, manifestName);
    try {
      await atStage('archive preparation', async () => writeFile(manifest, JSON.stringify(await describe(archive))));
      const result = await atStage('publication', () => client.uploadArtifact(name, [archive, manifest], repositoryRoot, {
        retentionDays: 1,
        compressionLevel: 0, // The TAR is already compressed.
      }));
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
    const result = await download(client, name, temporary, waitForVisibility, wait);
    if (result.digestMismatch) {
      throw new ArtifactValidationError('GitHub build-output artifact digest mismatch.');
    }
    await atStage('archive verification', async () => {
      const downloaded = path.join(temporary, archiveName);
      const expected = JSON.parse(await readFile(path.join(temporary, manifestName), 'utf8'));
      const actual = await describe(downloaded);
      if (actual.size !== expected.size || actual.sha256 !== expected.sha256) {
        throw new ArtifactValidationError('Incomplete or corrupt build-output artifact.');
      }
      // Validate before exposing the archive to TAR extraction.
      await rename(downloaded, archive);
    });
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
    console.error(artifactErrorMessage(error));
    process.exitCode = 1;
  }
}
