USE MMABooksFramework;

CALL usp_testingResetProductData();

CALL usp_ProductSelect(1);
CALL usp_ProductSelectAll();

CALL usp_ProductCreate(@newProdId, 'TEST', 'Test Product', 19.99, 10);
SELECT @newProdId;

CALL usp_ProductUpdate(@newProdId, 'TEST', 'Test Product Updated', 24.99, 15, 1);
CALL usp_ProductSelect(@newProdId);

CALL usp_ProductDelete(@newProdId, 2);
CALL usp_ProductSelect(@newProdId);