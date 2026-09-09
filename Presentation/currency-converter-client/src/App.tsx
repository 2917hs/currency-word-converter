import { useEffect, useState } from 'react'
import './App.css'

const API_BASE_URL = 'http://localhost:5295'

type LanguageOption = {
  code: string
  displayName: string
}

type ConvertCurrencyResponse = {
  amount: string
  language: string
  words: string
}

type ValidationProblemDetails = {
  title: string
  errors: Record<string, string[]>
}

function App() {
  const [languages, setLanguages] = useState<LanguageOption[]>([])
  const [language, setLanguage] = useState('en')
  const [amount, setAmount] = useState('')
  const [result, setResult] = useState('')
  const [error, setError] = useState('')
  const [validationError, setValidationError] = useState('')

  useEffect(() => {
    fetch(`${API_BASE_URL}/api/languages`)
      .then((response) => response.json())
      .then((data: LanguageOption[]) => setLanguages(data))
      .catch(() => setError('Could not load languages from the server.'))
  }, [])

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setError('')
    setResult('')

    const validationMessage = validateAmount(amount)
    if (validationMessage) {
      setValidationError(validationMessage)
      return
    }
    setValidationError('')

    const response = await fetch(`${API_BASE_URL}/api/currency/convert`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ amount, language }),
    })

    if (!response.ok) {
      const problem: ValidationProblemDetails = await response.json()
      const messages = Object.values(problem.errors).flat()
      setError(messages.join(' '))
      return
    }

    const data: ConvertCurrencyResponse = await response.json()
    setResult(data.words)
  }

  function validateAmount(value: string): string | null {
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

  return (
    <main>
      <h1>Currency to Words Converter</h1>

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="language">Language</label>
          <select
            id="language"
            value={language}
            onChange={(event) => setLanguage(event.target.value)}
          >
            {languages.map((option) => (
              <option key={option.code} value={option.code}>
                {option.displayName}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label htmlFor="amount">Amount</label>
          <input
            id="amount"
            type="text"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            placeholder="e.g. 25,10"
          />
          {validationError && <p style={{ color: 'red' }}>{validationError}</p>}
        </div>

        <button type="submit">Convert</button>
      </form>

      {result && <p>{result}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </main>
  )
}

export default App