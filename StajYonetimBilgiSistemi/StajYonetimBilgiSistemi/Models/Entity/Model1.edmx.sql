
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/11/2022 11:15:12
-- Generated from EDMX file: C:\Users\silan\source\repos\StajYonetimBilgiSistemi\StajYonetimBilgiSistemi\Models\Entity\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SBYS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Event_Kullanicilar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Event] DROP CONSTRAINT [FK_Event_Kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[FK_GUNLUK_CALISMA_Kullanicilar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GUNLUK_CALISMA] DROP CONSTRAINT [FK_GUNLUK_CALISMA_Kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[FK_GUNLUK_CALISMA_STAJYER_TANIM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GUNLUK_CALISMA] DROP CONSTRAINT [FK_GUNLUK_CALISMA_STAJYER_TANIM];
GO
IF OBJECT_ID(N'[dbo].[FK_Kullanicilar_KURUM_DEPARTMAN]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kullanicilar] DROP CONSTRAINT [FK_Kullanicilar_KURUM_DEPARTMAN];
GO
IF OBJECT_ID(N'[dbo].[FK_Kullanicilar_KURUM_PERSONEL]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kullanicilar] DROP CONSTRAINT [FK_Kullanicilar_KURUM_PERSONEL];
GO
IF OBJECT_ID(N'[dbo].[FK_Kullanicilar_KURUM_TANIM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kullanicilar] DROP CONSTRAINT [FK_Kullanicilar_KURUM_TANIM];
GO
IF OBJECT_ID(N'[dbo].[FK_KURUM_DEPARTMAN_KURUM_TANIM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KURUM_DEPARTMAN] DROP CONSTRAINT [FK_KURUM_DEPARTMAN_KURUM_TANIM];
GO
IF OBJECT_ID(N'[dbo].[FK_KURUM_PERSONEL_KURUM_DEPARTMAN]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KURUM_PERSONEL] DROP CONSTRAINT [FK_KURUM_PERSONEL_KURUM_DEPARTMAN];
GO
IF OBJECT_ID(N'[dbo].[FK_KURUM_PERSONEL_KURUM_TANIM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[KURUM_PERSONEL] DROP CONSTRAINT [FK_KURUM_PERSONEL_KURUM_TANIM];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_Bolumler]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_Bolumler];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_KURUM_DEPARTMAN]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_KURUM_DEPARTMAN];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_KURUM_PERSONEL]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_KURUM_PERSONEL];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_KURUM_PERSONEL1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_KURUM_PERSONEL1];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_KURUM_TANIM]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_KURUM_TANIM];
GO
IF OBJECT_ID(N'[dbo].[FK_STAJYER_TANIM_Uni]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[STAJYER_TANIM] DROP CONSTRAINT [FK_STAJYER_TANIM_Uni];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Bolumler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bolumler];
GO
IF OBJECT_ID(N'[dbo].[Event]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Event];
GO
IF OBJECT_ID(N'[dbo].[GUNLUK_CALISMA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GUNLUK_CALISMA];
GO
IF OBJECT_ID(N'[dbo].[Kullanicilar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[KURUM_DEPARTMAN]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KURUM_DEPARTMAN];
GO
IF OBJECT_ID(N'[dbo].[KURUM_PERSONEL]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KURUM_PERSONEL];
GO
IF OBJECT_ID(N'[dbo].[KURUM_TANIM]', 'U') IS NOT NULL
    DROP TABLE [dbo].[KURUM_TANIM];
GO
IF OBJECT_ID(N'[dbo].[MesajBilgileri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MesajBilgileri];
GO
IF OBJECT_ID(N'[dbo].[STAJYER_TANIM]', 'U') IS NOT NULL
    DROP TABLE [dbo].[STAJYER_TANIM];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Uni]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Uni];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Bolumler'
CREATE TABLE [dbo].[Bolumler] (
    [BolumId] int IDENTITY(1,1) NOT NULL,
    [BolumName] varchar(50)  NULL
);
GO

-- Creating table 'Event'
CREATE TABLE [dbo].[Event] (
    [EventID] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(100)  NOT NULL,
    [Description] nvarchar(300)  NULL,
    [Start] datetime  NOT NULL,
    [Son] datetime  NULL,
    [ThemeColor] nvarchar(10)  NULL,
    [IsFullDay] bit  NOT NULL,
    [stajerler] int  NULL
);
GO

-- Creating table 'GUNLUK_CALISMA'
CREATE TABLE [dbo].[GUNLUK_CALISMA] (
    [PK_GUNLUK_CALISMA] int IDENTITY(1,1) NOT NULL,
    [FK_STAJYER_TANIM] int  NOT NULL,
    [TARIH] datetime  NOT NULL,
    [ACIKLAMA] varchar(4000)  NULL,
    [kullaniciadi] int  NULL
);
GO

-- Creating table 'Kullanicilar'
CREATE TABLE [dbo].[Kullanicilar] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [KullaniciAdi] varchar(50)  NOT NULL,
    [AdiSoyadi] varchar(50)  NOT NULL,
    [Email] varchar(100)  NOT NULL,
    [Sifre] varchar(100)  NOT NULL,
    [Rol] varchar(50)  NOT NULL,
    [KayitTarihi] datetime  NOT NULL,
    [SifreKontrol] varchar(50)  NULL,
    [CalisanKurumTanim] int  NULL,
    [CalisanDepartman] int  NULL,
    [CalisanPersonel] int  NULL,
    [rememberme] bit  NULL
);
GO

-- Creating table 'KURUM_DEPARTMAN'
CREATE TABLE [dbo].[KURUM_DEPARTMAN] (
    [PK_KURUM_DEPARTMAN] int IDENTITY(1,1) NOT NULL,
    [FK_KURUM_TANIM] int  NOT NULL,
    [ADI] varchar(500)  NOT NULL,
    [AKTIF] int  NULL
);
GO

-- Creating table 'KURUM_PERSONEL'
CREATE TABLE [dbo].[KURUM_PERSONEL] (
    [PK_KURUM_PERSONEL] int IDENTITY(1,1) NOT NULL,
    [FK_KURUM_DEPARTMAN] int  NOT NULL,
    [ADI] varchar(50)  NOT NULL,
    [SOYADI] varchar(50)  NOT NULL,
    [UNVAN] varchar(100)  NULL,
    [GSM] varchar(100)  NULL,
    [EMAIL] varchar(200)  NULL,
    [FK_KURUM_TANIM] int  NULL
);
GO

-- Creating table 'KURUM_TANIM'
CREATE TABLE [dbo].[KURUM_TANIM] (
    [PK_KURUM_TANIM] int IDENTITY(1,1) NOT NULL,
    [FIRMA_ADI] varchar(250)  NOT NULL,
    [FIRMA_YEKTILISI] varchar(150)  NOT NULL,
    [FIRMA_ADRESİ] varchar(1000)  NOT NULL,
    [TELEFON] varchar(150)  NOT NULL,
    [EMAIL] varchar(100)  NULL,
    [Calisanlar] int  NULL
);
GO

-- Creating table 'MesajBilgileri'
CREATE TABLE [dbo].[MesajBilgileri] (
    [MesajBilgileriId] int IDENTITY(1,1) NOT NULL,
    [GönderenMail] varchar(50)  NULL,
    [AlıcıMail] varchar(50)  NULL,
    [Konu] varchar(100)  NULL,
    [MesajIcerigi] varchar(500)  NULL,
    [MesajTarihi] datetime  NULL,
    [SilindiMi] bit  NULL,
    [Dosyalar] varbinary(max)  NULL
);
GO

-- Creating table 'STAJYER_TANIM'
CREATE TABLE [dbo].[STAJYER_TANIM] (
    [PK_STAJYER_TANIM] int IDENTITY(1,1) NOT NULL,
    [ADI] varchar(50)  NOT NULL,
    [SOYADI] varchar(50)  NOT NULL,
    [UNIVERSITE] int  NULL,
    [UNIV_NO] varchar(50)  NOT NULL,
    [BOLUMU] int  NULL,
    [SINIFI] varchar(500)  NULL,
    [EMAIL] varchar(100)  NULL,
    [TELEFON] varchar(100)  NULL,
    [STAJ_YILI] int  NULL,
    [STAJ_BAS_TARIHI] datetime  NULL,
    [STAJ_BIT_TARIHI] datetime  NULL,
    [KURUM_ST_SORUMLUSU] int  NOT NULL,
    [KURUM_ONAY_KISI] int  NOT NULL,
    [FK_STAJ_KURUM] int  NOT NULL,
    [FK_DEPARTMAN] int  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Uni'
CREATE TABLE [dbo].[Uni] (
    [UniId] int IDENTITY(1,1) NOT NULL,
    [UniName] varchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BolumId] in table 'Bolumler'
ALTER TABLE [dbo].[Bolumler]
ADD CONSTRAINT [PK_Bolumler]
    PRIMARY KEY CLUSTERED ([BolumId] ASC);
GO

-- Creating primary key on [EventID] in table 'Event'
ALTER TABLE [dbo].[Event]
ADD CONSTRAINT [PK_Event]
    PRIMARY KEY CLUSTERED ([EventID] ASC);
GO

-- Creating primary key on [PK_GUNLUK_CALISMA] in table 'GUNLUK_CALISMA'
ALTER TABLE [dbo].[GUNLUK_CALISMA]
ADD CONSTRAINT [PK_GUNLUK_CALISMA]
    PRIMARY KEY CLUSTERED ([PK_GUNLUK_CALISMA] ASC);
GO

-- Creating primary key on [Id] in table 'Kullanicilar'
ALTER TABLE [dbo].[Kullanicilar]
ADD CONSTRAINT [PK_Kullanicilar]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PK_KURUM_DEPARTMAN] in table 'KURUM_DEPARTMAN'
ALTER TABLE [dbo].[KURUM_DEPARTMAN]
ADD CONSTRAINT [PK_KURUM_DEPARTMAN]
    PRIMARY KEY CLUSTERED ([PK_KURUM_DEPARTMAN] ASC);
GO

-- Creating primary key on [PK_KURUM_PERSONEL] in table 'KURUM_PERSONEL'
ALTER TABLE [dbo].[KURUM_PERSONEL]
ADD CONSTRAINT [PK_KURUM_PERSONEL]
    PRIMARY KEY CLUSTERED ([PK_KURUM_PERSONEL] ASC);
GO

-- Creating primary key on [PK_KURUM_TANIM] in table 'KURUM_TANIM'
ALTER TABLE [dbo].[KURUM_TANIM]
ADD CONSTRAINT [PK_KURUM_TANIM]
    PRIMARY KEY CLUSTERED ([PK_KURUM_TANIM] ASC);
GO

-- Creating primary key on [MesajBilgileriId] in table 'MesajBilgileri'
ALTER TABLE [dbo].[MesajBilgileri]
ADD CONSTRAINT [PK_MesajBilgileri]
    PRIMARY KEY CLUSTERED ([MesajBilgileriId] ASC);
GO

-- Creating primary key on [PK_STAJYER_TANIM] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [PK_STAJYER_TANIM]
    PRIMARY KEY CLUSTERED ([PK_STAJYER_TANIM] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [UniId] in table 'Uni'
ALTER TABLE [dbo].[Uni]
ADD CONSTRAINT [PK_Uni]
    PRIMARY KEY CLUSTERED ([UniId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BOLUMU] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_Bolumler]
    FOREIGN KEY ([BOLUMU])
    REFERENCES [dbo].[Bolumler]
        ([BolumId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_Bolumler'
CREATE INDEX [IX_FK_STAJYER_TANIM_Bolumler]
ON [dbo].[STAJYER_TANIM]
    ([BOLUMU]);
GO

-- Creating foreign key on [kullaniciadi] in table 'GUNLUK_CALISMA'
ALTER TABLE [dbo].[GUNLUK_CALISMA]
ADD CONSTRAINT [FK_GUNLUK_CALISMA_Kullanicilar]
    FOREIGN KEY ([kullaniciadi])
    REFERENCES [dbo].[Kullanicilar]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GUNLUK_CALISMA_Kullanicilar'
CREATE INDEX [IX_FK_GUNLUK_CALISMA_Kullanicilar]
ON [dbo].[GUNLUK_CALISMA]
    ([kullaniciadi]);
GO

-- Creating foreign key on [FK_STAJYER_TANIM] in table 'GUNLUK_CALISMA'
ALTER TABLE [dbo].[GUNLUK_CALISMA]
ADD CONSTRAINT [FK_GUNLUK_CALISMA_STAJYER_TANIM]
    FOREIGN KEY ([FK_STAJYER_TANIM])
    REFERENCES [dbo].[STAJYER_TANIM]
        ([PK_STAJYER_TANIM])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GUNLUK_CALISMA_STAJYER_TANIM'
CREATE INDEX [IX_FK_GUNLUK_CALISMA_STAJYER_TANIM]
ON [dbo].[GUNLUK_CALISMA]
    ([FK_STAJYER_TANIM]);
GO

-- Creating foreign key on [CalisanDepartman] in table 'Kullanicilar'
ALTER TABLE [dbo].[Kullanicilar]
ADD CONSTRAINT [FK_Kullanicilar_KURUM_DEPARTMAN]
    FOREIGN KEY ([CalisanDepartman])
    REFERENCES [dbo].[KURUM_DEPARTMAN]
        ([PK_KURUM_DEPARTMAN])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Kullanicilar_KURUM_DEPARTMAN'
CREATE INDEX [IX_FK_Kullanicilar_KURUM_DEPARTMAN]
ON [dbo].[Kullanicilar]
    ([CalisanDepartman]);
GO

-- Creating foreign key on [CalisanPersonel] in table 'Kullanicilar'
ALTER TABLE [dbo].[Kullanicilar]
ADD CONSTRAINT [FK_Kullanicilar_KURUM_PERSONEL]
    FOREIGN KEY ([CalisanPersonel])
    REFERENCES [dbo].[KURUM_PERSONEL]
        ([PK_KURUM_PERSONEL])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Kullanicilar_KURUM_PERSONEL'
CREATE INDEX [IX_FK_Kullanicilar_KURUM_PERSONEL]
ON [dbo].[Kullanicilar]
    ([CalisanPersonel]);
GO

-- Creating foreign key on [CalisanKurumTanim] in table 'Kullanicilar'
ALTER TABLE [dbo].[Kullanicilar]
ADD CONSTRAINT [FK_Kullanicilar_KURUM_TANIM]
    FOREIGN KEY ([CalisanKurumTanim])
    REFERENCES [dbo].[KURUM_TANIM]
        ([PK_KURUM_TANIM])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Kullanicilar_KURUM_TANIM'
CREATE INDEX [IX_FK_Kullanicilar_KURUM_TANIM]
ON [dbo].[Kullanicilar]
    ([CalisanKurumTanim]);
GO

-- Creating foreign key on [FK_KURUM_TANIM] in table 'KURUM_DEPARTMAN'
ALTER TABLE [dbo].[KURUM_DEPARTMAN]
ADD CONSTRAINT [FK_KURUM_DEPARTMAN_KURUM_TANIM]
    FOREIGN KEY ([FK_KURUM_TANIM])
    REFERENCES [dbo].[KURUM_TANIM]
        ([PK_KURUM_TANIM])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KURUM_DEPARTMAN_KURUM_TANIM'
CREATE INDEX [IX_FK_KURUM_DEPARTMAN_KURUM_TANIM]
ON [dbo].[KURUM_DEPARTMAN]
    ([FK_KURUM_TANIM]);
GO

-- Creating foreign key on [FK_KURUM_DEPARTMAN] in table 'KURUM_PERSONEL'
ALTER TABLE [dbo].[KURUM_PERSONEL]
ADD CONSTRAINT [FK_KURUM_PERSONEL_KURUM_DEPARTMAN]
    FOREIGN KEY ([FK_KURUM_DEPARTMAN])
    REFERENCES [dbo].[KURUM_DEPARTMAN]
        ([PK_KURUM_DEPARTMAN])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KURUM_PERSONEL_KURUM_DEPARTMAN'
CREATE INDEX [IX_FK_KURUM_PERSONEL_KURUM_DEPARTMAN]
ON [dbo].[KURUM_PERSONEL]
    ([FK_KURUM_DEPARTMAN]);
GO

-- Creating foreign key on [FK_DEPARTMAN] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_KURUM_DEPARTMAN]
    FOREIGN KEY ([FK_DEPARTMAN])
    REFERENCES [dbo].[KURUM_DEPARTMAN]
        ([PK_KURUM_DEPARTMAN])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_KURUM_DEPARTMAN'
CREATE INDEX [IX_FK_STAJYER_TANIM_KURUM_DEPARTMAN]
ON [dbo].[STAJYER_TANIM]
    ([FK_DEPARTMAN]);
GO

-- Creating foreign key on [FK_KURUM_TANIM] in table 'KURUM_PERSONEL'
ALTER TABLE [dbo].[KURUM_PERSONEL]
ADD CONSTRAINT [FK_KURUM_PERSONEL_KURUM_TANIM]
    FOREIGN KEY ([FK_KURUM_TANIM])
    REFERENCES [dbo].[KURUM_TANIM]
        ([PK_KURUM_TANIM])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_KURUM_PERSONEL_KURUM_TANIM'
CREATE INDEX [IX_FK_KURUM_PERSONEL_KURUM_TANIM]
ON [dbo].[KURUM_PERSONEL]
    ([FK_KURUM_TANIM]);
GO

-- Creating foreign key on [KURUM_ST_SORUMLUSU] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_KURUM_PERSONEL]
    FOREIGN KEY ([KURUM_ST_SORUMLUSU])
    REFERENCES [dbo].[KURUM_PERSONEL]
        ([PK_KURUM_PERSONEL])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_KURUM_PERSONEL'
CREATE INDEX [IX_FK_STAJYER_TANIM_KURUM_PERSONEL]
ON [dbo].[STAJYER_TANIM]
    ([KURUM_ST_SORUMLUSU]);
GO

-- Creating foreign key on [KURUM_ONAY_KISI] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_KURUM_PERSONEL1]
    FOREIGN KEY ([KURUM_ONAY_KISI])
    REFERENCES [dbo].[KURUM_PERSONEL]
        ([PK_KURUM_PERSONEL])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_KURUM_PERSONEL1'
CREATE INDEX [IX_FK_STAJYER_TANIM_KURUM_PERSONEL1]
ON [dbo].[STAJYER_TANIM]
    ([KURUM_ONAY_KISI]);
GO

-- Creating foreign key on [FK_STAJ_KURUM] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_KURUM_TANIM]
    FOREIGN KEY ([FK_STAJ_KURUM])
    REFERENCES [dbo].[KURUM_TANIM]
        ([PK_KURUM_TANIM])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_KURUM_TANIM'
CREATE INDEX [IX_FK_STAJYER_TANIM_KURUM_TANIM]
ON [dbo].[STAJYER_TANIM]
    ([FK_STAJ_KURUM]);
GO

-- Creating foreign key on [UNIVERSITE] in table 'STAJYER_TANIM'
ALTER TABLE [dbo].[STAJYER_TANIM]
ADD CONSTRAINT [FK_STAJYER_TANIM_Uni]
    FOREIGN KEY ([UNIVERSITE])
    REFERENCES [dbo].[Uni]
        ([UniId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_STAJYER_TANIM_Uni'
CREATE INDEX [IX_FK_STAJYER_TANIM_Uni]
ON [dbo].[STAJYER_TANIM]
    ([UNIVERSITE]);
GO

-- Creating foreign key on [stajerler] in table 'Event'
ALTER TABLE [dbo].[Event]
ADD CONSTRAINT [FK_Event_Kullanicilar]
    FOREIGN KEY ([stajerler])
    REFERENCES [dbo].[Kullanicilar]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Event_Kullanicilar'
CREATE INDEX [IX_FK_Event_Kullanicilar]
ON [dbo].[Event]
    ([stajerler]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------