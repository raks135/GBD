
1)CTE

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

2)CROSS APPLY
SELECT 
       t.id,
       t.date,
       t.qty,
       c.name
FROM dbo.customer AS c
    CROSS APPLY
(
    SELECT TOP 2
           c2.id,
           c2.date,
           c2.qty
    FROM dbo.customerorder AS c2
    WHERE c2.id = c.id ORDER BY c2.date DESC
) t;