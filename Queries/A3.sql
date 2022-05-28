SELECT C.surname, C.email
FROM Persons C
WHERE C.name = Y
AND NOT EXISTS
	((SELECT Purchases.carId
	  FROM Purchases
	  WHERE Purchases.carId = C.id)
	 EXCEPT
	 (SELECT Cars.id
	  FROM Cars))
AND NOT EXISTS
	((SELECT Cars.id
	  FROM Cars)
	 EXCEPT
	 (SELECT Purchases.carId
	  FROM Purchases
	  WHERE Purchases.personId = C.id));