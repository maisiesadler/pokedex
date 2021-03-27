# Pokedex

## Design decisions

If this were going to production
- Depending on requirements, I'd make the language chosen for the description configurable either for the application or per request
- Replace file cache with something like Redis
  - Use Elasticache for production
  - Create a docker-compose than spins up an instance of redis for local development
