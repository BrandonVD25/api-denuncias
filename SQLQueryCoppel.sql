create database bdcoppel
go
drop database bdcoppel
go
use bdcoppel
go
--creacion de tablas--
create table empresa(
empid int primary key identity,
nombre varchar(100) not null
)
go
create table pais(
paisid int primary key identity,
paisnombre varchar(100) not null
)
go
create table estado(
estadoid int primary key identity,
estadonombre varchar(100) not null,
paisid int not null,
foreign key (paisid) references pais(paisid)
)
go
create table contacto(
contactoid int primary key identity,
nombre varchar(150) not null,
correo varchar(150) not null,
telefono varchar(20) not null
)
go
create table estatus(
estatusid int not null primary key identity,
info varchar(100) not null
)
go
create table denuncia(
folio int primary key identity(10000,1),
empresaid int not null, 
foreign key(empresaid) references empresa(empid),
estadoid int not null,
foreign key(estadoid) references estado(estadoid),
detalle varchar(max) not null,
fecha date not null,
centro numeric (10),
contrasenia varchar(20) not null,
estatusid int not null,
foreign key (estatusid) references estatus(estatusid),
contactoid int,
foreign key(contactoid) references contacto(contactoid)
)
create table comentarios (
  ComentarioID INT PRIMARY KEY IDENTITY,
  folio INT,
  Comentario VARCHAR(MAX),
  FechaComentario DATE,
  FOREIGN KEY (folio) REFERENCES denuncia(folio)
)
go
create table usuario(
correo varchar(100) primary key,
usuario varchar(50) not null,
contrasenia varchar(20) not null,
activo bit not null,
enLinea bit not null
)
go
--inserciones--
insert into empresa values('Afore Coppel'),('BanCoppel'),('Coppel')
select*from empresa
go
insert into pais values('Argentina'),('Estados Unidos'),('Mexico')
select*from pais
go
insert into estado values('Buenos Aires',1),('California',2),('Aguascalientes',3),('Campeche',3),('Cdmx',3)
select*from estado
go
insert into usuario values('andmin@hotmail.com','Admin','ingesoftware',1,0)
go
insert into estatus values('nueva'),('atendiendo'),('finalizada'),('cancelada')
insert into contacto(correo,nombre,telefono) values('bd231123@gmail.com','brandon vizcarra duarte','6672725132')
insert into denuncia values(3,5,'asalto a tienda',GETDATE(),4,'12345678',1,1)
insert into denuncia values(3,5,'asalto a a tienda por segunda vez',GETDATE(),3,'12345678',1,1)
select*from denuncia
go
insert into comentarios values(10000,'estaremos atendiendo tu denuncia pronto',getdate())
insert into comentarios values(10000,'necesitaremos mas tiempo para atender tu denuncia',getdate())
go
--procedimientos para denuncias--
--lista de informes de denuncias para mostrar al admin--
create or alter procedure ObtenerInformesDenuncias
as
begin
   set nocount on;
    select D.folio as Folio, E.nombre as NombreEmpresa, P.paisnombre as Pais, ES.estadonombre as Estado, 
           D.centro as NumCentro,  D.fecha as Fecha,
           EST.info as InfoEstatus
    from denuncia D
    INNER JOIN empresa E on D.empresaid = E.empid
    INNER JOIN estado ES on D.estadoid = ES.estadoid
    INNER JOIN pais P on ES.paisid = P.paisid
    LEFT JOIN estatus EST on D.estatusid = EST.estatusid;
end
exec ObtenerInformesDenuncias
go
--ver detalle de la denuncia admin--
create or alter procedure verDetalleDenuncia 
@folio INT
AS
BEGIN
    SELECT d.folio AS Folio,
           E.nombre AS Empresa,
           p.paisnombre AS Pais,
           es.estadonombre AS Estado,
           d.centro AS NumC,
           d.detalle AS Detalle,
           est.info AS Info
    FROM denuncia d
    INNER JOIN empresa e ON d.empresaid = e.empid
    INNER JOIN estado es ON d.estadoid = es.estadoid
    INNER JOIN pais p ON es.paisid = p.paisid
    INNER JOIN estatus est ON d.estatusid = est.estatusid
    WHERE d.folio = @folio
end
go
exec verDetalleDenuncia 10000
go
--Agregar una nueva denuncia--
create or alter procedure AgregarInformeDenuncias @empresaid int, @estadoid int, @detalle varchar(max),@fecha date,@centro numeric(10),@contrasenia varchar(20)
,@estatusid int= 1, @contactoid int=null,@nombre varchar(100)=null, @correo varchar(100)=null, @telefono varchar(100)=null,@folio int output
as begin
if @nombre is not null and @telefono is not null and @correo is not null
begin 
insert into contacto values(@nombre,@correo,@telefono)
	set  @contactoid=SCOPE_IDENTITY()
end
else
begin
	set @contactoid=null
end
  insert into denuncia 
  VALUES (@empresaid, @estadoid, @detalle, @fecha, @centro, @contrasenia, @estatusid, @contactoid);
  set @folio= SCOPE_IDENTITY()
  print @folio
end
go
select*from denuncia
--Para cambiar el estatus--
go
--Mostrar denuncia del denunciante-
create or alter procedure MostrarDenunciaPorfolioDenunciante @folio int as
begin
   set nocount on;
SELECT D.folio AS Folio,EST.info as Estatus, D.detalle as detalle, emp.nombre as empresa, es.estadonombre as estado
	FROM Denuncia D
	INNER JOIN Estatus EST ON D.estatusid = EST.estatusid
	INNER JOIN  empresa EMP on D.empresaid= EMP.empid
	INNER JOIN estado ES on D.estadoid=ES.estadoid
WHERE D.folio = @folio;
end
exec MostrarDenunciaPorfolioDenunciante 10000
go
--Mostrar el historial de comentarios--
create or alter procedure MostrarHistorialComentarios @folio int as
begin
   set nocount on;
SELECT C.Comentario as Comentario, C.FechaComentario as Fecha
	FROM comentarios C
	INNER JOIN denuncia D on D.folio= C.folio
WHERE C.folio = @folio;
end
go
exec MostrarHistorialComentarios 10000
go
select*from estatus
go
--Validar login por folio 
CREATE OR ALTER PROCEDURE ValidarLoginDenunciante
  @folio INT,
  @contrasenia VARCHAR(50),
  @result NVARCHAR(MAX) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (SELECT 1 FROM denuncia WHERE folio = @folio AND contrasenia = @contrasenia)
  BEGIN
    set @result= 'Inicio de sesión exitoso. El folio y la contraseña son válidos.';
  END
  ELSE
  BEGIN
    set @result= 'Inicio de sesión fallido. El folio y/o la contraseña son incorrectos.';
  END
END
go 
--Validar login Administrador
create or alter procedure ValidarLoginUsuario
 @correo VARCHAR(100),
  @contrasenia VARCHAR(20),
  @respuesta VARCHAR(100) OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRY
    IF EXISTS (SELECT 1 FROM usuario WHERE correo = @correo AND enlinea = 1)
    BEGIN
      SET @respuesta = 'Inicio de sesión fallido: el usuario ya está en línea.';
      RETURN;
    END

    IF EXISTS (SELECT 1 FROM usuario WHERE correo = @correo AND contrasenia = @contrasenia AND activo = 1)
    BEGIN
      UPDATE usuario
      SET enlinea = 1
      WHERE correo = @correo;

      SET @respuesta = 'Inicio de sesión exitoso.';
    END
    ELSE
    BEGIN
      SET @respuesta = 'Inicio de sesión fallido: verifique sus credenciales.';
    END
  END TRY
  BEGIN CATCH
    SET @respuesta = 'Error en el servidor: ' ;
  END CATCH
END
go
create or alter proc userSalir @correo varchar(200)
as begin 
update usuario set enLinea=0
where correo= @correo
end
go
CREATE PROCEDURE ActualizarEstatusYAgregarComentario
  @folio INT,
  @nuevoEstatusID INT,
  @nuevoComentario VARCHAR(MAX)
AS
BEGIN
  UPDATE denuncia
  SET estatusid = @nuevoEstatusID
  WHERE folio = @folio;
  INSERT INTO comentarios (folio, Comentario, FechaComentario)
  VALUES (@folio, @nuevoComentario, GETDATE())
END;
select*from comentarios