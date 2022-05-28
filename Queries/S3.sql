SELECT Cars.name, Cars.price
FROM Cars
WHERE Cars.brandId IN
	(SELECT Brands.id
	 FROM Brands
	 WHERE Brands.countryId IN
	 	(SELECT Countries.id
		 FROM Countries
		 WHERE Countries.name = K));