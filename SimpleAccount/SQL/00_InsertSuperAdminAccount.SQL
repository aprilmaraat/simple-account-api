IF NOT EXISTS (SELECT * FROM [User] WHERE Email = 'aprilmaraat@gmail.com')
BEGIN
	INSERT INTO [User] ([FirstName], [LastName], [Email], [Password])
	VALUES ('AJ', 'Maraat', 'aprilmaraat@gmail.com', 'pass123456')
END