USE MMABooksFramework;

-- Query 1: Select one customer
DELIMITER //
CREATE PROCEDURE usp_CustomerSelect (in custId int)
BEGIN
	Select * from customers where customerid = custId;
END //
DELIMITER ;

-- Query 2: Select all customers
DELIMITER //
CREATE PROCEDURE usp_CustomerSelectAll ()
BEGIN
	Select * from customers order by name;
END //
DELIMITER ;

-- Query 3: Update a customer
DELIMITER //
CREATE PROCEDURE usp_CustomerUpdate (in custId int, in name_p varchar(100), in address_p varchar(50), in city_p varchar(20), in state_p varchar(2), in zipcode_p varchar(15), in conCurrId int)
BEGIN
	Update customers
	Set name = name_p, address = address_p, city = city_p, state = state_p, zipcode = zipcode_p,
	    concurrencyid = (concurrencyid + 1)
	Where customerid = custId and concurrencyid = conCurrId;
END //
DELIMITER ;

-- Query 4: Delete a customer
DELIMITER //
CREATE PROCEDURE usp_CustomerDelete (in custId int, in conCurrId int)
BEGIN
	Delete from customers where customerid = custId and concurrencyid = conCurrId;
END //
DELIMITER ;