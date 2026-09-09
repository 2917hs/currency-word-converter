export function validateAmount(value: string): string | null {
  const trimmed = value.trim()
  if (trimmed === '') return 'Amount is required.'

  const [dollarsPart, centsPart, ...rest] = trimmed.split(',')
  if (rest.length > 0) return "Amount must contain at most one ',' separator."

  const dollarsDigitsOnly = dollarsPart.replace(/\s/g, '')
  if (!/^\d+$/.test(dollarsDigitsOnly)) {
    return 'Dollars must contain only digits (and spaces).'
  }
  if (Number(dollarsDigitsOnly) > 999_999_999) {
    return 'The maximum supported amount is 999,999,999 dollars.'
  }

  if (centsPart !== undefined) {
    if (!/^\d{1,2}$/.test(centsPart)) {
      return "Cents must be one or two digits after the ',' (e.g. '25,1' or '25,10')."
    }
  }

  return null
}
