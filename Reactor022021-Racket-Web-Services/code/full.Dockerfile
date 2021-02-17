# 1. Build Stage
#https://hub.docker.com/r/racket/racket-ci
FROM racket/racket:8.0-full AS builder
WORKDIR /app
COPY . /app
RUN raco exe server.rkt
ENV "FUNCTIONS_HTTPWORKER_PORT" 7071
ENTRYPOINT [ "./server" ]