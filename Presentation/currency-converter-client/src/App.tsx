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

function App() {
  const [languages, setLanguages] = useState<LanguageOption[]>([])
  const [language, setLanguage] = useState('en')
  const [amount, setAmount] = useState('')
  const [result, setResult] = useState('')
  const [error, setError] = useState('')

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

    const response = await fetch(`${API_BASE_URL}/api/currency/convert`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ amount, language }),
    })

    if (!response.ok) {
      setError(`Request failed: ${response.status}`)
      return
    }

    const data: ConvertCurrencyResponse = await response.json()
    setResult(data.words)
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
        </div>

        <button type="submit">Convert</button>
      </form>

      {result && <p>{result}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}
    </main>
  )
}

export default App