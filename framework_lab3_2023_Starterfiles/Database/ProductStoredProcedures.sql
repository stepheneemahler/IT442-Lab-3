USE MMABooksFramework;

-- Query 1: Create a product
DELIMITER //
CREATE PROCEDURE usp_ProductCreate (out prodId int, in code_p varchar(10), in description_p varchar(50), in unitprice_p decimal(10,4), in onhand_p int)
BEGIN
	Insert into products (productcode, description, unitprice, onhandquantity, concurrencyid)
	Values (code_p, description_p, unitprice_p, onhand_p, 1);
	Select LAST_INSERT_ID() into prodId;
END //
DELIMITER ;

-- Query 2: Select one product
DELIMITER //
CREATE PROCEDURE usp_ProductSelect (in prodId int)
BEGIN
	Select * from products where productid = prodId;
END //
DELIMITER ;

-- Query 3: Select all products
DELIMITER //
CREATE PROCEDURE usp_ProductSelectAll ()
BEGIN
	Select * from products order by productcode;
END //
DELIMITER ;

-- Query 4: Update a product
DELIMITER //
CREATE PROCEDURE usp_ProductUpdate (in prodId int, in code_p varchar(10), in description_p varchar(50), in unitprice_p decimal(10,4), in onhand_p int, in conCurrId int)
BEGIN
	Update products
	Set productcode = code_p, description = description_p, unitprice = unitprice_p, onhandquantity = onhand_p,
	    concurrencyid = (concurrencyid + 1)
	Where productid = prodId and concurrencyid = conCurrId;
END //
DELIMITER ;

-- Query 5: Delete a product
DELIMITER //
CREATE PROCEDURE usp_ProductDelete (in prodId int, in conCurrId int)
BEGIN
	Delete from products where productid = prodId and concurrencyid = conCurrId;
END //
DELIMITER ;