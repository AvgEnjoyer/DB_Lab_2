SELECT C.surname
FROM Persons C
WHERE C.email != Y
AND NOT EXISTS
    ((SELECT Purchases.carId
      FROM Purchases
      WHERE Purchases.personId = C.id)
     EXCEPT
     (SELECT Purchases.carId
      FROM Purchases
      WHERE Purchases.personId IN
          (SELECT Persons.id
           FROM Persons
           WHERE Persons.email = Y)))
AND NOT EXISTS
    ((SELECT Purchases.carId
      FROM Purchases
      WHERE Purchases.personId IN
  	      (SELECT Persons.id
           FROM Persons
           WHERE Persons.email = Y))
     EXCEPT
     (SELECT Purchases.carId
      FROM Purchases
      WHERE Purchases.personId = C.id));