/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{ts,html}"
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: "#2563eb",
          foreground: "#ffffff"
        },
        slate: {
          950: "#020617"
        }
      },
      fontFamily: {
        sans: ["'Segoe UI'", "Inter", "system-ui", "sans-serif"]
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
}
