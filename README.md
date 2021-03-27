# Pokedex

## Getting Started

`dotnet run -p src/Pokedex`

Then navigate to `http://localhost:5000/swagger/index.html` to use the swagger docs.

### Using Docker

`docker build -t pokedex .`

`docker run -p 8080:80`

Then navigate to `http://localhost:8080/swagger/index.html` to use the swagger docs.

### To run tests

`dotnet test`

## Design decisions

If this were going to production
- Depending on requirements, I'd make the language chosen for the description configurable either for the application or per request
- Replace file cache with something like Redis
  - Use Elasticache for production
  - Create a docker-compose than spins up an instance of redis for local development
- There's not much logging and there is no tracing, this would be something I would want to improve before going to production
- I removed HttpsRedirection, if this was in production it would be behind a load balancer that would do SSL termination so it wouldn't be needed here
