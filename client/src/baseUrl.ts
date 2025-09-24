const isProduction = import.meta.env.PROD;

const prod = 'https://server-broken-dawn-7352.fly.dev'
const dev = 'http://localhost:5090'

export const finalUrl = isProduction ? prod : dev;
