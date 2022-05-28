SELECT Brands.name
FROM Brands
WHERE Brands.id IN
	(SELECT Cars.brandId
	 FROM Cars
	 WHERE Cars.id IN
	 	(SELECT Purchases.CarId
		 FROM Purchases
		 WHERE Purchases.personId IN
		 	(SELECT Persons.id
			 FROM Persons
			 WHERE Persons.name = X AND Persons.surname= Y
			 AND Persons.email = Z)));