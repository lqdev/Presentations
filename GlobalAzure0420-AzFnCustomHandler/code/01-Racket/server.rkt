;; Define language and import packages
#lang racket
(require json)
(require web-server/servlet)
(require web-server/servlet-env)

;; Get port where server listens on
(define PORT (string->number (getenv "FUNCTIONS_HTTPWORKER_PORT")))

;; Create function to handle GET /values request
(define (get-values req)
    (response/full
        200
        #"OK"
        (current-seconds)
        #"application/json;charset=utf-8"
        empty
        (list (jsexpr->bytes #hasheq((value . (1 2 3)))))))

;; Define routes
(define-values (dispatch req)
    (dispatch-rules
        [("values") #:method "get" get-values]
        [else (error "Route does not exist")]))

;; Define and start server
(serve/servlet
    (lambda (req) (dispatch req))
    #:launch-browser? #f
    #:quit? #f
    #:port PORT
    #:servlet-path "/"
    #:servlet-regexp #rx"")