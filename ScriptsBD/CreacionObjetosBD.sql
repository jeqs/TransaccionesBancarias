USE [StefaniniDB]
GO
/****** Object:  Table [dbo].[Banco]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banco](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
 CONSTRAINT [PK_Banco] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cliente]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Apellido] [varchar](256) NULL,
 CONSTRAINT [PK_Cliente] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CuentaBancaria]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CuentaBancaria](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BancoId] [int] NULL,
	[Nombre] [varchar](256) NULL,
	[ClienteId] [int] NULL,
	[TipoCuentaId] [int] NULL,
 CONSTRAINT [PK_CuentaBancaria] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoCuentaBancaria]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoCuentaBancaria](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Estado] [int] NULL,
 CONSTRAINT [PK_TipoCuentaBancaria] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoTransaccion]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoTransaccion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](256) NULL,
	[Estado] [int] NULL,
 CONSTRAINT [PK_TipoTransaccion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaccion]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaccion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CuentaBancariaId] [int] NULL,
	[TipoTransaccionId] [int] NULL,
	[Monto] [decimal](18, 0) NULL,
	[GMF] [decimal](18, 0) NULL,
 CONSTRAINT [PK_Transaccion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CuentaBancaria]  WITH CHECK ADD  CONSTRAINT [FK_CuentaBancaria_Banco] FOREIGN KEY([BancoId])
REFERENCES [dbo].[Banco] ([Id])
GO
ALTER TABLE [dbo].[CuentaBancaria] CHECK CONSTRAINT [FK_CuentaBancaria_Banco]
GO
ALTER TABLE [dbo].[CuentaBancaria]  WITH CHECK ADD  CONSTRAINT [FK_CuentaBancaria_Cliente] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Cliente] ([Id])
GO
ALTER TABLE [dbo].[CuentaBancaria] CHECK CONSTRAINT [FK_CuentaBancaria_Cliente]
GO
ALTER TABLE [dbo].[CuentaBancaria]  WITH CHECK ADD  CONSTRAINT [FK_CuentaBancaria_TipoCuentaBancaria] FOREIGN KEY([TipoCuentaId])
REFERENCES [dbo].[TipoCuentaBancaria] ([Id])
GO
ALTER TABLE [dbo].[CuentaBancaria] CHECK CONSTRAINT [FK_CuentaBancaria_TipoCuentaBancaria]
GO
ALTER TABLE [dbo].[Transaccion]  WITH CHECK ADD  CONSTRAINT [FK_Transaccion_CuentaBancaria] FOREIGN KEY([CuentaBancariaId])
REFERENCES [dbo].[CuentaBancaria] ([Id])
GO
ALTER TABLE [dbo].[Transaccion] CHECK CONSTRAINT [FK_Transaccion_CuentaBancaria]
GO
ALTER TABLE [dbo].[Transaccion]  WITH CHECK ADD  CONSTRAINT [FK_Transaccion_TipoTransaccion] FOREIGN KEY([TipoTransaccionId])
REFERENCES [dbo].[TipoTransaccion] ([Id])
GO
ALTER TABLE [dbo].[Transaccion] CHECK CONSTRAINT [FK_Transaccion_TipoTransaccion]
GO
/****** Object:  StoredProcedure [dbo].[ObtenerTransacciones]    Script Date: 25/01/2021 8:39:23 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ObtenerTransacciones]
AS
BEGIN
	Select Transaccion.Id, Transaccion.CuentaBancariaId, Transaccion.TipoTransaccionId, 
	Transaccion.Monto, 
	IsNull(Transaccion.GMF, 0) GMF, 
	TipoTransaccion.Nombre As [TipoTransaccion],
	CuentaBancaria.Nombre AS CuentaBancaria
	From Transaccion
	Inner Join TipoTransaccion On Transaccion.TipoTransaccionId = TipoTransaccion.Id
	Inner Join CuentaBancaria On Transaccion.CuentaBancariaId = CuentaBancaria.Id
END
GO
