SELECT Persons.name, Persons.surname
FROM Persons
WHERE Persons.id IN
	(SELECT Purchases.personId
	 FROM Purchases
	 WHERE Purchases.carId IN
	 	(SELECT Cars.id
		 FROM Cars
		 WHERE Cars.brandId IN
		 	(SELECT Brands.id
			 FROM Brands
			 WHERE Brands.name = X)));