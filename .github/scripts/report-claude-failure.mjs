import { readFileSync } from 'node:fs';
import { pathToFileURL } from 'node:url';

export function classifyFailure(rawExecution) {
  let messages;
  try {
    messages = JSON.parse(rawExecution);
  } catch {
    return 'unavailable execution';
  }
  const result = Array.isArray(messages) ? messages.at(-1) : undefined;
  if (result?.type !== 'result' || result.is_error !== true) {
    return 'unclassified failure';
  }

  // Inspect only the failed terminal result. Return fixed categories, never
  // error text, transcript content, account details, or credentials.
  const detail = [result.result, ...(Array.isArray(result.errors) ? result.errors : [])]
    .filter(value => typeof value === 'string').join('\n');
  // Prefer the SDK's numeric API status. Older CLI versions embed the status
  // in "API Error: NNN" instead. Both paths map to a fixed allowlist.
  const status = result.api_error_status ?? Number(detail.match(/\bAPI Error:\s*(\d{3})\b/i)?.[1]);
  const statusCategories = new Map([
    [400, 'invalid API request'], [401, 'authentication failure'],
    [402, 'billing failure'], [403, 'permission failure'], [404, 'API resource not found'],
    [409, 'API resource conflict'], [413, 'request too large'], [429, 'rate or usage limit'],
    [500, 'service or connection failure'], [502, 'service or connection failure'],
    [503, 'service or connection failure'], [504, 'service or connection failure'],
    [529, 'service or connection failure'],
  ]);
  if (statusCategories.has(status)) {
    return statusCategories.get(status);
  }
  if (/rate.?limit|usage.?limit|hit your limit|insufficient_quota|credit balance/i.test(detail)) {
    return 'rate or usage limit';
  }
  if (/authentication_error|unauthorized|not logged in|invalid.{0,20}(?:token|api.key)|(?:token|oauth).{0,20}(?:expired|invalid)/i.test(detail)) {
    return 'authentication failure';
  }
  if (/overloaded_error|service unavailable|connection (?:error|refused|reset)|ECONNRESET|ETIMEDOUT/i.test(detail)) {
    return 'service or connection failure';
  }
  return 'unclassified model error';
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  let category = 'unavailable execution';
  try {
    category = classifyFailure(readFileSync(process.env.REVIEW_EXECUTION_FILE, 'utf8'));
  } catch {
    // The action can fail before writing its execution file.
  }
  console.log(`Claude execution failure category: ${category}.`);
}
