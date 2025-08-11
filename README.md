# Proyecto_PrograWebAvanzada_Grupo8

-- Script para crear la base de datos EventosCostaRica
CREATE DATABASE EventosCostaRica;
USE EventosCostaRica;

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Administrador', 'Cliente')),
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- Tabla de Eventos
CREATE TABLE Eventos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Descripcion NVARCHAR(MAX),
    FechaEvento DATETIME2 NOT NULL,
    Lugar NVARCHAR(200) NOT NULL,
    Banner NVARCHAR(500),
    CapacidadTotal INT DEFAULT 100,
    PrecioEntrada DECIMAL(10,2) NOT NULL,
    Vendido BIT DEFAULT 0,
    FechaCreacion DATETIME2 DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- Tabla de Asientos
CREATE TABLE Asientos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EventoId INT NOT NULL,
    Fila INT NOT NULL,
    Numero INT NOT NULL,
    Disponible BIT DEFAULT 1,
    FOREIGN KEY (EventoId) REFERENCES Eventos(Id) ON DELETE CASCADE,
    UNIQUE(EventoId, Fila, Numero)
);

-- Tabla de Compras/Boletos
CREATE TABLE Boletos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    EventoId INT NOT NULL,
    AsientoId INT NOT NULL,
    FechaCompra DATETIME2 DEFAULT GETDATE(),
    Precio DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
    FOREIGN KEY (EventoId) REFERENCES Eventos(Id),
    FOREIGN KEY (AsientoId) REFERENCES Asientos(Id),
    UNIQUE(AsientoId) -- Un asiento solo puede ser comprado una vez
);

-- Insertar datos de prueba
INSERT INTO Usuarios (Nombre, Email, Password, Rol) VALUES
('Administrador Sistema', 'admin@eventos.cr', 'admin123', 'Administrador'),
('Juan Pérez', 'juan@email.com', 'cliente123', 'Cliente'),
('María García', 'maria@email.com', 'cliente123', 'Cliente');

INSERT INTO Eventos (Nombre, Descripcion, FechaEvento, Lugar, Banner, PrecioEntrada) VALUES
('Concierto Rock Nacional', 'Gran concierto con las mejores bandas de Costa Rica', '2025-09-15 20:00:00', 'Teatro Nacional', '/images/concierto-rock.jpg', 15000),
('Festival de Jazz', 'Una noche mágica con los mejores artistas de jazz', '2025-10-20 19:00:00', 'Teatro Melico Salazar', '/images/festival-jazz.jpg', 20000),
('Obra Teatral Clásica', 'Presentación de la obra Romeo y Julieta', '2025-08-30 18:00:00', 'Teatro Nacional', '/images/romeo-julieta.jpg', 12000);