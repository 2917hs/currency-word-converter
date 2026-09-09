import { useState } from 'react'
import './App.css'

function App() {
  const [language, setLanguage] = useState('en')
  const [amount, setAmount] = useState('')
  const [result, setResult] = useState('')

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()
    setResult(`You entered "${amount}" in language "${language}"`)
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
            <option value="en">English</option>
            <option value="de">German</option>
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
    </main>
  )
}

export default App