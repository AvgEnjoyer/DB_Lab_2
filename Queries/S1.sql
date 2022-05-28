SELECT AVG(Cars.price)
FROM Cars
WHERE Cars.brandId IN
	(SELECT Brands.id
	 FROM Brands
	 WHERE Brands.name = P);