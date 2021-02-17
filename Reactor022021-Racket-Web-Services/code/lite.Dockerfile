# 1. Build Stage
#https://hub.docker.com/r/racket/racket-ci
FROM racket/racket:8.0-full AS builder
WORKDIR /app
COPY . /app
RUN raco exe server.rkt


# 2. Deployment
FROM racket/racket:8.0
WORKDIR /home
COPY --from=builder /app/server .
ENV "FUNCTIONS_HTTPWORKER_PORT" 7071
ENTRYPOINT [ "./server" ]