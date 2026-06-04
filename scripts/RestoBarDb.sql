-- ============================================
-- Base de Datos: RestoBarDb
-- Proyecto: Resto Bar - TPI Programación III
-- ============================================

-- 1. CREAR LA BASE DE DATOS
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'RestoBarDb')
BEGIN
    DROP DATABASE RestoBarDb;
END
GO

CREATE DATABASE RestoBarDb;
GO

-- 2. USAR LA BASE DE DATOS
USE RestoBarDb;
GO

-- 3. CREAR TABLA Mesas
CREATE TABLE Mesas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero INT NOT NULL UNIQUE,
    Ocupada BIT NOT NULL DEFAULT 0,
    MeseroId INT NULL,
    Estado NVARCHAR(50) NULL
);

-- Crear índices para mejor performance
CREATE INDEX IX_Mesas_MeseroId ON Mesas(MeseroId);
CREATE INDEX IX_Mesas_Ocupada ON Mesas(Ocupada);
GO

-- 4. INSERTAR DATOS DE PRUEBA
INSERT INTO Mesas (Numero, Ocupada, MeseroId, Estado) VALUES 
(1, 0, NULL, 'Libre'),
(2, 1, 1, 'Ocupada'),
(3, 0, NULL, 'Libre'),
(4, 1, 2, 'Ocupada'),
(5, 0, NULL, 'Libre'),
(6, 1, 3, 'Ocupada'),
(7, 0, NULL, 'Libre'),
(8, 1, 1, 'Ocupada');

GO

-- 5. VERIFICAR DATOS INSERTADOS
SELECT 'Mesas creadas y pobladas correctamente' AS [Estado];
SELECT * FROM Mesas;
GO
