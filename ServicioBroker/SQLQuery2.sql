USE [bd_api]
GO
/****** Object:  Table [dbo].[buques]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[buques](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BUQUE] [varchar](50) NULL,
	[VIAJE] [varchar](11) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[clientes]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[clientes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CLIENTE] [varchar](110) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[dform_bodegac]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dform_bodegac](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Presentaciones] [varchar](70) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[dform_patiocontenedor]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dform_patiocontenedor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PRESENTACIONES] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[dform_patioferrocarril]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[dform_patioferrocarril](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Presentaciones] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MAQUINAS]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MAQUINAS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[productos]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[productos](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PRODUCTO] [varchar](110) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tabla_patio_contenedor]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tabla_patio_contenedor](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BUQUE] [varchar](50) NULL,
	[VIAJE] [varchar](11) NULL,
	[REGIMEN] [varchar](4) NULL,
	[FECHA_ENTRADA] [date] NULL,
	[PRESENTACION] [varchar](35) NULL,
	[INICIALES] [varchar](6) NULL,
	[NUMERO] [int] NULL,
	[PESO] [decimal](7, 0) NULL,
	[UNIDADES] [varchar](2) NULL,
	[PRODUCTO] [varchar](110) NULL,
	[CLIENTE] [varchar](110) NULL,
	[PEDIMENTO] [decimal](15, 0) NULL,
	[VALOR_COMERCIAL] [varchar](11) NULL,
	[FECHA_SALIDA] [date] NULL,
	[SESION_ENTRADA] [varchar](15) NULL,
	[SENSION_SALIDA] [varchar](15) NULL,
	[ESTADO] [tinyint] NULL,
	[AGENTE] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tabla_patio_ferrocarril]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tabla_patio_ferrocarril](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BUQUE] [varchar](45) NULL,
	[VIAJE] [varchar](7) NULL,
	[REGIMEN] [varchar](4) NULL,
	[FECHA_ENTRADA] [date] NULL,
	[PRESENTACION] [varchar](10) NULL,
	[INICIALES] [varchar](6) NULL,
	[NUMERO] [int] NULL,
	[PESO] [decimal](7, 0) NULL,
	[UNIDADES] [varchar](2) NULL,
	[PRODUCTO] [varchar](110) NULL,
	[CLIENTE] [varchar](110) NULL,
	[PEDIMENTO] [decimal](15, 0) NULL,
	[VALOR_COMERCIAL] [varchar](11) NULL,
	[FECHA_SALIDA] [date] NULL,
	[SESION_ENTRADA] [varchar](15) NULL,
	[SENSION_SALIDA] [varchar](15) NULL,
	[ESTADO] [tinyint] NULL,
	[AGENTE] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tabla_usuario]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tabla_usuario](
	[id_usuario] [smallint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[nombre_Usuario] [nvarchar](15) NOT NULL,
	[contraseña_Usuario] [nvarchar](20) NOT NULL,
	[tipo_Usuario] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_tabla_usuario] PRIMARY KEY CLUSTERED 
(
	[id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Temporal]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Temporal](
	[ID] [varchar](10) NULL,
	[BUQUE] [varchar](50) NULL,
	[INICIALES] [varchar](10) NULL,
	[NUMERO] [varchar](10) NULL,
	[VIAJE] [varchar](15) NULL,
	[FECHA_ENTRADA] [varchar](12) NULL,
	[REGIMEN] [varchar](15) NULL,
	[ESTADO] [varchar](1) NULL,
	[ALMACEN] [varchar](50) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TEMPORAL2]    Script Date: 08/02/2019 01:24:03 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TEMPORAL2](
	[BUQUE] [nchar](100) NULL,
	[VIAJE] [nchar](10) NULL,
	[ALMACEN] [nchar](20) NULL,
	[REGIMEN] [nchar](10) NULL
) ON [PRIMARY]

GO
