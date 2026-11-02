USE MMABooksFramework;

CALL usp_CustomerSelect(1);
CALL usp_CustomerSelectAll();

CALL usp_CustomerCreate(@newId, 'Test, Steph', '123 Main St', 'Portland', 'OR', '97201');
SELECT @newId;

CALL usp_CustomerUpdate(@newId, 'Test, Steph Updated', '123 Main St', 'Portland', 'OR', '97201', 1);
CALL usp_CustomerSelect(@newId);

CALL usp_CustomerDelete(@newId, 2);
CALL usp_CustomerSelect(@newId);