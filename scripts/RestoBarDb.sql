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

-- 3. CREAR TABLA Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- 4. CREAR TABLA Mesas
CREATE TABLE Mesas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero INT NOT NULL UNIQUE,
    Ocupada BIT NOT NULL DEFAULT 0,
    MeseroId INT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    Estado NVARCHAR(50) NULL
);

-- Crear índices para mejor performance
CREATE INDEX IX_Mesas_MeseroId ON Mesas(MeseroId);
CREATE INDEX IX_Mesas_Ocupada ON Mesas(Ocupada);
GO

-- 5. CREAR TABLA Insumos
CREATE TABLE Insumos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    Tipo NVARCHAR(50) NOT NULL,
    Activo BIT NOT NULL DEFAULT 1
);
GO

-- 5.2. CREAR TABLA Pedidos
CREATE TABLE Pedidos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MesaId INT NOT NULL FOREIGN KEY REFERENCES Mesas(Id),
    MeseroId INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    FechaHora DATETIME NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    Total DECIMAL(18,2) NOT NULL DEFAULT 0.00
);
GO

-- 5.3. CREAR TABLA Comandas
CREATE TABLE Comandas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL FOREIGN KEY REFERENCES Pedidos(Id),
    Estado NVARCHAR(50) NOT NULL,
    FechaHora DATETIME NOT NULL,
    Observaciones NVARCHAR(500) NULL
);
GO

-- 5.4. CREAR TABLA DetallesPedidos
CREATE TABLE DetallesPedidos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ComandaId INT NOT NULL FOREIGN KEY REFERENCES Comandas(Id),
    InsumoId INT NOT NULL FOREIGN KEY REFERENCES Insumos(Id),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL
);
GO

-- 6. INSERTAR DATOS DE PRUEBA

-- Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, Password, Rol, Activo) VALUES
('Juan', 'Perez', 'juan.perez@restobar.com', 'admin123', 'Gerente', 1),
('Martin', 'Gomez', 'martin.gomez@restobar.com', 'mesero123', 'Mesero', 1),
('Ana', 'Lopez', 'ana.lopez@restobar.com', 'mesero123', 'Mesero', 1),
('Carlos', 'Rodriguez', 'carlos.rod@restobar.com', 'chef123', 'Chef', 1);

-- Mesas
INSERT INTO Mesas (Numero, Ocupada, MeseroId, Estado) VALUES 
(1, 0, NULL, 'Libre'),
(2, 1, 2, 'Ocupada'),
(3, 0, NULL, 'Libre'),
(4, 1, 3, 'Ocupada'),
(5, 0, NULL, 'Libre'),
(6, 1, 2, 'Ocupada'),
(7, 0, NULL, 'Libre'),
(8, 1, 3, 'Ocupada');

-- Insumos
INSERT INTO Insumos (Nombre, Precio, Stock, Tipo, Activo) VALUES
('Coca Cola 500ml', 1500.00, 50, 'Bebida', 1),
('Agua Mineral 500ml', 1200.00, 40, 'Bebida', 1),
('Cerveza Quilmes 1L', 2500.00, 30, 'Bebida', 1),
('Milanesa con Papas Fritas', 6500.00, 20, 'Plato', 1),
('Hamburguesa Completa', 5800.00, 25, 'Plato', 1),
('Flan con Dulce de Leche', 2000.00, 15, 'Plato', 1);
GO

-- 7. VERIFICAR DATOS INSERTADOS
SELECT 'Tablas creadas y pobladas correctamente' AS [Estado];
SELECT * FROM Usuarios;
SELECT * FROM Mesas;
SELECT * FROM Insumos;
SELECT * FROM Pedidos;
SELECT * FROM Comandas;
SELECT * FROM DetallesPedidos;
GO
