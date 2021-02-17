#lang racket

;;; This is a comment


;;; Literals
(println "Literals")

1
"Hello World"

;;; Assign / Store values

(printf "~n")
(println "Assign / Store Values")
(define x 3)
(println x)
(printf "The values of x is ~v ~n" x)

;;; Operations

(printf "~n")
(println "Operations")
(+ 1 1)
(/ 8 2)
(+ x 2)

(define four
  (+ 3 1))

;;; Functions
(printf "~n")
(println "Functions")

;;; Define function
(define (double n)
  (* n 2))

;;; Call function
(double 3)

;;;Define function
(define (make-animal-sound animal sound)
  (printf "The ~a says ~a ~n" animal sound))

;;; Partially apply function
(define dog (curry make-animal-sound "dog"))
(define cat (curry make-animal-sound "cat"))

;;; Partially apply
(dog "woof")
(cat "meow")

;;; Collections
(printf "~n")
(println "Collections")

;;; Pairs
(cons 1 2) ;;; Pairs
(define myPair '(1 2))
(car myPair)
(cdr myPair)

;;; List
'(1 2 3 4)
(define list1 (list 1 2 3 4))

;;; List operations
(length list1)
(map double list1)
(filter
 (lambda (n)
   (= 0 (modulo n 2)))
 list1)

;;; Structs
(printf "~n")
(println "Structs")
(struct point (x y))
(define myPoint (point 1 2))
(printf "x: ~a, y: ~a ~n" (point-x myPoint) (point-y myPoint))