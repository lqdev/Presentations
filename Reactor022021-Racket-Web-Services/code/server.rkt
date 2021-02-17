#lang racket
(require json)
(require web-server/servlet)
(require web-server/servlet-env)

;;; Define utility functions

;;; Convert raw request body to jsexrp
(define (parse-json-body req)
    (bytes->jsexpr (request-post-data/raw req)))

;;; Function to get value from hash given a key
(define (get-hash-value h v)
    (hash-ref h v))

;;; Define PORT
(define PORT (string->number (getenv "FUNCTIONS_HTTPWORKER_PORT")))

;;; Define handlers
(define (get-values req)
    (response/jsexpr 
            (hasheq 
                'values '(1 2 3))))

(define (post-values req)
    (define get-property 
        (curry get-hash-value (parse-json-body req)))

    (define x (string->number (get-property 'x)))
    (define y (string->number (get-property 'y)))
    
    (response/jsexpr
        (hasheq 'sum (+ x y))))

;;; Define router
(define-values (dispatch req)
    (dispatch-rules
        [("values") #:method "get" get-values]
        [("values") #:method "post" post-values]
        [else (error "Route does not exist")]))

;; Define and start server
(serve/servlet
    (lambda (req) (dispatch req))
    #:launch-browser? #f
    #:quit? #f
    #:port PORT
    #:servlet-path "/"
    #:servlet-regexp #rx"")
