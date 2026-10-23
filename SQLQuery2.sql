
DELETE FROM dbo.Gastos;

DBCC CHECKIDENT ('dbo.Gastos', RESEED, 0);

INSERT INTO dbo.Gastos (Descripcion, Monto, Categoria)
VALUES
('Compra de alimentos', 45.50, 'Comida'),
('Gasolina', 250.00, 'Transporte'),
('Pago de electricidad', 75.00, 'Servicios');
