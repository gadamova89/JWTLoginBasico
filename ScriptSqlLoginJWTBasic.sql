create database LoginJwtBasico
use LoginJwtBasico

create table Usuario
(
	IdUsuario int identity(1,1) primary key,
	Usuario varchar(100),
	PasswordHash varchar(500)
)
go

CREATE PROCEDURE ValidarUsuario
    @Usuario VARCHAR(100)
AS
BEGIN
    SELECT PasswordHash FROM Usuario WHERE Usuario = @Usuario
END
go


CREATE PROCEDURE RegistrarUsuario
    @Usuario VARCHAR(100),
    @Passwordhash VARCHAR(500)
AS
BEGIN
    INSERT INTO Usuario (Usuario, PasswordHash)
    VALUES (@Usuario, @Passwordhash)
END
