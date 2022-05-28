SELECT Countries.name
FROM Countries
WHERE Countries.id IN
	(SELECT Brands.countryId
	 FROM Brands
	 WHERE Brands.id IN
		(SELECT D.id
		 FROM Brands D
		 WHERE NOT EXISTS
	 		((SELECT Cars.price
			  FROM Cars
		      WHERE Cars.brandId = K)
		     EXCEPT
		     (SELECT Cars.price
		      FROM Cars
		      WHERE Cars.BrandId = D.id AND Cars.brandId != K))));