IF EXISTS(SELECT 1 FROM master.dbo.sysdatabases
			WHERE name = 'sw_planets_db')
BEGIN
	DROP DATABASE sw_planets_db
	print '' print '*** dropping database sw_planets_db ***'
END
GO

print '' print '*** creating database sw_planets_db ***'
GO
CREATE DATABASE sw_planets_db
GO

print '' print '*** using sw_planets_db ***'
GO
USE [sw_planets_db]
GO

/* User table */
print '' print '*** creating user table'
GO
CREATE TABLE [dbo].[User] (
	[UserID]	    [int] IDENTITY(100000,1)	NOT NULL,
	[UserName]		[nvarchar](100)				NOT NUll,
	[PasswordHash]	[nvarchar](100)				NOT NULL DEFAULT
		'9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[FirstName]		[nvarchar](50)				NOT NUll,
	[FamilyName]	[nvarchar](100)				NOT NUll,
	[Active]		[bit]						NOT NULL DEFAULT 1,
	CONSTRAINT [pk_UserID] PRIMARY KEY([UserID]),
	CONSTRAINT [ak_UserName] UNIQUE([UserName])
)
GO
/* User test records */
print '' print '*** inserting user test records'
GO
INSERT INTO [dbo].[User]
		([UserName], [PasswordHash], [FirstName], [FamilyName])
    VALUES
		('admin@sw.com', 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342', 'admin', 'user'), -- P@ssw0rd
		('trustedUser@sw.com', 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342',  'trusted', 'user'), 
		('newUser@sw.com', 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342', 'new', 'user') 
GO


/* UserRole table and data */
print '' print '*** creating Role table'
GO
CREATE TABLE [dbo].[Role] (
	[RoleID]		[nvarchar](50)			NOT NULL,
	[Description]	[nvarchar](250)			NOT NULL,
	CONSTRAINT [pk_RoleID] PRIMARY KEY ([RoleID])
)
GO
print '' print '*** inserting sample Role records'
GO
INSERT INTO [dbo].[Role]
		([RoleID], [Description])
	VALUES
		('Admin', 'administers user accounts and assigns roles'),
		('Trusted User', 'can create and edit new planet records'),
		('New User', 'can view planet records')
GO

/* UserRole join table to join User and UserRole */

print '' print '*** creating UserRole table'
GO
CREATE TABLE [dbo].[UserRole] (
	[UserID]		[int] 					NOT NULL,
	[UserName]		[nvarchar](100)			NOT NULL,	
	[RoleID]		[nvarchar](50)			NOT NULL,	

	CONSTRAINT [fk_UserRole_UserID] FOREIGN KEY ([UserID])
		REFERENCES [dbo].[User]([UserID]),	
	
	CONSTRAINT [fk_UserRole_UserName] FOREIGN KEY ([UserName])
        REFERENCES [dbo].[User]([UserName]),

	CONSTRAINT [fk_UserRole_RoleID] FOREIGN KEY ([RoleID])
		REFERENCES [dbo].[Role]([RoleID]),

	CONSTRAINT [pk_UserRole] PRIMARY KEY ([UserID], [RoleID])
)
GO

print '' print '*** inserting sample UserRole records'
GO
INSERT INTO [dbo].[UserRole]
		([UserID], [UserName], [RoleID])
	VALUES
		(100000, 'admin@sw.com', 'Admin'),
		(100001, 'trustedUser@sw.com', 'Trusted User'),
		(100002, 'newUser@sw.com', 'New User')
GO






/* login-related stored procedures */

print '' print '*** creating sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
(
	@UserName				[nvarchar](100),
	@PasswordHash		[nvarchar](100)
)
AS
	BEGIN
		SELECT 	COUNT([UserID]) AS 'Authenticated'
		 FROM	[User]
		WHERE	@UserName = [UserName]
		  AND	@PasswordHash = [PasswordHash]
		  AND	[Active] = 1
	END
GO

print '' print '*** creating sp_select_user_by_username'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_username]
(
	@UserName				[nvarchar](50)			
)
AS
	BEGIN
		SELECT 	[UserID], [UserName], [FirstName], [FamilyName], [Active]
		FROM	[User]
		WHERE	@UserName = [UserName]
	END
GO

print '' print '*** creating sp_select_roles_by_userID'
GO
CREATE PROCEDURE [dbo].[sp_select_roles_by_userID]
(
	@UserID			[int]			
)
AS
	BEGIN
		SELECT 	[RoleID]
		FROM	[UserRole]
		WHERE	@UserID = [UserID]
	END
GO





/* Planet Stuff */

/*  */
print '' print '*** creating Species table ***'
GO
CREATE TABLE [dbo].[Species] (
	[SpeciesID]	    	 [nvarchar](50)	    			NOT NULL,
	[ArticleLink]	     [nvarchar](250)				NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Species',
	CONSTRAINT [pk_SpeciesID]     		PRIMARY KEY([SpeciesID])
)
GO

print '' print '*** creating Region table ***'
GO
CREATE TABLE [dbo].[Region] (
	[RegionID]	[nvarchar](50)			    NOT NULL,
	[ArticleLink]	     [nvarchar](250)    NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Category:Regions',
	CONSTRAINT [pk_RegionID] PRIMARY KEY ([RegionID])
)
GO

print '' print '*** creating Terrain table ***'
GO
CREATE TABLE [dbo].[Terrain] (
	[TerrainType]           [nvarchar](50) 		        NOT NULL,
	[TerrainDescription]	[nvarchar](500)			    NOT NULL DEFAULT 'no description',
	CONSTRAINT [pk_TerrainType] PRIMARY KEY ([TerrainType])
)
GO

print '' print '*** creating Sector table ***'
GO
CREATE TABLE [dbo].[Sector] (
	[SectorID]           [nvarchar](50)		    		NOT NULL,
	[RegionID]		     [nvarchar](50)       		    NOT NULL,
	[ArticleLink]	     [nvarchar](250)				NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Sector',
    CONSTRAINT [fk_sector_regionID] FOREIGN KEY ([RegionID])
        REFERENCES  [dbo].[Region]([RegionID]),
    CONSTRAINT [pk_SectorID] PRIMARY KEY ([SectorID])
)
GO

print '' print '*** creating PlanetarySystem table ***'
GO
CREATE TABLE [dbo].[PlanetarySystem] (
	[SystemID]           [nvarchar](50) 		    	NOT NULL,
	[SectorID]		     [nvarchar](50)        		    NOT NULL,
	[ArticleLink]	     [nvarchar](250)				NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Star_system',
    CONSTRAINT [pk_SystemID] PRIMARY KEY ([SystemID]),
    CONSTRAINT [fk_PlanetarySystem_SectorID] FOREIGN KEY ([SectorID])
        REFERENCES  [dbo].[Sector]([SectorID])
)
GO

print '' print '*** creating Planet table ***'
GO
CREATE TABLE [dbo].[Planet] (
	[PlanetID]           [nvarchar](50)  				NOT NULL,
	[SystemID]		     [nvarchar](50)        		    NOT NULL,
	[GridNumber]    	 [nvarchar](5)                  NOT NULL DEFAULT 'A-1',
	[ArticleLink]	     [nvarchar](250)				NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Planet',
	[PlanetCoordinateX]  [decimal](10, 6)				NOT NULL,
	[PlanetCoordinateY]  [decimal](10, 6)				NOT NULL,
    CONSTRAINT [pk_PlanetID_MVC] PRIMARY KEY ([PlanetID]),
    CONSTRAINT [fk_Planet_PlanetarySystemID_MVC] FOREIGN KEY ([SystemID])
        REFERENCES  [dbo].[PlanetarySystem]([SystemID])
)
GO

print '' print '*** creating PlanetMVC table ***'
GO
CREATE TABLE [dbo].[PlanetMVC] (
	[PlanetID]           [nvarchar](50)  				NOT NULL,
	[SystemID]		     [nvarchar](50)        		    NOT NULL,
	[GridNumber]    	 [nvarchar](5)                  NOT NULL DEFAULT 'A-1',
	[ArticleLink]	     [nvarchar](250)				NOT NULL DEFAULT 'https://starwars.fandom.com/wiki/Planet',
	[PlanetCoordinateX]  [decimal](10, 6)				NOT NULL,
	[PlanetCoordinateY]  [decimal](10, 6)				NOT NULL,
    CONSTRAINT [pk_PlanetID] PRIMARY KEY ([PlanetID]),
    CONSTRAINT [fk_Planet_PlanetarySystemID] FOREIGN KEY ([SystemID])
        REFERENCES  [dbo].[PlanetarySystem]([SystemID])
)
GO

print '' print '*** inserting sample region records'
GO
INSERT INTO [dbo].[Region]
		([RegionID], [ArticleLink])
	VALUES
		('Colonies', 					'https://starwars.fandom.com/wiki/Colonies'),
		('Core Worlds', 				'https://starwars.fandom.com/wiki/Core_Worlds'),
		('Deep Core', 					'https://starwars.fandom.com/wiki/Deep_Core'),
		('Expansion Region', 			'https://starwars.fandom.com/wiki/Expansion_Region'),
		('Inner Rim Territories', 		'https://starwars.fandom.com/wiki/Inner_Rim_Territories'),
		('Mid Rim Territories', 		'https://starwars.fandom.com/wiki/Mid_Rim_Territories'),
		('Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Outer_Rim_Territories'),
		('Unknown Regions', 			'https://starwars.fandom.com/wiki/Unknown_Regions'),
		('Western Reaches', 			'https://starwars.fandom.com/wiki/Western_Reaches'),
		('Wild Space', 					'https://starwars.fandom.com/wiki/Wild_Space'),
		('Extragalactic', 				'https://starwars.fandom.com/wiki/Extragalactic'),
		('Hutt Space',					'https://starwars.fandom.com/wiki/Hutt_Space')
GO

print '' print '*** inserting sample sector records'
GO
INSERT INTO [dbo].[Sector]
		([SectorID], [RegionID], [ArticleLink])
	VALUES
		('Corusca', 			'Core Worlds', 					'https://starwars.fandom.com/wiki/Corusca_sector'),
		('Alderaan', 			'Core Worlds', 					'https://starwars.fandom.com/wiki/Alderaan_sector'),
		('Cademimu',			'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Cademimu_sector'),
		('Gordian Reach', 		'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Gordian_Reach'),
		('Corporate', 			'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Corporate_Sector'),
		('Mytaranor', 			'Mid Rim Territories', 			'https://starwars.fandom.com/wiki/Mytaranor_sector'),
		('Abrion', 				'Extragalactic', 				'https://starwars.fandom.com/wiki/Abrion_sector'),
		('Bryx', 				'Mid Rim Territories', 			'https://starwars.fandom.com/wiki/Bryx'),
		('Ombakond', 			'Expansion Region', 			'https://starwars.fandom.com/wiki/Ombakond_sector'),
		('Chommell', 			'Mid Rim Territories', 			'https://starwars.fandom.com/wiki/Chommell_sector'),
		('Sanbra', 				'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Sanbra_sector'),
		('Arkanis', 			'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Arkanis'),
		('Sector 5', 			'Deep Core', 					'https://starwars.fandom.com/wiki/Sector_5'),
		('Kuat', 				'Core Worlds', 					'https://starwars.fandom.com/wiki/Kuat_sector'),
		('Corellian', 			'Core Worlds', 					'https://starwars.fandom.com/wiki/Corellian_sector'),
		('Hosnian', 			'Core Worlds', 					'https://starwars.fandom.com/wiki/Hosnian_Prime'),
		('Jakku', 				'Inner Rim Territories',		'https://starwars.fandom.com/wiki/Jakku%27s_sector'),
		('Bright Jewel', 		'Mid Rim Territories', 			'https://starwars.fandom.com/wiki/Bright_Jewel_sector'),
		('Gaulus', 				'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Gaulus_sector'),
		('Crait',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Crait'),
		('Sullust', 			'Outer Rim Territories', 		'https://starwars.fandom.com/wiki/Sullust_sector'),
		('Subterrel',			'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Subterrel_sector'),
		('Atravis',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Atravis_sector'),
		('Sluis',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Sluis_sector'),
		('Tarabba',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Tarabba_sector'),
		('Anoat',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Anoat_sector'),
		('Trilon',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Trilon_sector'),
		('Moddell',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Moddell_sector'),
		('Ahch-To',				'Unknown Regions',				'https://starwars.fandom.com/wiki/Ahch-To'),
		('Rakata',				'Unknown Regions',				'https://starwars.fandom.com/wiki/Rakata_Prime'),
		('Exegol',				'Unknown Regions',				'https://starwars.fandom.com/wiki/Exegol'),
		('7G',					'Unknown Regions',				'https://starwars.fandom.com/wiki/7G_sector'),
		('Mortis',				'Wild Space',					'https://starwars.fandom.com/wiki/Mortis'),
		('Albarrio',			'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Albarrio_sector'),
		('Raioballo',			'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Raioballo_sector'),
		('Taris',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Taris'),
		('Quelli',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Quelli_sector'),
		('Mandalore',			'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Mandalore_sector'),
		('Calamari',			'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Calamari_sector'),
		('Kessel',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Kessel_system'),
		('Bheriz',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Bheriz_sector'),
		('Nal Hutta',			'Hutt Space',					'https://starwars.fandom.com/wiki/Nal_Hutta'),
		('Tashtor',				'Mid Rim Territories',			'https://starwars.fandom.com/wiki/Tashtor_sector'),
		('Hetzal',				'Outer Rim Territories',		'https://starwars.fandom.com/wiki/Hetzal_system')



GO

print '' print '*** inserting sample system records'
GO
INSERT INTO [dbo].[PlanetarySystem]
		([SystemID], [SectorID], [ArticleLink])
	VALUES
		('Coruscant', 			'Corusca', 				'https://starwars.fandom.com/wiki/Coruscant_system'),
		('Alderaan', 			'Alderaan', 			'https://starwars.fandom.com/wiki/Alderaan_system'),
		('Ajan Kloss', 			'Cademimu', 			'https://starwars.fandom.com/wiki/Ajan_Kloss'),
		('Yavin', 				'Gordian Reach', 		'https://starwars.fandom.com/wiki/Yavin_system'),
		('Cantonica', 			'Corporate', 			'https://starwars.fandom.com/wiki/Cantonica_system'),
		('Kashyyyk', 			'Mytaranor', 			'https://starwars.fandom.com/wiki/Kashyyyk_system'),
		('Kamino', 				'Abrion', 				'https://starwars.fandom.com/wiki/Kamino_system'),
		('Kijimi', 				'Bryx', 				'https://starwars.fandom.com/wiki/Kijimi'),
		('Middian', 			'Ombakond', 			'https://starwars.fandom.com/wiki/Middian_system'),
		('Naboo', 				'Chommell', 			'https://starwars.fandom.com/wiki/Naboo_system'),
		('Ileenium', 			'Sanbra', 				'https://starwars.fandom.com/wiki/Ileenium_system'),
		('Tatoo', 				'Arkanis', 				'https://starwars.fandom.com/wiki/Tatoo_system'),
		('Geonosis', 			'Arkanis', 				'https://starwars.fandom.com/wiki/Geonosis_system'),
		('Tython', 				'Sector 5', 			'https://starwars.fandom.com/wiki/Tython_system'),
		('Kuat', 				'Kuat', 				'https://starwars.fandom.com/wiki/Kuat_system'),
		('Corellian', 			'Corellian', 			'https://starwars.fandom.com/wiki/Corellia_system'),
		('Hosnian', 			'Hosnian', 				'https://starwars.fandom.com/wiki/Hosnian_system'),
		('Jakku', 				'Jakku', 				'https://starwars.fandom.com/wiki/Jakku_system'),
		('Bright Jewel', 		'Bright Jewel', 		'https://starwars.fandom.com/wiki/Bright_Jewel_system'),
		('Scarif',				'Abrion', 				'https://starwars.fandom.com/wiki/Scarif_system'),
		('Ryloth',				'Gaulus',				'https://starwars.fandom.com/wiki/Ryloth_system'),
		('Crait',				'Crait',				'https://starwars.fandom.com/wiki/Crait_system'),
		('Sullust',				'Sullust',				'https://starwars.fandom.com/wiki/Sullust_system'),
		('Polis Massa',			'Subterrel',			'https://starwars.fandom.com/wiki/Polis_Massa_system'),
		('Mustafar',			'Atravis',				'https://starwars.fandom.com/wiki/Mustafar_system'),
		('Dagobah',				'Sluis',				'https://starwars.fandom.com/wiki/Dagobah_system'),
		('Utapau',				'Tarabba',				'https://starwars.fandom.com/wiki/Utapau_system'),
		('Bespin',				'Anoat',				'https://starwars.fandom.com/wiki/Bespin_system'),
		('Hoth',				'anoat',				'https://starwars.fandom.com/wiki/Hoth_system'),
		('Batuu',				'Trilon',				'https://starwars.fandom.com/wiki/Batuu_system'),
		('Endor',				'Moddell',				'https://starwars.fandom.com/wiki/Endor_system'),
		('Ahch-To',				'Ahch-To',				'https://starwars.fandom.com/wiki/Ahch-To_system'),
		('Rakata',				'Rakata',				'https://starwars.fandom.com/wiki/Rakata_Prime'),
		('Exegol',				'Exegol',				'https://starwars.fandom.com/wiki/Exegol_system'),
		('Ilum',				'7G',					'https://starwars.fandom.com/wiki/Ilum_system'),
		('Mortis',				'Mortis',				'https://starwars.fandom.com/wiki/Mortis'),
		('Mygeeto',				'Albarrio',				'https://starwars.fandom.com/wiki/Mygeeto_system'),
		('Dantooine',			'Raioballo',			'https://starwars.fandom.com/wiki/Dantooine_system'),
		('Taris',				'Taris',				'https://starwars.fandom.com/wiki/Taris_system'),
		('Dathomir',			'Quelli',				'https://starwars.fandom.com/wiki/Dathomir_system'),
		('Mandalore',			'Mandalore',			'https://starwars.fandom.com/wiki/Mandalore_system'),
		('Mon Calamari',		'Calamari',				'https://starwars.fandom.com/wiki/Mon_Calamari_system'),
		('Kessel',				'Kessel',				'https://starwars.fandom.com/wiki/Kessel_system'),
		('Eadu',				'Bheriz',				'https://starwars.fandom.com/wiki/Eadu_system'),
		('Nal Hutta',			'Nal Hutta',			'https://starwars.fandom.com/wiki/Nal_Hutta'),
		('Takodana',			'Tashtor',				'https://starwars.fandom.com/wiki/Takodana_system'),
		('Hetzal',				'Hetzal',				'https://starwars.fandom.com/wiki/Hetzal_system')
GO

print '' print '*** inserting sample planet records'
GO
INSERT INTO [dbo].[Planet]
		([PlanetID], [SystemID], [GridNumber], [ArticleLink], [PlanetCoordinateX], [PlanetCoordinatey])
	VALUES
		('Coruscant',  		'Coruscant', 	'L-9', 		'https://starwars.fandom.com/wiki/Coruscant',   	861.888547, 715.166944),
		('Alderaan',   		'Alderaan', 	'M-10', 	'https://starwars.fandom.com/wiki/Alderaan',    	969.843622, 735.215744),
		('Ajan Kloss', 		'Ajan Kloss', 	'L-5', 		'https://starwars.fandom.com/wiki/Ajan_Kloss', 		918.744449, 359.424889),
		('Yavin 4', 		'Yavin', 		'P-6', 		'https://starwars.fandom.com/wiki/Yavin_4',      	1264.17942, 389.552281),
		('Cantonica', 		'Cantonica', 	'R-5', 		'https://starwars.fandom.com/wiki/Cantonica',   	1515.78786, 204.693657),
		('Kashyyyk', 		'Kashyyyk', 	'P-9', 		'https://starwars.fandom.com/wiki/Kashyyyk', 		1273.42141, 697.618775),
		('Kamino', 			'Kamino', 		'S-15', 	'https://starwars.fandom.com/wiki/Kamino', 			1510.63259, 1212.08976),
		('Kijimi', 			'Kijimi', 		'R-8', 		'https://starwars.fandom.com/wiki/Kijimi', 			1407.43033, 566.690502),
		('Pasaana', 		'Middian', 		'P-12', 	'https://starwars.fandom.com/wiki/Pasaana', 		1190.24346, 1049.54821),
		('Naboo', 			'Naboo', 		'O-17', 	'https://starwars.fandom.com/wiki/Naboo', 			1131.71083, 1335.31635),
		('D''Qar', 			'Ileenium', 	'O-17', 	'https://starwars.fandom.com/wiki/D%27Qar',			1114.76718, 1420.03463),
		('Tatooine',		'Tatoo', 		'R-16', 	'https://starwars.fandom.com/wiki/Tatooine', 		1430.53531, 1332.23568),
		('Geonosis', 		'Geonosis', 	'R-16', 	'https://starwars.fandom.com/wiki/Geonosis', 		1430.53531, 1301.42904),
		('Tython', 			'Tython', 		'L-10', 	'https://starwars.fandom.com/wiki/Tython', 			870.684054, 813.868727),
		('Kuat', 			'Kuat', 		'M-10', 	'https://starwars.fandom.com/wiki/Kuat', 			1023.82116, 781.482205),
		('Corellia', 	  	'Corellian', 	'M-11',		'https://starwars.fandom.com/wiki/Corellia', 		991.434638, 901.775003),
		('Hosnian Prime', 	'Hosnian', 		'M-12',		'https://starwars.fandom.com/wiki/Hosnian_Prime', 	974.470268, 991.223495),
		('Jakku', 			'Jakku', 		'I-13',		'https://starwars.fandom.com/wiki/Jakku', 			624.387380, 1025.15223),
		('Ord Mantell', 	'Bright Jewel', 'L-7',		'https://starwars.fandom.com/wiki/Ord_Mantell', 	897.359500, 490.003500),
		('Scarif', 			'Scarif',		'S-15',		'https://starwars.fandom.com/wiki/Scarif', 			1542.00552, 1236.43573),
		('Ryloth',			'Ryloth',		'R-17',		'https://starwars.fandom.com/wiki/Ryloth',			1477.23247, 1389.11506),
		('Crait',			'Crait',		'N-17',		'https://starwars.fandom.com/wiki/Crait',			1053.12352, 1382.94619),
		('Sullust',			'Sullust',		'M-17',		'https://starwars.fandom.com/wiki/Sullust',			1009.94122, 1401.45278),
		('Polis Massa',		'Polis Massa', 	'K-20',		'https://starwars.fandom.com/wiki/Polis_Massa',		840.000000, 1679.05154),
		('Mustafar',		'Mustafar',		'L-19',		'https://starwars.fandom.com/wiki/Mustafar', 		901.986146, 1608.10964),
		('Dagobah',			'Dagobah',		'M-19',		'https://starwars.fandom.com/wiki/Dagobah',			1006.85671, 1563.38539),
		('Utapau',			'Utapau',		'N-19',		'https://starwars.fandom.com/wiki/Utapau',			1048.49660, 1598.85635),
		('Bespin',			'Bespin',		'K-18',		'https://starwars.fandom.com/wiki/Bespin',			768.319994, 1467.76804),
		('Hoth',			'Hoth',			'K-18',		'https://starwars.fandom.com/wiki/Hoth',			775.319949, 1500.00000),
		('Batuu',			'Batuu',		'G-15',		'https://starwars.fandom.com/wiki/Batuu',			524.143381, 1227.18244),
		('Endor',			'Endor',		'H-16',		'https://starwars.fandom.com/wiki/Endor',			570.409842, 1332.05309),
		('Ahch-To',			'Ahch-To',		'F-13',		'https://starwars.fandom.com/wiki/Ahch-To',			148.228300, 1166.79204),
		('Rakata Prime',	'Rakata',		'G-11',		'https://starwars.fandom.com/wiki/Rakata_Prime',	451.659259, 877.099558),
		('Exegol',			'Exegol',		'F-7',		'https://starwars.fandom.com/wiki/Exegol',			220.593414, 500.799008),
		('Ilum',			'Ilum',			'G-7',		'https://starwars.fandom.com/wiki/Ilum',			491.756858, 496.172362),
		('Mortis',			'Mortis',		'K-2',		'https://starwars.fandom.com/wiki/Mortis',			815.622085, 59.7254120),
		('Mygeeto',			'Mygeeto',		'K-5',		'https://starwars.fandom.com/wiki/Mygeeto', 		823.333162, 252.502333),
		('Dantooine',		'Dantooine',	'L-4',		'https://starwars.fandom.com/wiki/Dantooine',		860.346331, 209.320303),
		('Taris',			'Taris',		'N-7',		'https://starwars.fandom.com/wiki/Taris',			1097.84749, 448.363685),
		('Dathomir',		'Dathomir',		'O-6',		'https://starwars.fandom.com/wiki/Dathomir',		1150.28228, 431.399316),
		('Mandalore',		'Mandalore',	'O-7',		'https://starwars.fandom.com/wiki/Mandalore',		1182.66934, 488.461285),
		('Mon Cala',		'Mon Calamari',	'U-6',		'https://starwars.fandom.com/wiki/Mon_Cala',		1677.72047, 432.941530),
		('Kessel', 			'Kessel',		'T-10',		'https://starwars.fandom.com/wiki/Kessel',			1612.94743, 742.926820),
		('Eadu',			'Eadu',			'U-10',		'https://starwars.fandom.com/wiki/Eadu',			1649.96060, 753.622320),
		('Nal Hutta',		'Nal Hutta',	'S-12',		'https://starwars.fandom.com/wiki/Nal_Hutta',		1499.20904, 915.654942),
		('Takodana',		'Takodana',		'J-16',		'https://starwars.fandom.com/wiki/Takodana',		683.377118, 1267.28004),
		('Hetzal',			'Hetzal',		'Q-16',		'https://starwars.fandom.com/wiki/Hetzal_Prime',	1368.12071, 1257.28004)

GO

print '' print '*** inserting sample planet records'
GO
INSERT INTO [dbo].[PlanetMVC]
		([PlanetID], [SystemID], [GridNumber], [ArticleLink], [PlanetCoordinateX], [PlanetCoordinatey])
	VALUES
		('Coruscant',  		'Coruscant', 	'L-9', 		'https://starwars.fandom.com/wiki/Coruscant',   	350.6363525390625, 267.54544830322266),
		('Alderaan',   		'Alderaan', 	'M-10', 	'https://starwars.fandom.com/wiki/Alderaan',    	388.6363525390625, 276.54544830322266),
		('Ajan Kloss', 		'Ajan Kloss', 	'L-5', 		'https://starwars.fandom.com/wiki/Ajan_Kloss', 		372.6363525390625, 132.54544830322266),
		('Yavin 4', 		'Yavin', 		'P-6', 		'https://starwars.fandom.com/wiki/Yavin_4',      	507.6363525390625, 144.54544830322266),
		('Cantonica', 		'Cantonica', 	'R-5', 		'https://starwars.fandom.com/wiki/Cantonica',   	593.6363525390625, 79.54544830322266),
		('Kashyyyk', 		'Kashyyyk', 	'P-9', 		'https://starwars.fandom.com/wiki/Kashyyyk', 		505.6363525390625, 261.54544830322266),
		('Kamino', 			'Kamino', 		'S-15', 	'https://starwars.fandom.com/wiki/Kamino', 			590.6363525390625, 445.54544830322266),
		('Kijimi', 			'Kijimi', 		'R-8', 		'https://starwars.fandom.com/wiki/Kijimi', 			555.6363525390625, 211.54544830322266),
		('Pasaana', 		'Middian', 		'P-12', 	'https://starwars.fandom.com/wiki/Pasaana', 		469.6363525390625, 385.54544830322266),
		('Naboo', 			'Naboo', 		'O-17', 	'https://starwars.fandom.com/wiki/Naboo', 			452.6363525390625, 498.54544830322266),
		('D''Qar', 			'Ileenium', 	'O-17', 	'https://starwars.fandom.com/wiki/D%27Qar',			447.6363525390625, 528.5454483032227),
		('Tatooine',		'Tatoo', 		'R-16', 	'https://starwars.fandom.com/wiki/Tatooine', 		558.6363525390625, 495.54544830322266),
		('Geonosis', 		'Geonosis', 	'R-16', 	'https://starwars.fandom.com/wiki/Geonosis', 		559.6363525390625, 484.54544830322266),
		('Tython', 			'Tython', 		'L-10', 	'https://starwars.fandom.com/wiki/Tython', 			559.6363525390625, 484.54544830322266),
		('Kuat', 			'Kuat', 		'M-10', 	'https://starwars.fandom.com/wiki/Kuat', 			411.6363525390625, 303.54544830322266),
		('Corellia', 	  	'Corellian', 	'M-11',		'https://starwars.fandom.com/wiki/Corellia', 		393.6363525390625, 348.54544830322266),
		('Hosnian Prime', 	'Hosnian', 		'M-12',		'https://starwars.fandom.com/wiki/Hosnian_Prime', 	389.6363525390625, 375.54544830322266),
		('Jakku', 			'Jakku', 		'I-13',		'https://starwars.fandom.com/wiki/Jakku', 			267.6363525390625, 390.54544830322266),
		('Ord Mantell', 	'Bright Jewel', 'L-7',		'https://starwars.fandom.com/wiki/Ord_Mantell', 	360.6363525390625, 184.54544830322266),
		('Scarif', 			'Scarif',		'S-15',		'https://starwars.fandom.com/wiki/Scarif', 			602.6363525390625, 474.54544830322266),
		('Ryloth',			'Ryloth',		'R-17',		'https://starwars.fandom.com/wiki/Ryloth',			580.6363525390625, 510.54544830322266),
		('Crait',			'Crait',		'N-17',		'https://starwars.fandom.com/wiki/Crait',			425.6363525390625, 516.5454483032227),
		('Sullust',			'Sullust',		'M-17',		'https://starwars.fandom.com/wiki/Sullust',			425.6363525390625, 516.5454483032227),
		('Polis Massa',		'Polis Massa', 	'K-20',		'https://starwars.fandom.com/wiki/Polis_Massa',		339.6363525390625, 621.5454483032227),
		('Mustafar',		'Mustafar',		'L-19',		'https://starwars.fandom.com/wiki/Mustafar', 		374.6363525390625, 598.5454483032227),
		('Dagobah',			'Dagobah',		'M-19',		'https://starwars.fandom.com/wiki/Dagobah',			407.6363525390625, 588.5454483032227),
		('Utapau',			'Utapau',		'N-19',		'https://starwars.fandom.com/wiki/Utapau',			422.6363525390625, 599.5454483032227),
		('Bespin',			'Bespin',		'K-18',		'https://starwars.fandom.com/wiki/Bespin',			315.6363525390625, 547.5454483032227),
		('Hoth',			'Hoth',			'K-18',		'https://starwars.fandom.com/wiki/Hoth',			318.6363525390625, 560.5454483032227),
		('Batuu',			'Batuu',		'G-15',		'https://starwars.fandom.com/wiki/Batuu',			227.6363525390625, 458.54544830322266),
		('Endor',			'Endor',		'H-16',		'https://starwars.fandom.com/wiki/Endor',			249.6363525390625, 498.54544830322266),
		('Ahch-To',			'Ahch-To',		'F-13',		'https://starwars.fandom.com/wiki/Ahch-To',			92.6363525390625, 434.54544830322266),
		('Rakata Prime',	'Rakata',		'G-11',		'https://starwars.fandom.com/wiki/Rakata_Prime',	201.6363525390625, 331.54544830322266),
		('Exegol',			'Exegol',		'F-7',		'https://starwars.fandom.com/wiki/Exegol',			117.6363525390625, 172.54544830322266),
		('Ilum',			'Ilum',			'G-7',		'https://starwars.fandom.com/wiki/Ilum',			223.6363525390625, 173.54544830322266),
		('Mortis',			'Mortis',		'K-2',		'https://starwars.fandom.com/wiki/Mortis',			343.6363525390625, 13.545448303222656),
		('Mygeeto',			'Mygeeto',		'K-5',		'https://starwars.fandom.com/wiki/Mygeeto', 		332.6363525390625, 92.54544830322266),
		('Dantooine',		'Dantooine',	'L-4',		'https://starwars.fandom.com/wiki/Dantooine',		345.6363525390625, 68.54544830322266),
		('Taris',			'Taris',		'N-7',		'https://starwars.fandom.com/wiki/Taris',			439.6363525390625, 175.54544830322266),
		('Dathomir',		'Dathomir',		'O-6',		'https://starwars.fandom.com/wiki/Dathomir',		463.6363525390625, 168.54544830322266),
		('Mandalore',		'Mandalore',	'O-7',		'https://starwars.fandom.com/wiki/Mandalore',		478.6363525390625, 184.54544830322266),
		('Mon Cala',		'Mon Calamari',	'U-6',		'https://starwars.fandom.com/wiki/Mon_Cala',		651.6363525390625, 164.54544830322266),
		('Kessel', 			'Kessel',		'T-10',		'https://starwars.fandom.com/wiki/Kessel',			627.6363525390625, 280.54544830322266),
		('Eadu',			'Eadu',			'U-10',		'https://starwars.fandom.com/wiki/Eadu',			645.6363525390625, 282.54544830322266),
		('Nal Hutta',		'Nal Hutta',	'S-12',		'https://starwars.fandom.com/wiki/Nal_Hutta',		583.6363525390625, 343.54544830322266),
		('Takodana',		'Takodana',		'J-16',		'https://starwars.fandom.com/wiki/Takodana',		293.6363525390625, 471.54544830322266),
		('Hetzal',			'Hetzal',		'Q-16',		'https://starwars.fandom.com/wiki/Hetzal_Prime',	535.6363525390625, 458.54544830322266)

GO


print '' print '*** creating PlanetSpecies table ***'
GO
CREATE TABLE [dbo].[PlanetSpecies] (
	[SpeciesID]          [nvarchar](50)                 NOT NULL,
	[PlanetID]		     [nvarchar](50)                 NOT NULL,
    CONSTRAINT [fk_PlanetSpecies_SpeciesID] FOREIGN KEY ([SpeciesID])
        REFERENCES  [dbo].[Species]([SpeciesID]),
    CONSTRAINT [fk_PlanetSpecies_PlanetID] FOREIGN KEY ([PlanetID])
        REFERENCES  [dbo].[Planet]([PlanetID])    
)
GO

print '' print '*** creating PlanetTerrain table ***'
GO
CREATE TABLE [dbo].[PlanetTerrain] (
	[PlanetID]           [nvarchar](50)                 NOT NULL,
	[TerrainType]		 [nvarchar](50)                 NOT NULL,
    CONSTRAINT [fk_PlanetTerrain_PlanetID] FOREIGN KEY ([PlanetID])
        REFERENCES  [dbo].[Planet]([PlanetID]),
    CONSTRAINT [fk_PlanetTerrain_TerrainType] FOREIGN KEY ([TerrainType])
        REFERENCES  [dbo].[Terrain]([TerrainType])    
)
GO

print '' print '*** creating sp_select_planet_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_planet_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[Planet].[PlanetID], [Planet].[GridNumber], [Planet].[ArticleLink] AS 'Planet Article', [Planet].[PlanetCoordinateX], [Planet].[PlanetCoordinateY], [Planet].[SystemID],
		 [PlanetarySystem].[ArticleLink] AS 'System Article', [Sector].[SectorID] AS 'Sector Name', [Sector].[ArticleLink] AS 'Sector Article', [Region].[RegionID] AS 'Region Name', [Region].[ArticleLink] AS 'Region Article'

		FROM 	[dbo].[Planet]
		JOIN    [dbo].[PlanetarySystem]
		  ON 	([Planet].[SystemID] = [PlanetarySystem].[SystemID])
		JOIN 	[dbo].[Sector]
		  ON 	([PlanetarySystem].[SectorID] = [Sector].[SectorID])
		JOIN	[dbo].[Region]
		  ON	([Sector].[RegionID]) = [Region].[RegionID]
		WHERE   [PlanetID] LIKE '%' + @PlanetID + '%'
	END
GO

print '' print '*** creating sp_select_planet_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_planetMVC_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[PlanetMVC].[PlanetID], [PlanetMVC].[GridNumber], [PlanetMVC].[ArticleLink] AS 'Planet Article', [PlanetMVC].[PlanetCoordinateX], [PlanetMVC].[PlanetCoordinateY], [PlanetMVC].[SystemID],
		 [PlanetarySystem].[ArticleLink] AS 'System Article', [Sector].[SectorID] AS 'Sector Name', [Sector].[ArticleLink] AS 'Sector Article', [Region].[RegionID] AS 'Region Name', [Region].[ArticleLink] AS 'Region Article'

		FROM 	[dbo].[PlanetMVC]
		JOIN    [dbo].[PlanetarySystem]
		  ON 	([PlanetMVC].[SystemID] = [PlanetarySystem].[SystemID])
		JOIN 	[dbo].[Sector]
		  ON 	([PlanetarySystem].[SectorID] = [Sector].[SectorID])
		JOIN	[dbo].[Region]
		  ON	([Sector].[RegionID]) = [Region].[RegionID]
		WHERE   [PlanetID] LIKE '%' + @PlanetID + '%'
	END
GO

print '' print '*** creating sp_select_one_planet_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_one_planet_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[Planet].[PlanetID], [Planet].[GridNumber], [Planet].[ArticleLink] AS 'Planet Article', [Planet].[PlanetCoordinateX], [Planet].[PlanetCoordinateY], [Planet].[SystemID],
		 [PlanetarySystem].[ArticleLink] AS 'System Article', [Sector].[SectorID] AS 'Sector Name', [Sector].[ArticleLink] AS 'Sector Article', [Region].[RegionID] AS 'Region Name', [Region].[ArticleLink] AS 'Region Article'

		FROM 	[dbo].[Planet]
		JOIN    [dbo].[PlanetarySystem]
		  ON 	([Planet].[SystemID] = [PlanetarySystem].[SystemID])
		JOIN 	[dbo].[Sector]
		  ON 	([PlanetarySystem].[SectorID] = [Sector].[SectorID])
		JOIN	[dbo].[Region]
		  ON	([Sector].[RegionID]) = [Region].[RegionID]
		WHERE   [PlanetID] = @PlanetID
	END
GO

print '' print '*** creating sp_select_one_planetMVC_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_one_planetMVC_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[PlanetMVC].[PlanetID], [PlanetMVC].[GridNumber], [PlanetMVC].[ArticleLink] AS 'Planet Article', [PlanetMVC].[PlanetCoordinateX], [PlanetMVC].[PlanetCoordinateY], [PlanetMVC].[SystemID],
		 [PlanetarySystem].[ArticleLink] AS 'System Article', [Sector].[SectorID] AS 'Sector Name', [Sector].[ArticleLink] AS 'Sector Article', [Region].[RegionID] AS 'Region Name', [Region].[ArticleLink] AS 'Region Article'

		FROM 	[dbo].[PlanetMVC]
		JOIN    [dbo].[PlanetarySystem]
		  ON 	([PlanetMVC].[SystemID] = [PlanetarySystem].[SystemID])
		JOIN 	[dbo].[Sector]
		  ON 	([PlanetarySystem].[SectorID] = [Sector].[SectorID])
		JOIN	[dbo].[Region]
		  ON	([Sector].[RegionID]) = [Region].[RegionID]
		WHERE   [PlanetID] = @PlanetID
	END
GO

print '' print '*** creating sp_select_region_by_regionID'
GO
CREATE PROCEDURE [dbo].[sp_select_region_by_regionID]
(
	@RegionID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[RegionID]

		FROM 	[dbo].[Region]
		WHERE   [RegionID] = @RegionID
	END
GO

print '' print '*** creating sp_select_sector_by_sectorID'
GO
CREATE PROCEDURE [dbo].[sp_select_sector_by_sectorID]
(
	@SectorID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[SectorID]

		FROM 	[dbo].[Sector]
		WHERE   [SectorID] = @SectorID
	END
GO

print '' print '*** creating sp_select_system_by_systemID'
GO
CREATE PROCEDURE [dbo].[sp_select_system_by_systemID]
(
	@SystemID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[SystemID]

		FROM 	[dbo].[PlanetarySystem]
		WHERE    @SystemID = [SystemID]
	END
GO

print '' print '*** creating sp_select_all_systems'
GO
CREATE PROCEDURE [dbo].[sp_select_all_systems]
AS
	BEGIN
		Select 	[SystemID]
		FROM 	[dbo].[PlanetarySystem]
	END
GO

print '' print '*** creating sp_select_all_regions'
GO
CREATE PROCEDURE [dbo].[sp_select_all_regions]
AS
	BEGIN
		Select 	[RegionID]
		FROM 	[dbo].[Region]
	END
GO

print '' print '*** creating sp_select_all_sectors'
GO
CREATE PROCEDURE [dbo].[sp_select_all_sectors]
AS
	BEGIN
		Select 	[SectorID]
		FROM 	[dbo].[Sector]
	END
GO

print '' print '*** creating sp_select_planet_terrains_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_planet_terrains_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[TerrainType]
		FROM 	[dbo].[PlanetTerrain]
		WHERE   @PlanetID = [PlanetID]
	END
GO

print '' print '*** creating sp_select_planet_species_by_planetID'
GO
CREATE PROCEDURE [dbo].[sp_select_species_by_planetID]
(
	@PlanetID		[nvarchar](50)
)
AS
	BEGIN
		Select 	[SpeciesID]
		FROM 	[dbo].[PlanetSpecies]
		WHERE   @PlanetID = [PlanetID]
	END
GO

print '' print '*** creating sp_insert_planet_record'
GO
CREATE PROCEDURE [dbo].[sp_insert_planet_record]
(
	@PlanetID             [nvarchar](50),  
	@SystemID		      [nvarchar](50),
	@GridNumber    	 	  [nvarchar](5),   
	@ArticleLink	      [nvarchar](250),
	@PlanetCoordinateX 	  [decimal](10, 6),
	@PlanetCoordinateY	  [decimal](10, 6)
)
AS
	BEGIN
		INSERT INTO [dbo].[Planet]
		   (PlanetID,         
			SystemID,		  
			GridNumber,    	 
			ArticleLink,	  
			PlanetCoordinateX,
			PlanetCoordinateY)
		VALUES 
		   (@PlanetID,        
			@SystemID,	  
			@GridNumber,	 
			@ArticleLink,  
			@PlanetCoordinateX,
			@PlanetCoordinateY)
	END
GO

print '' print '*** creating sp_insert_planetMVC_record'
GO
CREATE PROCEDURE [dbo].[sp_insert_planetMVC_record]
(
	@PlanetID             [nvarchar](50),  
	@SystemID		      [nvarchar](50),
	@GridNumber    	 	  [nvarchar](5),   
	@ArticleLink	      [nvarchar](250),
	@PlanetCoordinateX 	  [decimal](10, 6),
	@PlanetCoordinateY	  [decimal](10, 6)
)
AS
	BEGIN
		INSERT INTO [dbo].[PlanetMVC]
		   (PlanetID,         
			SystemID,		  
			GridNumber,    	 
			ArticleLink,	  
			PlanetCoordinateX,
			PlanetCoordinateY)
		VALUES 
		   (@PlanetID,        
			@SystemID,	  
			@GridNumber,	 
			@ArticleLink,  
			@PlanetCoordinateX,
			@PlanetCoordinateY)
	END
GO

print '' print '*** creating sp_insert_region_record'
GO
CREATE PROCEDURE [dbo].[sp_insert_region_record]
(
	@RegionID             [nvarchar](50),  
	@ArticleLink	      [nvarchar](250)
)
AS
	BEGIN
		INSERT INTO [dbo].[Region]
		   (RegionID,         
			ArticleLink)
		VALUES 
		   (@RegionID,        
			@ArticleLink)
	END
GO

print '' print '*** creating sp_insert_sector_record'
GO
CREATE PROCEDURE [dbo].[sp_insert_sector_record]
(
	@SectorID             [nvarchar](50),
	@RegionID             [nvarchar](50),    
	@ArticleLink	      [nvarchar](250)
)
AS
	BEGIN
		INSERT INTO [dbo].[Sector]
		   (SectorID, 
		    RegionID,        
			ArticleLink)
		VALUES 
		   (@SectorID,   
			@RegionID,   
			@ArticleLink)
	END
GO

print '' print '*** creating sp_insert_system_record'
GO
CREATE PROCEDURE [dbo].[sp_insert_system_record]
(
	@SystemID             [nvarchar](50),
	@SectorID             [nvarchar](50),    
	@ArticleLink	      [nvarchar](250)
)
AS
	BEGIN
		INSERT INTO [dbo].[PlanetarySystem]
		   (SystemID, 
		    SectorID,        
			ArticleLink)
		VALUES 
		   (@SystemID,   
			@SectorID,   
			@ArticleLink)
	END
GO

print '' print '*** creating sp_delete_planet_record'
GO
CREATE PROCEDURE [dbo].[sp_delete_planet_record]
(
	@PlanetID             [nvarchar](50)
)
AS
	BEGIN
		DELETE FROM [dbo].[Planet]
		WHERE @PlanetID = [PlanetID]
	END
GO


print '' print '*** creating sp_delete_planetMVC_record'
GO
CREATE PROCEDURE [dbo].[sp_delete_planetMVC_record]
(
	@PlanetID             [nvarchar](50)
)
AS
	BEGIN
		DELETE FROM [dbo].[PlanetMVC]
		WHERE @PlanetID = [PlanetID]
	END
GO

print '' print '*** creating sp_update_planetMVC'
GO
CREATE PROCEDURE [dbo].[sp_update_planetMVC]
(
	@OldPlanetID              [nvarchar](50),  
	@OldSystemID		      [nvarchar](50),
	@OldGridNumber    	 	  [nvarchar](5),   
	@OldArticleLink	      	  [nvarchar](250),
	@OldPlanetCoordinateX 	  [decimal](10, 6),
	@OldPlanetCoordinateY	  [decimal](10, 6),

	@NewPlanetID              [nvarchar](50),
	@NewSystemID		      [nvarchar](50),
	@NewGridNumber    	 	  [nvarchar](5),   
	@NewArticleLink	      	  [nvarchar](250),
	@NewPlanetCoordinateX 	  [decimal](10, 6),
	@NewPlanetCoordinateY	  [decimal](10, 6)
	
)
AS
	BEGIN
		UPDATE [PlanetMVC] SET
		[PlanetID] = 					@NewPlanetID,
		[SystemID] = 					@NewSystemID,
		[GridNumber] = 					@NewGridNumber,
		[ArticleLink] = 				@NewArticleLink,
		[PlanetCoordinateX] = 			@NewPlanetCoordinateX,
		[PlanetCoordinateY]  = 			@NewPlanetCoordinateY
		
		WHERE @OldPlanetID = 			[PlanetID]
		  AND @OldSystemID = 			[SystemID]
		  AND @OldGridNumber = 			[GridNumber]
		  AND @OldArticleLink = 		[ArticleLink] 
		  AND @OldPlanetCoordinateX = 	[PlanetCoordinateX] 
		  AND @OldPlanetCoordinateY = 	[PlanetCoordinateY]
		RETURN 	@@ROWCOUNT
	END
GO

print '' print '*** creating sp_update_planet_coordinates'
GO
CREATE PROCEDURE [dbo].[sp_update_planet_coordinates]
(
	@PlanetID             [nvarchar](50),
	@PlanetCoordinateX 	  [decimal](10, 6),
	@PlanetCoordinateY 	  [decimal](10, 6)
)
AS
	BEGIN
		UPDATE [Planet] SET
		[PlanetCoordinateX] = @PlanetCoordinateX,
		[PlanetCoordinateY]  = @PlanetCoordinateY
		WHERE @PlanetID = [PlanetID]
	END
GO

print '' print '*** creating sp_update_planetMVC_coordinates'
GO
CREATE PROCEDURE [dbo].[sp_update_planetMVC_coordinates]
(
	@PlanetID             [nvarchar](50),
	@PlanetCoordinateX 	  [decimal](10, 6),
	@PlanetCoordinateY 	  [decimal](10, 6)
)
AS
	BEGIN
		UPDATE [PlanetMVC] SET
		[PlanetCoordinateX] = @PlanetCoordinateX,
		[PlanetCoordinateY]  = @PlanetCoordinateY
		WHERE @PlanetID = [PlanetID]
	END
GO

print '' print '*** Creating sp_insert_user'
GO
CREATE PROCEDURE [dbo].[sp_insert_user]
	(
		@FirstName			[nvarchar](50),
		@FamilyName			[nvarchar](100),
		@UserName			[nvarchar](100)
	)
AS
	BEGIN
		INSERT INTO [User]
			([FirstName], [FamilyName], [UserName])
		VALUES
			(@FirstName, @FamilyName, @UserName)
		
		SELECT SCOPE_IDENTITY()
	END
GO

print '' print '*** creating sp_select_all_user_roles'
GO
CREATE PROCEDURE [dbo].[sp_select_all_user_roles]
AS
	BEGIN
		SELECT [RoleID]
		  FROM [Role]
	END
GO

print '' PRINT '*** creating sp_add_user_roles';
GO

CREATE PROCEDURE [dbo].[sp_add_user_roles]
(
    @UserID     INT,
    @UserName   NVARCHAR(100),
    @RoleID     NVARCHAR(50)    
)
AS
BEGIN
    INSERT INTO [dbo].[UserRole] (UserID, UserName, RoleID)
    VALUES (@UserID, @UserName, @RoleID)
END
GO

PRINT ''
PRINT '*** creating sp_delete_user_role'
GO
CREATE PROCEDURE [dbo].[sp_delete_user_role]
(
	@UserName   [nvarchar](100),
	@RoleID     [nvarchar](50)
)
AS
BEGIN
	DELETE FROM [dbo].[UserRole]
	WHERE [UserName] = @UserName AND [RoleID] = @RoleID;
END
GO
		