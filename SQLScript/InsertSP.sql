USE [GBD]
GO
/****** Object:  StoredProcedure [dbo].[Sp_ProductDataIns]    Script Date: 02/21/2021 2:36:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Sp_ProductDataIns]
(@jsonData NVARCHAR(MAX))
AS
BEGIN

    BEGIN TRY
        BEGIN TRANSACTION;

        SET NOCOUNT ON;
        CREATE TABLE #tmp
        (
            ProductId INT,
            [Name] [NVARCHAR](50),
            [TotalPrice] [DECIMAL](18, 2),
            [Discount] [DECIMAL](3, 1),
            [Rating] NVARCHAR(100),
            Detail NVARCHAR(MAX),
            Price CHAR(10),
            Stock INT,
            ReviewId INT,
            Customer NVARCHAR(50),
            Body NVARCHAR(MAX),
            Stars INT
        );
        INSERT INTO #tmp
        (
            ProductId,
            Name,
            TotalPrice,
            Discount,
            Rating,
            Detail,
            Price,
            Stock,
            ReviewId,
            Customer,
            Body,
            Stars
        )
        SELECT DISTINCT
               CONVERT(INT, RIGHT(t.Id, CHARINDEX('/', REVERSE(t.Id)) - 1)) AS ProductId,
               t.Name,
               CONVERT(DECIMAL(18, 2), t.TotalPrice),
               CONVERT(DECIMAL(3, 1), t.Discount),
               CONVERT(INT, IIF(ISNUMERIC(t.Rating) = 1, t.Rating, 0)),
               pd.Detail,
               pd.Price,
               CONVERT(INT, IIF(ISNUMERIC(pd.Stock) = 1, pd.Stock, 0)),
               r.Id,
               r.Customer,
               r.Body,
               r.Stars
        FROM
            OPENJSON(@jsonData)
            WITH
            (
                Id NVARCHAR(100) '$.Href.Link',
                Name NVARCHAR(50) '$.Name',
                TotalPrice DECIMAL '$.TotalPrice',
                Discount DECIMAL '$.Discount',
                Rating NVARCHAR(100) '$.Rating',
                ProductDetail NVARCHAR(MAX) '$.ProductDetail' AS JSON
            ) AS t
            OUTER  APPLY
            OPENJSON(ProductDetail)
            WITH
            (
                Name NVARCHAR(50) '$.Name',
                Detail NVARCHAR(MAX) '$.Detail',
                Price CHAR(10) '$.Price',
                Stock NVARCHAR(50) '$.Stock',
                TotalPrice DECIMAL '$.TotalPrice',
                Rating NVARCHAR(100) '$.Rating',
                Discount DECIMAL '$.Discount',
                Reviews NVARCHAR(MAX) '$.Reviews' AS JSON
            ) AS pd
            OUTER  APPLY
            OPENJSON(Reviews)
            WITH
            (
                Id INT '$.Id',
                Customer NVARCHAR(50) '$.Customer',
                Body NVARCHAR(MAX) '$.Body',
                Stars INT '$.Stars'
            ) AS r;
        INSERT INTO dbo.Product
        (
            Id,
            Name,
            TotalPrice,
            Discount,
            Rating,
            InsertedDate,
            UpdatedDate,
            InsertedBy
        )
        SELECT DISTINCT
               t.ProductId,
               t.Name,
               t.TotalPrice,
               t.Discount,
               CONVERT(INT, t.Rating),
               GETDATE(),
               GETDATE(),
               '1'
        FROM #tmp AS t
            LEFT JOIN dbo.Product AS p
                ON p.Id = t.ProductId
        WHERE p.Id IS NULL;

        UPDATE p
        SET p.Name = t.Name,
            p.TotalPrice = t.TotalPrice,
            p.Discount = t.Discount,
            p.Rating = t.Rating,
            p.UpdatedDate = GETDATE()
        FROM dbo.Product AS p
            LEFT JOIN #tmp AS t
                ON p.Id = t.ProductId
        WHERE p.Id IS NOT NULL;


        INSERT INTO dbo.ProductDetail
        (
            ProductId,
            Name,
            Detail,
            Price,
            Stock,
            Discount,
            Rating,
            TotalPrice,
            InsertDate,
            InsertedBy
        )
        SELECT DISTINCT
               t.ProductId,
               t.Name,
               t.Detail,
               t.Price,
               t.Stock,
               t.Discount,
               t.Rating,
               t.TotalPrice,
               GETDATE(),
               '1'
        FROM #tmp AS t;

        INSERT INTO dbo.Review
        (
            Id,
            Customer,
            Body,
            Stars,
            ProductId,
            InsertedDate,
            InsertedBy,
            UpdatedDate
        )
        SELECT DISTINCT
               t.ReviewId,
               t.Customer,
               t.Body,
               t.Stars,
               t.ProductId,
               GETDATE(),
               '1',
               GETDATE()
        FROM #tmp AS t
            LEFT JOIN dbo.Review AS r
                ON r.Id = t.ReviewId
        WHERE Id IS NULL
              AND t.ReviewId > 0;
        UPDATE r
        SET r.Body = t.Body,
            r.Stars = t.Stars,
            r.UpdatedDate = GETDATE()
        FROM dbo.Review AS r
            LEFT JOIN #tmp AS t
                ON r.Id = t.ReviewId
        WHERE Id IS NOT NULL;

        DROP TABLE #tmp;
        COMMIT TRANSACTION;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH;

END;
