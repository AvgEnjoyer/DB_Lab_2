SELECT Brands.name
FROM Brands
WHERE Brands.id IN
	(SELECT Cars.brandId
	 FROM Cars
	 WHERE Cars.price != P);