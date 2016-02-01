
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/01/2016 10:51:57
-- Generated from EDMX file: C:\Users\Kevin\documents\visual studio 2013\Projects\ExtractorDatos\Almacenamiento\Modelo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [trip2bilbao];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_parkingsentradas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[entradasSet] DROP CONSTRAINT [FK_parkingsentradas];
GO
IF OBJECT_ID(N'[dbo].[FK_parkingstarifas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tarifasSet] DROP CONSTRAINT [FK_parkingstarifas];
GO
IF OBJECT_ID(N'[dbo].[FK_tiempos_ciudadtiempos_dia_ciudad]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tiempos_dia_ciudadSet] DROP CONSTRAINT [FK_tiempos_ciudadtiempos_dia_ciudad];
GO
IF OBJECT_ID(N'[dbo].[FK_tiempos_comarcatiempos_dia_comarca]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tiempos_dia_comarcaSet] DROP CONSTRAINT [FK_tiempos_comarcatiempos_dia_comarca];
GO
IF OBJECT_ID(N'[dbo].[FK_tiempos_predicciontiempos_dia]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tiempos_diaSet] DROP CONSTRAINT [FK_tiempos_predicciontiempos_dia];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_metroparadas_metro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[paradas_metroSet] DROP CONSTRAINT [FK_paradas_metroparadas_metro];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_metroviajes_metro_paradas_metro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[paradas_metroviajes_metro] DROP CONSTRAINT [FK_paradas_metroviajes_metro_paradas_metro];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_metroviajes_metro_viajes_metro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[paradas_metroviajes_metro] DROP CONSTRAINT [FK_paradas_metroviajes_metro_viajes_metro];
GO
IF OBJECT_ID(N'[dbo].[FK_lineas_bizkaibusviajes_bizkaibus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_bizkaibusSet] DROP CONSTRAINT [FK_lineas_bizkaibusviajes_bizkaibus];
GO
IF OBJECT_ID(N'[dbo].[FK_viajes_bizkaibusparadas_bizkaibus_viajes_bizkaibus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus] DROP CONSTRAINT [FK_viajes_bizkaibusparadas_bizkaibus_viajes_bizkaibus];
GO
IF OBJECT_ID(N'[dbo].[FK_viajes_bizkaibusparadas_bizkaibus_paradas_bizkaibus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus] DROP CONSTRAINT [FK_viajes_bizkaibusparadas_bizkaibus_paradas_bizkaibus];
GO
IF OBJECT_ID(N'[dbo].[FK_lineas_bilbobustiempos_linea_parada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tiempos_linea_paradaSet] DROP CONSTRAINT [FK_lineas_bilbobustiempos_linea_parada];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_bilbobustiempos_linea_parada]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tiempos_linea_paradaSet] DROP CONSTRAINT [FK_paradas_bilbobustiempos_linea_parada];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_bilbobusviajes_bilbobus_paradas_bilbobus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[paradas_bilbobusviajes_bilbobus] DROP CONSTRAINT [FK_paradas_bilbobusviajes_bilbobus_paradas_bilbobus];
GO
IF OBJECT_ID(N'[dbo].[FK_paradas_bilbobusviajes_bilbobus_viajes_bilbobus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[paradas_bilbobusviajes_bilbobus] DROP CONSTRAINT [FK_paradas_bilbobusviajes_bilbobus_viajes_bilbobus];
GO
IF OBJECT_ID(N'[dbo].[FK_lineas_bilbobusviajes_bilbobus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_bilbobusSet] DROP CONSTRAINT [FK_lineas_bilbobusviajes_bilbobus];
GO
IF OBJECT_ID(N'[dbo].[FK_lineas_euskotrenviajes_euskotren]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_euskotrenSet] DROP CONSTRAINT [FK_lineas_euskotrenviajes_euskotren];
GO
IF OBJECT_ID(N'[dbo].[FK_viajes_euskotrenparadas_euskotren_viajes_euskotren]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_euskotrenparadas_euskotren] DROP CONSTRAINT [FK_viajes_euskotrenparadas_euskotren_viajes_euskotren];
GO
IF OBJECT_ID(N'[dbo].[FK_viajes_euskotrenparadas_euskotren_paradas_euskotren]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_euskotrenparadas_euskotren] DROP CONSTRAINT [FK_viajes_euskotrenparadas_euskotren_paradas_euskotren];
GO
IF OBJECT_ID(N'[dbo].[FK_lineas_metroviajes_metro]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[viajes_metroSet] DROP CONSTRAINT [FK_lineas_metroviajes_metro];
GO
IF OBJECT_ID(N'[dbo].[FK_parkingsestados_parking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[estados_parkingSet] DROP CONSTRAINT [FK_parkingsestados_parking];
GO
IF OBJECT_ID(N'[dbo].[FK_puntos_biciestados_puntobici]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[estados_puntobiciSet] DROP CONSTRAINT [FK_puntos_biciestados_puntobici];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[entradasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[entradasSet];
GO
IF OBJECT_ID(N'[dbo].[tarifasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tarifasSet];
GO
IF OBJECT_ID(N'[dbo].[parkingsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[parkingsSet];
GO
IF OBJECT_ID(N'[dbo].[incidenciasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[incidenciasSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_tranviaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_tranviaSet];
GO
IF OBJECT_ID(N'[dbo].[hospitalesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[hospitalesSet];
GO
IF OBJECT_ID(N'[dbo].[puntos_biciSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[puntos_biciSet];
GO
IF OBJECT_ID(N'[dbo].[centros_de_saludSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[centros_de_saludSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_dia_ciudadSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_dia_ciudadSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_ciudadSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_ciudadSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_comarcaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_comarcaSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_dia_comarcaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_dia_comarcaSet];
GO
IF OBJECT_ID(N'[dbo].[farmaciasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[farmaciasSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_diaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_diaSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_prediccionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_prediccionSet];
GO
IF OBJECT_ID(N'[dbo].[lineas_metroSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[lineas_metroSet];
GO
IF OBJECT_ID(N'[dbo].[viajes_metroSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_metroSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_metroSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_metroSet];
GO
IF OBJECT_ID(N'[dbo].[lineas_bizkaibusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[lineas_bizkaibusSet];
GO
IF OBJECT_ID(N'[dbo].[viajes_bizkaibusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_bizkaibusSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_bizkaibusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_bizkaibusSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_bilbobusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_bilbobusSet];
GO
IF OBJECT_ID(N'[dbo].[tiempos_linea_paradaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tiempos_linea_paradaSet];
GO
IF OBJECT_ID(N'[dbo].[lineas_bilbobusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[lineas_bilbobusSet];
GO
IF OBJECT_ID(N'[dbo].[viajes_bilbobusSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_bilbobusSet];
GO
IF OBJECT_ID(N'[dbo].[lineas_euskotrenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[lineas_euskotrenSet];
GO
IF OBJECT_ID(N'[dbo].[viajes_euskotrenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_euskotrenSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_euskotrenSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_euskotrenSet];
GO
IF OBJECT_ID(N'[dbo].[estados_parkingSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[estados_parkingSet];
GO
IF OBJECT_ID(N'[dbo].[estados_puntobiciSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[estados_puntobiciSet];
GO
IF OBJECT_ID(N'[dbo].[parkingDeustoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[parkingDeustoSet];
GO
IF OBJECT_ID(N'[dbo].[paradas_metroviajes_metro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_metroviajes_metro];
GO
IF OBJECT_ID(N'[dbo].[viajes_bizkaibusparadas_bizkaibus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus];
GO
IF OBJECT_ID(N'[dbo].[paradas_bilbobusviajes_bilbobus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[paradas_bilbobusviajes_bilbobus];
GO
IF OBJECT_ID(N'[dbo].[viajes_euskotrenparadas_euskotren]', 'U') IS NOT NULL
    DROP TABLE [dbo].[viajes_euskotrenparadas_euskotren];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'entradasSet'
CREATE TABLE [dbo].[entradasSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [parkingsId] int  NOT NULL
);
GO

-- Creating table 'tarifasSet'
CREATE TABLE [dbo].[tarifasSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [tipo] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [zona] nvarchar(max)  NOT NULL,
    [actualizacion] datetime  NOT NULL,
    [parkingsId] int  NOT NULL
);
GO

-- Creating table 'parkingsSet'
CREATE TABLE [dbo].[parkingsSet] (
    [id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [tipo] nvarchar(max)  NOT NULL,
    [fecha] datetime  NOT NULL,
    [estado] nvarchar(max)  NOT NULL,
    [capacidad] int  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL
);
GO

-- Creating table 'incidenciasSet'
CREATE TABLE [dbo].[incidenciasSet] (
    [id] int  NOT NULL,
    [tipo] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [fechaInicio] datetime  NOT NULL,
    [fechaFin] datetime  NOT NULL
);
GO

-- Creating table 'paradas_tranviaSet'
CREATE TABLE [dbo].[paradas_tranviaSet] (
    [Id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL
);
GO

-- Creating table 'hospitalesSet'
CREATE TABLE [dbo].[hospitalesSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [direccion] nvarchar(max)  NOT NULL,
    [codigoPostal] int  NOT NULL,
    [region] nvarchar(max)  NOT NULL,
    [calle] nvarchar(max)  NOT NULL,
    [ciudad] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [web] nvarchar(max)  NOT NULL,
    [telefono] bigint  NOT NULL,
    [idHospital] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'puntos_biciSet'
CREATE TABLE [dbo].[puntos_biciSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [estado] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [capacidad] int  NOT NULL
);
GO

-- Creating table 'centros_de_saludSet'
CREATE TABLE [dbo].[centros_de_saludSet] (
    [codigo_centro] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [direcionCompleta] nvarchar(max)  NOT NULL,
    [codigoPostal] int  NOT NULL,
    [provincia] nvarchar(max)  NOT NULL,
    [region] nvarchar(max)  NOT NULL,
    [horario] nvarchar(max)  NOT NULL,
    [calle] nvarchar(max)  NOT NULL,
    [ciudad] nvarchar(max)  NOT NULL,
    [urlAdicional] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [web] nvarchar(max)  NOT NULL,
    [telefono] bigint  NOT NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'tiempos_dia_ciudadSet'
CREATE TABLE [dbo].[tiempos_dia_ciudadSet] (
    [id] int  NOT NULL,
    [nombreCiudad] nvarchar(max)  NOT NULL,
    [descripcionES] nvarchar(max)  NOT NULL,
    [descripcionEU] nvarchar(max)  NOT NULL,
    [maxima] int  NOT NULL,
    [minima] int  NOT NULL,
    [tiempos_ciudadId] int  NOT NULL
);
GO

-- Creating table 'tiempos_ciudadSet'
CREATE TABLE [dbo].[tiempos_ciudadSet] (
    [id] int  NOT NULL,
    [horaDeActualizacion] datetime  NOT NULL,
    [dia] datetime  NOT NULL,
    [descripcionES] nvarchar(max)  NOT NULL,
    [descripcionEU] nvarchar(max)  NOT NULL,
    [tiempos_dia_ciudadId] int  NOT NULL
);
GO

-- Creating table 'tiempos_comarcaSet'
CREATE TABLE [dbo].[tiempos_comarcaSet] (
    [id] int  NOT NULL,
    [diaActualizacion] datetime  NOT NULL,
    [nombreComarcaES] nvarchar(max)  NOT NULL,
    [nombreComarcaEU] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tiempos_dia_comarcaSet'
CREATE TABLE [dbo].[tiempos_dia_comarcaSet] (
    [id] int  NOT NULL,
    [fechaPrediccion] datetime  NOT NULL,
    [vientoES] nvarchar(max)  NOT NULL,
    [vientoEU] nvarchar(max)  NOT NULL,
    [tiempoES] nvarchar(max)  NOT NULL,
    [tiempoEU] nvarchar(max)  NOT NULL,
    [temperaturaES] nvarchar(max)  NOT NULL,
    [temperaturaEU] nvarchar(max)  NOT NULL,
    [descripcionES] nvarchar(max)  NOT NULL,
    [descripcionEU] nvarchar(max)  NOT NULL,
    [tiempos_comarcaId] int  NOT NULL
);
GO

-- Creating table 'farmaciasSet'
CREATE TABLE [dbo].[farmaciasSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [direccion] nvarchar(max)  NOT NULL,
    [direccionAbreviada] nvarchar(max)  NOT NULL,
    [provincia] nvarchar(max)  NOT NULL,
    [ciudad] nvarchar(max)  NOT NULL,
    [codigoPostal] int  NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [telefono] bigint  NOT NULL,
    [web] nvarchar(max)  NOT NULL,
    [informacionAdcional] nvarchar(max)  NOT NULL,
    [nombrePropietario] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tiempos_diaSet'
CREATE TABLE [dbo].[tiempos_diaSet] (
    [id] int  NOT NULL,
    [fechaPrediccion] datetime  NOT NULL,
    [vientoES] nvarchar(max)  NOT NULL,
    [vientoEU] nvarchar(max)  NOT NULL,
    [tiempoES] nvarchar(max)  NOT NULL,
    [tiempoEU] nvarchar(max)  NOT NULL,
    [temperaturaES] nvarchar(max)  NOT NULL,
    [temperaturaEU] nvarchar(max)  NOT NULL,
    [tiempo_prediccionId] int  NOT NULL,
    [tiempos_prediccionId] int  NOT NULL
);
GO

-- Creating table 'tiempos_prediccionSet'
CREATE TABLE [dbo].[tiempos_prediccionSet] (
    [id] int  NOT NULL,
    [fechaInicioPrediccion] nvarchar(max)  NOT NULL,
    [fechaFinPrediccion] nvarchar(max)  NOT NULL,
    [fechaRealizacionPrediccion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'lineas_metroSet'
CREATE TABLE [dbo].[lineas_metroSet] (
    [id] int  NOT NULL,
    [abreviatura] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [tipo] int  NOT NULL,
    [idMetro] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'viajes_metroSet'
CREATE TABLE [dbo].[viajes_metroSet] (
    [id] int  NOT NULL,
    [lineas_metro_id] int  NOT NULL
);
GO

-- Creating table 'paradas_metroSet'
CREATE TABLE [dbo].[paradas_metroSet] (
    [id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [latitud] nvarchar(max)  NOT NULL,
    [longitud] nvarchar(max)  NOT NULL,
    [tipoLocalizacion] nvarchar(max)  NOT NULL,
    [paradas_metroId] int  NOT NULL
);
GO

-- Creating table 'lineas_bizkaibusSet'
CREATE TABLE [dbo].[lineas_bizkaibusSet] (
    [id] int  NOT NULL,
    [abreviatura] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [tipoTransporte] int  NOT NULL
);
GO

-- Creating table 'viajes_bizkaibusSet'
CREATE TABLE [dbo].[viajes_bizkaibusSet] (
    [id] int  NOT NULL,
    [lineas_bizkaibusId] int  NOT NULL
);
GO

-- Creating table 'paradas_bizkaibusSet'
CREATE TABLE [dbo].[paradas_bizkaibusSet] (
    [id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [tipoLocalizacion] int  NOT NULL
);
GO

-- Creating table 'paradas_bilbobusSet'
CREATE TABLE [dbo].[paradas_bilbobusSet] (
    [id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [latitud] float  NOT NULL,
    [longitud] float  NOT NULL
);
GO

-- Creating table 'tiempos_linea_paradaSet'
CREATE TABLE [dbo].[tiempos_linea_paradaSet] (
    [id] int  NOT NULL,
    [descripcionLinea] nvarchar(max)  NOT NULL,
    [tiempoEspera] nvarchar(max)  NOT NULL,
    [lineas_bilbobusId] int  NOT NULL,
    [paradas_bilbobusId] int  NOT NULL,
    [fecha] datetime  NOT NULL
);
GO

-- Creating table 'lineas_bilbobusSet'
CREATE TABLE [dbo].[lineas_bilbobusSet] (
    [id] int  NOT NULL,
    [abreviatura] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [tipoTransporte] int  NOT NULL
);
GO

-- Creating table 'viajes_bilbobusSet'
CREATE TABLE [dbo].[viajes_bilbobusSet] (
    [id] int  NOT NULL,
    [lineas_bilbobusId] int  NOT NULL
);
GO

-- Creating table 'lineas_euskotrenSet'
CREATE TABLE [dbo].[lineas_euskotrenSet] (
    [id] int  NOT NULL,
    [abreviatura] nvarchar(max)  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [tipo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'viajes_euskotrenSet'
CREATE TABLE [dbo].[viajes_euskotrenSet] (
    [id] int  NOT NULL,
    [lineas_euskotrenId] int  NOT NULL
);
GO

-- Creating table 'paradas_euskotrenSet'
CREATE TABLE [dbo].[paradas_euskotrenSet] (
    [id] int  NOT NULL,
    [nombre] nvarchar(max)  NOT NULL,
    [descripcion] nvarchar(max)  NOT NULL,
    [latitud] nvarchar(max)  NOT NULL,
    [longitud] nvarchar(max)  NOT NULL,
    [url] nvarchar(max)  NOT NULL,
    [tipoLocalizacion] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'estados_parkingSet'
CREATE TABLE [dbo].[estados_parkingSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [disponible] int  NOT NULL,
    [fecha] datetime  NOT NULL,
    [parkings_id] int  NOT NULL
);
GO

-- Creating table 'estados_puntobiciSet'
CREATE TABLE [dbo].[estados_puntobiciSet] (
    [id] int IDENTITY(1,1) NOT NULL,
    [anclajeslibres] int  NOT NULL,
    [bicisLibres] int  NOT NULL,
    [fecha] datetime  NOT NULL,
    [puntos_bici_id] int  NOT NULL
);
GO

-- Creating table 'parkingDeustoSet'
CREATE TABLE [dbo].[parkingDeustoSet] (
    [fecha] datetime  NOT NULL,
    [dbs] int  NOT NULL,
    [general] int  NOT NULL
);
GO

-- Creating table 'paradas_metroviajes_metro'
CREATE TABLE [dbo].[paradas_metroviajes_metro] (
    [paradas_metro_id] int  NOT NULL,
    [viajes_metro_id] int  NOT NULL
);
GO

-- Creating table 'viajes_bizkaibusparadas_bizkaibus'
CREATE TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus] (
    [viajes_bizkaibus_id] int  NOT NULL,
    [paradas_bizkaibus_id] int  NOT NULL
);
GO

-- Creating table 'paradas_bilbobusviajes_bilbobus'
CREATE TABLE [dbo].[paradas_bilbobusviajes_bilbobus] (
    [paradas_bilbobus_id] int  NOT NULL,
    [viajes_bilbobus_id] int  NOT NULL
);
GO

-- Creating table 'viajes_euskotrenparadas_euskotren'
CREATE TABLE [dbo].[viajes_euskotrenparadas_euskotren] (
    [viajes_euskotren_id] int  NOT NULL,
    [paradas_euskotren_id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'entradasSet'
ALTER TABLE [dbo].[entradasSet]
ADD CONSTRAINT [PK_entradasSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tarifasSet'
ALTER TABLE [dbo].[tarifasSet]
ADD CONSTRAINT [PK_tarifasSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'parkingsSet'
ALTER TABLE [dbo].[parkingsSet]
ADD CONSTRAINT [PK_parkingsSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [fechaInicio], [fechaFin] in table 'incidenciasSet'
ALTER TABLE [dbo].[incidenciasSet]
ADD CONSTRAINT [PK_incidenciasSet]
    PRIMARY KEY CLUSTERED ([id], [fechaInicio], [fechaFin] ASC);
GO

-- Creating primary key on [Id] in table 'paradas_tranviaSet'
ALTER TABLE [dbo].[paradas_tranviaSet]
ADD CONSTRAINT [PK_paradas_tranviaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'hospitalesSet'
ALTER TABLE [dbo].[hospitalesSet]
ADD CONSTRAINT [PK_hospitalesSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'puntos_biciSet'
ALTER TABLE [dbo].[puntos_biciSet]
ADD CONSTRAINT [PK_puntos_biciSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'centros_de_saludSet'
ALTER TABLE [dbo].[centros_de_saludSet]
ADD CONSTRAINT [PK_centros_de_saludSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_dia_ciudadSet'
ALTER TABLE [dbo].[tiempos_dia_ciudadSet]
ADD CONSTRAINT [PK_tiempos_dia_ciudadSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_ciudadSet'
ALTER TABLE [dbo].[tiempos_ciudadSet]
ADD CONSTRAINT [PK_tiempos_ciudadSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_comarcaSet'
ALTER TABLE [dbo].[tiempos_comarcaSet]
ADD CONSTRAINT [PK_tiempos_comarcaSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_dia_comarcaSet'
ALTER TABLE [dbo].[tiempos_dia_comarcaSet]
ADD CONSTRAINT [PK_tiempos_dia_comarcaSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'farmaciasSet'
ALTER TABLE [dbo].[farmaciasSet]
ADD CONSTRAINT [PK_farmaciasSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_diaSet'
ALTER TABLE [dbo].[tiempos_diaSet]
ADD CONSTRAINT [PK_tiempos_diaSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'tiempos_prediccionSet'
ALTER TABLE [dbo].[tiempos_prediccionSet]
ADD CONSTRAINT [PK_tiempos_prediccionSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'lineas_metroSet'
ALTER TABLE [dbo].[lineas_metroSet]
ADD CONSTRAINT [PK_lineas_metroSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'viajes_metroSet'
ALTER TABLE [dbo].[viajes_metroSet]
ADD CONSTRAINT [PK_viajes_metroSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'paradas_metroSet'
ALTER TABLE [dbo].[paradas_metroSet]
ADD CONSTRAINT [PK_paradas_metroSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'lineas_bizkaibusSet'
ALTER TABLE [dbo].[lineas_bizkaibusSet]
ADD CONSTRAINT [PK_lineas_bizkaibusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'viajes_bizkaibusSet'
ALTER TABLE [dbo].[viajes_bizkaibusSet]
ADD CONSTRAINT [PK_viajes_bizkaibusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'paradas_bizkaibusSet'
ALTER TABLE [dbo].[paradas_bizkaibusSet]
ADD CONSTRAINT [PK_paradas_bizkaibusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'paradas_bilbobusSet'
ALTER TABLE [dbo].[paradas_bilbobusSet]
ADD CONSTRAINT [PK_paradas_bilbobusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id], [fecha] in table 'tiempos_linea_paradaSet'
ALTER TABLE [dbo].[tiempos_linea_paradaSet]
ADD CONSTRAINT [PK_tiempos_linea_paradaSet]
    PRIMARY KEY CLUSTERED ([id], [fecha] ASC);
GO

-- Creating primary key on [id] in table 'lineas_bilbobusSet'
ALTER TABLE [dbo].[lineas_bilbobusSet]
ADD CONSTRAINT [PK_lineas_bilbobusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'viajes_bilbobusSet'
ALTER TABLE [dbo].[viajes_bilbobusSet]
ADD CONSTRAINT [PK_viajes_bilbobusSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'lineas_euskotrenSet'
ALTER TABLE [dbo].[lineas_euskotrenSet]
ADD CONSTRAINT [PK_lineas_euskotrenSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'viajes_euskotrenSet'
ALTER TABLE [dbo].[viajes_euskotrenSet]
ADD CONSTRAINT [PK_viajes_euskotrenSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'paradas_euskotrenSet'
ALTER TABLE [dbo].[paradas_euskotrenSet]
ADD CONSTRAINT [PK_paradas_euskotrenSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'estados_parkingSet'
ALTER TABLE [dbo].[estados_parkingSet]
ADD CONSTRAINT [PK_estados_parkingSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'estados_puntobiciSet'
ALTER TABLE [dbo].[estados_puntobiciSet]
ADD CONSTRAINT [PK_estados_puntobiciSet]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [fecha] in table 'parkingDeustoSet'
ALTER TABLE [dbo].[parkingDeustoSet]
ADD CONSTRAINT [PK_parkingDeustoSet]
    PRIMARY KEY CLUSTERED ([fecha] ASC);
GO

-- Creating primary key on [paradas_metro_id], [viajes_metro_id] in table 'paradas_metroviajes_metro'
ALTER TABLE [dbo].[paradas_metroviajes_metro]
ADD CONSTRAINT [PK_paradas_metroviajes_metro]
    PRIMARY KEY CLUSTERED ([paradas_metro_id], [viajes_metro_id] ASC);
GO

-- Creating primary key on [viajes_bizkaibus_id], [paradas_bizkaibus_id] in table 'viajes_bizkaibusparadas_bizkaibus'
ALTER TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus]
ADD CONSTRAINT [PK_viajes_bizkaibusparadas_bizkaibus]
    PRIMARY KEY CLUSTERED ([viajes_bizkaibus_id], [paradas_bizkaibus_id] ASC);
GO

-- Creating primary key on [paradas_bilbobus_id], [viajes_bilbobus_id] in table 'paradas_bilbobusviajes_bilbobus'
ALTER TABLE [dbo].[paradas_bilbobusviajes_bilbobus]
ADD CONSTRAINT [PK_paradas_bilbobusviajes_bilbobus]
    PRIMARY KEY CLUSTERED ([paradas_bilbobus_id], [viajes_bilbobus_id] ASC);
GO

-- Creating primary key on [viajes_euskotren_id], [paradas_euskotren_id] in table 'viajes_euskotrenparadas_euskotren'
ALTER TABLE [dbo].[viajes_euskotrenparadas_euskotren]
ADD CONSTRAINT [PK_viajes_euskotrenparadas_euskotren]
    PRIMARY KEY CLUSTERED ([viajes_euskotren_id], [paradas_euskotren_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [parkingsId] in table 'entradasSet'
ALTER TABLE [dbo].[entradasSet]
ADD CONSTRAINT [FK_parkingsentradas]
    FOREIGN KEY ([parkingsId])
    REFERENCES [dbo].[parkingsSet]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_parkingsentradas'
CREATE INDEX [IX_FK_parkingsentradas]
ON [dbo].[entradasSet]
    ([parkingsId]);
GO

-- Creating foreign key on [parkingsId] in table 'tarifasSet'
ALTER TABLE [dbo].[tarifasSet]
ADD CONSTRAINT [FK_parkingstarifas]
    FOREIGN KEY ([parkingsId])
    REFERENCES [dbo].[parkingsSet]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_parkingstarifas'
CREATE INDEX [IX_FK_parkingstarifas]
ON [dbo].[tarifasSet]
    ([parkingsId]);
GO

-- Creating foreign key on [tiempos_ciudadId] in table 'tiempos_dia_ciudadSet'
ALTER TABLE [dbo].[tiempos_dia_ciudadSet]
ADD CONSTRAINT [FK_tiempos_ciudadtiempos_dia_ciudad]
    FOREIGN KEY ([tiempos_ciudadId])
    REFERENCES [dbo].[tiempos_ciudadSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tiempos_ciudadtiempos_dia_ciudad'
CREATE INDEX [IX_FK_tiempos_ciudadtiempos_dia_ciudad]
ON [dbo].[tiempos_dia_ciudadSet]
    ([tiempos_ciudadId]);
GO

-- Creating foreign key on [tiempos_comarcaId] in table 'tiempos_dia_comarcaSet'
ALTER TABLE [dbo].[tiempos_dia_comarcaSet]
ADD CONSTRAINT [FK_tiempos_comarcatiempos_dia_comarca]
    FOREIGN KEY ([tiempos_comarcaId])
    REFERENCES [dbo].[tiempos_comarcaSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tiempos_comarcatiempos_dia_comarca'
CREATE INDEX [IX_FK_tiempos_comarcatiempos_dia_comarca]
ON [dbo].[tiempos_dia_comarcaSet]
    ([tiempos_comarcaId]);
GO

-- Creating foreign key on [tiempos_prediccionId] in table 'tiempos_diaSet'
ALTER TABLE [dbo].[tiempos_diaSet]
ADD CONSTRAINT [FK_tiempos_predicciontiempos_dia]
    FOREIGN KEY ([tiempos_prediccionId])
    REFERENCES [dbo].[tiempos_prediccionSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tiempos_predicciontiempos_dia'
CREATE INDEX [IX_FK_tiempos_predicciontiempos_dia]
ON [dbo].[tiempos_diaSet]
    ([tiempos_prediccionId]);
GO

-- Creating foreign key on [paradas_metroId] in table 'paradas_metroSet'
ALTER TABLE [dbo].[paradas_metroSet]
ADD CONSTRAINT [FK_paradas_metroparadas_metro]
    FOREIGN KEY ([paradas_metroId])
    REFERENCES [dbo].[paradas_metroSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_paradas_metroparadas_metro'
CREATE INDEX [IX_FK_paradas_metroparadas_metro]
ON [dbo].[paradas_metroSet]
    ([paradas_metroId]);
GO

-- Creating foreign key on [paradas_metro_id] in table 'paradas_metroviajes_metro'
ALTER TABLE [dbo].[paradas_metroviajes_metro]
ADD CONSTRAINT [FK_paradas_metroviajes_metro_paradas_metro]
    FOREIGN KEY ([paradas_metro_id])
    REFERENCES [dbo].[paradas_metroSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [viajes_metro_id] in table 'paradas_metroviajes_metro'
ALTER TABLE [dbo].[paradas_metroviajes_metro]
ADD CONSTRAINT [FK_paradas_metroviajes_metro_viajes_metro]
    FOREIGN KEY ([viajes_metro_id])
    REFERENCES [dbo].[viajes_metroSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_paradas_metroviajes_metro_viajes_metro'
CREATE INDEX [IX_FK_paradas_metroviajes_metro_viajes_metro]
ON [dbo].[paradas_metroviajes_metro]
    ([viajes_metro_id]);
GO

-- Creating foreign key on [lineas_bizkaibusId] in table 'viajes_bizkaibusSet'
ALTER TABLE [dbo].[viajes_bizkaibusSet]
ADD CONSTRAINT [FK_lineas_bizkaibusviajes_bizkaibus]
    FOREIGN KEY ([lineas_bizkaibusId])
    REFERENCES [dbo].[lineas_bizkaibusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_lineas_bizkaibusviajes_bizkaibus'
CREATE INDEX [IX_FK_lineas_bizkaibusviajes_bizkaibus]
ON [dbo].[viajes_bizkaibusSet]
    ([lineas_bizkaibusId]);
GO

-- Creating foreign key on [viajes_bizkaibus_id] in table 'viajes_bizkaibusparadas_bizkaibus'
ALTER TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus]
ADD CONSTRAINT [FK_viajes_bizkaibusparadas_bizkaibus_viajes_bizkaibus]
    FOREIGN KEY ([viajes_bizkaibus_id])
    REFERENCES [dbo].[viajes_bizkaibusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [paradas_bizkaibus_id] in table 'viajes_bizkaibusparadas_bizkaibus'
ALTER TABLE [dbo].[viajes_bizkaibusparadas_bizkaibus]
ADD CONSTRAINT [FK_viajes_bizkaibusparadas_bizkaibus_paradas_bizkaibus]
    FOREIGN KEY ([paradas_bizkaibus_id])
    REFERENCES [dbo].[paradas_bizkaibusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_viajes_bizkaibusparadas_bizkaibus_paradas_bizkaibus'
CREATE INDEX [IX_FK_viajes_bizkaibusparadas_bizkaibus_paradas_bizkaibus]
ON [dbo].[viajes_bizkaibusparadas_bizkaibus]
    ([paradas_bizkaibus_id]);
GO

-- Creating foreign key on [lineas_bilbobusId] in table 'tiempos_linea_paradaSet'
ALTER TABLE [dbo].[tiempos_linea_paradaSet]
ADD CONSTRAINT [FK_lineas_bilbobustiempos_linea_parada]
    FOREIGN KEY ([lineas_bilbobusId])
    REFERENCES [dbo].[lineas_bilbobusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_lineas_bilbobustiempos_linea_parada'
CREATE INDEX [IX_FK_lineas_bilbobustiempos_linea_parada]
ON [dbo].[tiempos_linea_paradaSet]
    ([lineas_bilbobusId]);
GO

-- Creating foreign key on [paradas_bilbobusId] in table 'tiempos_linea_paradaSet'
ALTER TABLE [dbo].[tiempos_linea_paradaSet]
ADD CONSTRAINT [FK_paradas_bilbobustiempos_linea_parada]
    FOREIGN KEY ([paradas_bilbobusId])
    REFERENCES [dbo].[paradas_bilbobusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_paradas_bilbobustiempos_linea_parada'
CREATE INDEX [IX_FK_paradas_bilbobustiempos_linea_parada]
ON [dbo].[tiempos_linea_paradaSet]
    ([paradas_bilbobusId]);
GO

-- Creating foreign key on [paradas_bilbobus_id] in table 'paradas_bilbobusviajes_bilbobus'
ALTER TABLE [dbo].[paradas_bilbobusviajes_bilbobus]
ADD CONSTRAINT [FK_paradas_bilbobusviajes_bilbobus_paradas_bilbobus]
    FOREIGN KEY ([paradas_bilbobus_id])
    REFERENCES [dbo].[paradas_bilbobusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [viajes_bilbobus_id] in table 'paradas_bilbobusviajes_bilbobus'
ALTER TABLE [dbo].[paradas_bilbobusviajes_bilbobus]
ADD CONSTRAINT [FK_paradas_bilbobusviajes_bilbobus_viajes_bilbobus]
    FOREIGN KEY ([viajes_bilbobus_id])
    REFERENCES [dbo].[viajes_bilbobusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_paradas_bilbobusviajes_bilbobus_viajes_bilbobus'
CREATE INDEX [IX_FK_paradas_bilbobusviajes_bilbobus_viajes_bilbobus]
ON [dbo].[paradas_bilbobusviajes_bilbobus]
    ([viajes_bilbobus_id]);
GO

-- Creating foreign key on [lineas_bilbobusId] in table 'viajes_bilbobusSet'
ALTER TABLE [dbo].[viajes_bilbobusSet]
ADD CONSTRAINT [FK_lineas_bilbobusviajes_bilbobus]
    FOREIGN KEY ([lineas_bilbobusId])
    REFERENCES [dbo].[lineas_bilbobusSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_lineas_bilbobusviajes_bilbobus'
CREATE INDEX [IX_FK_lineas_bilbobusviajes_bilbobus]
ON [dbo].[viajes_bilbobusSet]
    ([lineas_bilbobusId]);
GO

-- Creating foreign key on [lineas_euskotrenId] in table 'viajes_euskotrenSet'
ALTER TABLE [dbo].[viajes_euskotrenSet]
ADD CONSTRAINT [FK_lineas_euskotrenviajes_euskotren]
    FOREIGN KEY ([lineas_euskotrenId])
    REFERENCES [dbo].[lineas_euskotrenSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_lineas_euskotrenviajes_euskotren'
CREATE INDEX [IX_FK_lineas_euskotrenviajes_euskotren]
ON [dbo].[viajes_euskotrenSet]
    ([lineas_euskotrenId]);
GO

-- Creating foreign key on [viajes_euskotren_id] in table 'viajes_euskotrenparadas_euskotren'
ALTER TABLE [dbo].[viajes_euskotrenparadas_euskotren]
ADD CONSTRAINT [FK_viajes_euskotrenparadas_euskotren_viajes_euskotren]
    FOREIGN KEY ([viajes_euskotren_id])
    REFERENCES [dbo].[viajes_euskotrenSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [paradas_euskotren_id] in table 'viajes_euskotrenparadas_euskotren'
ALTER TABLE [dbo].[viajes_euskotrenparadas_euskotren]
ADD CONSTRAINT [FK_viajes_euskotrenparadas_euskotren_paradas_euskotren]
    FOREIGN KEY ([paradas_euskotren_id])
    REFERENCES [dbo].[paradas_euskotrenSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_viajes_euskotrenparadas_euskotren_paradas_euskotren'
CREATE INDEX [IX_FK_viajes_euskotrenparadas_euskotren_paradas_euskotren]
ON [dbo].[viajes_euskotrenparadas_euskotren]
    ([paradas_euskotren_id]);
GO

-- Creating foreign key on [lineas_metro_id] in table 'viajes_metroSet'
ALTER TABLE [dbo].[viajes_metroSet]
ADD CONSTRAINT [FK_lineas_metroviajes_metro]
    FOREIGN KEY ([lineas_metro_id])
    REFERENCES [dbo].[lineas_metroSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_lineas_metroviajes_metro'
CREATE INDEX [IX_FK_lineas_metroviajes_metro]
ON [dbo].[viajes_metroSet]
    ([lineas_metro_id]);
GO

-- Creating foreign key on [parkings_id] in table 'estados_parkingSet'
ALTER TABLE [dbo].[estados_parkingSet]
ADD CONSTRAINT [FK_parkingsestados_parking]
    FOREIGN KEY ([parkings_id])
    REFERENCES [dbo].[parkingsSet]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_parkingsestados_parking'
CREATE INDEX [IX_FK_parkingsestados_parking]
ON [dbo].[estados_parkingSet]
    ([parkings_id]);
GO

-- Creating foreign key on [puntos_bici_id] in table 'estados_puntobiciSet'
ALTER TABLE [dbo].[estados_puntobiciSet]
ADD CONSTRAINT [FK_puntos_biciestados_puntobici]
    FOREIGN KEY ([puntos_bici_id])
    REFERENCES [dbo].[puntos_biciSet]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_puntos_biciestados_puntobici'
CREATE INDEX [IX_FK_puntos_biciestados_puntobici]
ON [dbo].[estados_puntobiciSet]
    ([puntos_bici_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------