# Pokedex

[![Build and Test](https://github.com/maisie-tests/pokedex/actions/workflows/buildtest.yml/badge.svg)](https://github.com/maisie-tests/pokedex/actions/workflows/buildtest.yml)

## Getting Started

Prerequisites
- [dotnet 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

Clone the repo

`dotnet run -p src/Pokedex`

Then navigate to `http://localhost:5000/swagger/index.html` to use the swagger docs.

To run tests

`dotnet test`

### Using Docker

Prerequisites
- [Docker desktop](https://www.docker.com/products/docker-desktop)

`docker build -t pokedex .`

`docker run -p 8080:80 pokedex`

Then navigate to `http://localhost:8080/swagger/index.html` to use the swagger docs.

## Design decisions

The application is designed using Hexagonal Architecture principles.
The business logic is inside the `/Domain` folder, the calling code or input ports is in `/Controllers` and the queries or output ports are in `Queries`. The Domain code defines the interfaces it requires and the Controllers/Queries reference the domain code, not the other way around.

I added a caching layer, using the file system to keep it simple.

If this were going to production
- Depending on requirements, I'd make the language chosen for the description configurable either for the application or per request
- Replace file cache with something like Redis
  - Use Elasticache for production
  - Create a docker-compose than spins up an instance of redis for local development
- There's not much logging and there is no tracing, this would be something I would want to improve before going to production
- I removed HttpsRedirection, if this was in production it would be behind a load balancer that would do SSL termination so it wouldn't be needed here
- I'd add circuit breaking in to the two api's, once we detect we are being rate limited there is no point in calling the api
- Create a mock api that returns a few of the responses for local development and testing
- Api urls should be configurable so mock api's can be used in test environments/local development
  - Assuming the paid account would be used in production would need to have api-key's and a way to configure those 
