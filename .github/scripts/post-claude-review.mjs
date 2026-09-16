import { spawnSync } from 'node:child_process';
import { pathToFileURL } from 'node:url';

export function buildReview(rawReview, headSha) {
  if (!/^[a-f0-9]{40}$/.test(headSha ?? '')) {
    throw new Error('A captured pull request head SHA is required.');
  }

  let review;
  try {
    review = JSON.parse(rawReview);
  } catch {
    throw new Error('Claude did not return a valid structured review.');
  }

  const validText = value => typeof value === 'string' && value.trim().length > 0;
  if (!review || !validText(review.summary) || !Array.isArray(review.findings)
    || !review.findings.every(validText)) {
    throw new Error('The review requires a summary and an array of nonempty findings.');
  }

  const sections = [review.summary.trim(), ...review.findings.map(finding => finding.trim())];
  if (sections.some(section => /REVIEW_VERDICT\s*:/i.test(section))) {
    throw new Error('Verdict markers must be added by the trusted publisher.');
  }

  const verdict = review.findings.length === 0 ? 'CLEAR' : 'BLOCKING';
  const body = [
    '## Claude Review',
    sections[0],
    review.findings.length === 0 ? 'No actionable findings.' : sections.slice(1).join('\n\n'),
    `<!-- REVIEW_VERDICT: ${verdict} HEAD: ${headSha} -->`,
  ].join('\n\n');
  if (Buffer.byteLength(body, 'utf8') > 60000) {
    throw new Error('The review exceeds the comment size budget.');
  }
  return body;
}

function runGitHub(args, input) {
  const result = spawnSync('gh', args, { input, encoding: 'utf8', shell: false });
  if (result.error || result.status !== 0) {
    throw new Error('GitHub review publication request failed.');
  }
  return result.stdout;
}

export function publishReview({ rawReview, headSha, prNumber, repository }, run = runGitHub) {
  if (!/^[1-9]\d*$/.test(prNumber ?? '')
    || !/^[A-Za-z0-9_.-]+\/[A-Za-z0-9_.-]+$/.test(repository ?? '')) {
    throw new Error('The workflow must supply a valid pull request and repository.');
  }
  const body = buildReview(rawReview, headSha);
  const verifyHead = () => {
    const current = JSON.parse(run([
      'pr', 'view', prNumber, '--repo', repository, '--json', 'headRefOid,state',
    ]));
    if (current.state !== 'OPEN' || current.headRefOid !== headSha) {
      throw new Error('The pull request is closed or its head changed during review.');
    }
  };
  verifyHead();
  // Keep model output out of shell commands and command-line arguments.
  run(['pr', 'comment', prNumber, '--repo', repository, '--body-file', '-'], body);
  verifyHead();
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  try {
    publishReview({
      rawReview: process.env.REVIEW_JSON,
      headSha: process.env.REVIEW_HEAD_SHA,
      prNumber: process.env.PR_NUMBER,
      repository: process.env.GH_REPO,
    });
  } catch (error) {
    console.error(error.message);
    process.exitCode = 1;
  }
}
