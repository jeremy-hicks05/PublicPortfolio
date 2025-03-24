const PROXY_CONFIG = [
  {
    context: [
      /*"/weatherforecast",*/
      /*"/ticket",*/
      "/api",
    ],
    //target: "https://localhost:50443",
    target: "https://mtadev",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
