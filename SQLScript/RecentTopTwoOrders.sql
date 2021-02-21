WITH cte
AS (SELECT  c.id,
           c.Name  
           co.Date,
           co.Qty,
           ROW_NUMBER() OVER (PARTITION BY 
		  c.id
                              ORDER BY c.id,co.Date DESC
                                       
                             ) AS rn
    FROM dbo.Customers AS c INNER JOIN  dbo.CustomerOrder AS co ON co.id = c.id)
	SELECT cte.id,
           cte.Name,
           cte.Date,
           cte.Qty
            FROM cte WHERE rn<3
