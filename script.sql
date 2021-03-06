USE [FurnitureShop]
GO
/****** Object:  UserDefinedFunction [dbo].[DemDonHangTP]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[DemDonHangTP](@thang int,@Nam int)
returns int
as
begin

	declare @re int;
	select @re = count(MaDH)
	from DonHang
	where MONTH(DonHang.NgayDat) = @thang and YEAR(DonHang.NgayDat) = @nam and ThanhPho ='HCM'
	group by ThanhPho
	return @re
end
GO
/****** Object:  UserDefinedFunction [dbo].[demkhachhang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[demkhachhang] (@Thang int, @Nam int)
returns int
begin
	declare @re int;
	select @re = count(MaDH)
	from DonHang
	where MONTH(DonHang.NgayDat) = @thang and YEAR(DonHang.NgayDat) = @nam and ThanhPho !='HCM'
		group by ThanhPho


	return @re; 
end
GO
/****** Object:  UserDefinedFunction [dbo].[Signin]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Signin](@TK nvarchar(50), @MK nvarchar(50))
returns nvarchar(50)
as
begin
		DECLARE @Taikhoan nvarchar(50), 
		@Matkhau nvarchar(50), 
		@QTC nvarchar(50)
	

		SET @Taikhoan = @TK
		SET @Matkhau = 'MK'

		Select @MatKhau = TaiKhoan.MatKhau from TaiKhoan where TaiKhoan.TaiKhoan = @Taikhoan ;

		if (@Matkhau=@MK)
		
				SELECT @QTC = TaiKhoan.QuyenTruyCap  from TaiKhoan where TaiKhoan.TaiKhoan=@Taikhoan;
	
		Else
			Set @QTC ='Notfound';
		
		RETURN @QTC
END;
GO
/****** Object:  UserDefinedFunction [dbo].[TienShip]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create function [dbo].[TienShip](@MaDH int)
returns real
as
begin

declare @Tien real
declare @TP real
declare @MaDv real

Select @TP = DonHang.ThanhPho,@MaDv = DonHang.MaDvVanChuyen from DonHang where DonHang.MaDH = @MaDH

Select @Tien = Gia from DvVanChuyen where DvVanChuyen.MaDvVanChuyen = @Madv and DvVanChuyen.ViTri = @TP

return @Tien
end
GO
/****** Object:  UserDefinedFunction [dbo].[TinhDThu]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create function [dbo].[TinhDThu](@Thang int, @Nam int)
returns real
as
begin
	declare @re real;
	select @re = sum(TongTien)
	from DonHang
	where MONTH(DonHang.NgayDat) = @Thang and YEAR(DonHang.NgayDat) = @nam and DonHang.TrangThai = 'Done';
	return @re;
end
GO
/****** Object:  UserDefinedFunction [dbo].[tongtien]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


create function [dbo].[tongtien](@MaDH int)
returns real
as
begin
declare @Tong real
select @Tong = Sum(ChiTietDonHang.Gia*ChiTietDonHang.SoLuong)
from ChiTietDonHang
where(ChiTietDonHang.MaDH = @MaDH)
return @Tong
end
GO
/****** Object:  Table [dbo].[SanPham]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SanPham](
	[MaSp] [int] IDENTITY(1,1) NOT NULL,
	[tenSP] [nvarchar](50) NULL,
	[MoTa] [nvarchar](max) NULL,
	[MaLoai] [int] NULL,
	[TrangThai] [nvarchar](10) NULL,
	[gia] [real] NULL,
	[Giamgia] [int] NULL,
	[mausac] [nvarchar](30) NULL,
	[SoLuongDangDat] [int] NULL,
	[Hinhanh] [nvarchar](200) NULL,
	[LastModify] [datetime] NULL,
 CONSTRAINT [PK__SanPham__2725087C042B2BCA] PRIMARY KEY CLUSTERED 
(
	[MaSp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChitietDonHang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChitietDonHang](
	[MaDH] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NULL,
	[Gia] [real] NULL,
	[tenSP] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChitietDonHang] PRIMARY KEY CLUSTERED 
(
	[MaDH] ASC,
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DonHang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DonHang](
	[MaDH] [int] IDENTITY(1,1) NOT NULL,
	[TenKH] [nvarchar](50) NULL,
	[DiaChi] [nvarchar](50) NULL,
	[Quan] [nvarchar](30) NULL,
	[ThanhPho] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NULL,
	[sdt] [nvarchar](11) NULL,
	[MaDvVanChuyen] [int] NULL,
	[TongTien] [real] NULL,
	[TrangThai] [nvarchar](10) NOT NULL,
	[NgayDat] [date] NULL,
	[MaKH] [int] NULL,
 CONSTRAINT [PK__DonHang__27258661F9589F0A] PRIMARY KEY CLUSTERED 
(
	[MaDH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ChitietMuaHang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[ChitietMuaHang]
as
select Count(SoLuong) as TongSo
	from SanPham inner join ChitietDonHang on  SanPham.MaSp = ChitietDonHang.MaSP inner join DonHang on DonHang.MaDH = ChitietDonHang.MaDH
GO
/****** Object:  View [dbo].[ChitietMua]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[ChitietMua]
as
select ChitietDonHang.MaSP,ChitietDonHang.SoLuong,DonHang.NgayDat,SanPham.tenSP from SanPham inner join ChitietDonHang on  SanPham.MaSp = ChitietDonHang.MaSP inner join DonHang on DonHang.MaDH = ChitietDonHang.MaDH
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[TaiKhoan] [nvarchar](30) NOT NULL,
	[MatKhau] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NOT NULL,
	[QuyenTruyCap] [nvarchar](2) NULL,
 CONSTRAINT [PK_TaiKhoan_1] PRIMARY KEY CLUSTERED 
(
	[TaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[MaKH] [int] IDENTITY(1,1) NOT NULL,
	[TenKH] [nvarchar](50) NULL,
	[NamSinh] [date] NULL,
	[DiaChi] [nvarchar](50) NULL,
	[Quan] [nvarchar](30) NULL,
	[ThanhPho] [nvarchar](30) NULL,
	[Email] [nvarchar](100) NULL,
	[sdt] [nvarchar](11) NULL,
	[Taikhoan] [nvarchar](30) NOT NULL,
	[hinhanh] [nvarchar](200) NULL,
 CONSTRAINT [PK__KhachHan__2725CF1EFE2E7E55] PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[ThongTinKhachHangVaTaiKhoan]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ThongTinKhachHangVaTaiKhoan]
AS
SELECT dbo.KhachHang.MaKH, dbo.KhachHang.hinhanh, dbo.KhachHang.TenKH, dbo.TaiKhoan.TaiKhoan, dbo.TaiKhoan.Email, dbo.KhachHang.sdt, dbo.TaiKhoan.QuyenTruyCap
FROM     dbo.TaiKhoan INNER JOIN
                  dbo.KhachHang ON dbo.TaiKhoan.TaiKhoan = dbo.KhachHang.Taikhoan
WHERE  (dbo.TaiKhoan.QuyenTruyCap = 'Ad') OR
                  (dbo.TaiKhoan.QuyenTruyCap = 'Ua')
GO
/****** Object:  View [dbo].[thongtinNguoiDung]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[thongtinNguoiDung]
AS
SELECT dbo.KhachHang.MaKH, dbo.KhachHang.hinhanh, dbo.KhachHang.TenKH, dbo.TaiKhoan.TaiKhoan, dbo.TaiKhoan.Email, dbo.KhachHang.sdt, dbo.TaiKhoan.QuyenTruyCap
FROM     dbo.TaiKhoan INNER JOIN
                  dbo.KhachHang ON dbo.TaiKhoan.TaiKhoan = dbo.KhachHang.Taikhoan
WHERE  (dbo.TaiKhoan.QuyenTruyCap = 'Ur') OR
                  (dbo.TaiKhoan.QuyenTruyCap = 'Ua')
GO
/****** Object:  Table [dbo].[City]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[ID] [nvarchar](50) NULL,
	[TenTP] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DvVanChuyen]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DvVanChuyen](
	[MaDvVanChuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenDv] [nvarchar](30) NULL,
	[GiaNoiThanh] [real] NULL,
	[GiaNgoaiThanh] [real] NULL,
 CONSTRAINT [PK_DvVanChuyen] PRIMARY KEY CLUSTERED 
(
	[MaDvVanChuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhoHang]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhoHang](
	[MaSp] [int] NOT NULL,
	[tenSP] [nvarchar](50) NULL,
	[SoLuong] [int] NOT NULL,
 CONSTRAINT [PK__KhoHang__2725087CC5D321A3] PRIMARY KEY CLUSTERED 
(
	[MaSp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiSP]    Script Date: 11/24/2019 11:27:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiSP](
	[maloai] [int] IDENTITY(1,1) NOT NULL,
	[tenloai] [nvarchar](50) NULL,
 CONSTRAINT [PK__LoaiSP__734B3AEA0ED14E64] PRIMARY KEY CLUSTERED 
(
	[maloai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (5, 1032, 30, 333, NULL)
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (5, 1033, 30, 222, NULL)
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (14, 35, 30, 240, N'Chair')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (14, 1032, 30, 222, N'Cambrey Dining Chair With Fabric')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (14, 1033, 30, 345, N'Shell Arm Chair ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (19, 20, 5, 0, NULL)
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (19, 33, 5, 0, NULL)
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (23, 20, 4, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (24, 20, 5, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (25, 33, 2, 0, N'Dining white Wood leg Chair')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (26, 20, 4, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (27, 20, 4, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (28, 20, 15, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (29, 20, 9, 0, N'Zenning Sofa ')
INSERT [dbo].[ChitietDonHang] ([MaDH], [MaSP], [SoLuong], [Gia], [tenSP]) VALUES (30, 20, 6, 0, N'Zenning Sofa ')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'An Giang', N'An Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bà Rịa – Vũng Tàu', N'Bà Rịa – Vũng Tàu')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bắc Giang', N'Bắc Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bắc Kạn', N'Bắc Kạn')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bạc Liêu', N'Bạc Liêu')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bắc Ninh', N'Bắc Ninh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bến Tre', N'Bến Tre')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bình Định', N'Bình Định')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bình Dương', N'Bình Dương')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bình Phước', N'Bình Phước')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Bình Thuận', N'Bình Thuận')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Cà Mau', N'Cà Mau')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Cao Bằng', N'Cao Bằng')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Cần Thơ', N'Cần Thơ')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Đà Nẵng', N'Đà Nẵng')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Đắk Lắk', N'Đắk Lắk')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Đắk Nông', N'Đắk Nông')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Điện Biên', N'Điện Biên')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Đồng Nai', N'Đồng Nai')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Đồng Tháp', N'Đồng Tháp')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Gia Lai', N'Gia Lai')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hà Giang', N'Hà Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hà Nam', N'Hà Nam')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hà Nội', N'Hà Nội')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hà Tĩnh', N'Hà Tĩnh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hải Dương', N'Hải Dương')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hải Phòng', N'Hải Phòng')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hậu Giang', N'Hậu Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hòa Bình', N'Hòa Bình')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hưng Yên', N'Hưng Yên')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Hồ Chí Minh', N'Hồ Chí Minh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Khánh Hòa', N'Khánh Hòa')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Kiên Giang', N'Kiên Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Kon Tum', N'Kon Tum')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Lai Châu', N'Lai Châu')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Lâm Đồng', N'Lâm Đồng')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Lạng Sơn', N'Lạng Sơn')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Lào Cai', N'Lào Cai')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Long An', N'Long An')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Nam Định', N'Nam Định')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Nghệ An', N'Nghệ An')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Ninh Bình', N'Ninh Bình')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Ninh Thuận', N'Ninh Thuận')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Phú Thọ', N'Phú Thọ')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Phú Yên', N'Phú Yên')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Quảng Bình', N'Quảng Bình')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Quảng Nam', N'Quảng Nam')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Quảng Ngãi', N'Quảng Ngãi')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Quảng Ninh', N'Quảng Ninh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Quảng Trị', N'Quảng Trị')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Sóc Trăng', N'Sóc Trăng')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Sơn La', N'Sơn La')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Tây Ninh', N'Tây Ninh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Thái Bình', N'Thái Bình')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Thái Nguyên', N'Thái Nguyên')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Thanh Hóa', N'Thanh Hóa')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Thừa Thiên Huế', N'Thừa Thiên Huế')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Tiền Giang', N'Tiền Giang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Trà Vinh', N'Trà Vinh')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Tuyên Quang', N'Tuyên Quang')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Vĩnh Long', N'Vĩnh Long')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Vĩnh Phúc', N'Vĩnh Phúc')
INSERT [dbo].[City] ([ID], [TenTP]) VALUES (N'Yên Bái', N'Yên Bái')
SET IDENTITY_INSERT [dbo].[DonHang] ON 

INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (1, N'huy', NULL, NULL, N'HCM', NULL, NULL, 5, 1000, N'Done', CAST(N'2019-11-01' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (5, N'huy', NULL, NULL, N'HCM', NULL, NULL, 6, 2300, N'Done', CAST(N'2019-11-03' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (6, N'huy', NULL, NULL, N'HCM', NULL, NULL, 5, 13200, N'Done', CAST(N'2019-11-02' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (7, N'huy', NULL, NULL, N'HCM', NULL, NULL, 5, 2100, N'Done', CAST(N'2019-11-03' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (8, N'huy', NULL, NULL, N'HCM', NULL, NULL, 5, 300, N'Done', CAST(N'2019-11-11' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (10, N'huy', NULL, NULL, N'Hue', NULL, NULL, 6, 11000, N'Done', CAST(N'2019-11-10' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (11, N'huy', NULL, NULL, N'Hue', NULL, NULL, 5, 12000, N'Done', CAST(N'2019-11-01' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (14, N'huy', N'23/77', N'3', N'HCM', N'huy311099', N'0976960375', 4, 9900, N'Done', CAST(N'2019-11-22' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (17, N'huy', N'47/11', N'8', N'HCM', N'huy3199', N'0976980456', 4, 8700, N'No', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (18, N'huy', NULL, NULL, NULL, NULL, NULL, NULL, 9800, N'No', CAST(N'2019-10-31' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (19, N'Khoa', N'ThuDuc', N'9', N'HCM', N'huy311099@gmail.com', N'0976960375', 5, 40500, N'Done', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (20, N'Khoa', N'ThuDuc', N'9', N'Bình Định', N'huy311099@gmail.com', N'0976960375', 5, 40000, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (21, N'Khoa', N'ThuDuc', N'9', N'HCM', N'huy311099@gmail.com', N'0976960375', 4, 40300, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (22, N'Khoa', N'ThuDuc', N'9', N'Trà Vinh', N'huy311099@gmail.com', N'0976960375', 6, 80000, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (23, N'Khoa', N'ThuDuc', N'9', N'Quảng Trị', N'huy311099@gmail.com', N'0976960375', 4, 32000, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (24, N'Khoa', N'ThuDuc', N'9', N'HCM', N'huy311099@gmail.com', N'0976960375', 5, 400, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (25, N'Khoa', N'ThuDuc', N'9', N'HCM', N'huy311099@gmail.com', N'0976960375', 5, 200, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (26, N'Khoa', N'ThuDuc', N'quan 9', N'HCM', N'KhoaBinu@gmail.com', N'0976960375', 5, 320, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (27, N'Khoa', N'ThuDuc', N'quan 9', N'HCM', N'KhoaBinu@gmail.com', N'0976960375', 5, 320, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (28, N'Khoa', N'ThuDuc', N'quan 9', N'HCM', N'KhoaBinu@gmail.com', N'0976960375', 5, 1200, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (29, N'Khoa', N'ThuDuc', N'quan 9', N'HCM', N'KhoaBinu@gmail.com', N'0976960375', 6, 720, N'1', CAST(N'2019-11-24' AS Date), NULL)
INSERT [dbo].[DonHang] ([MaDH], [TenKH], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [MaDvVanChuyen], [TongTien], [TrangThai], [NgayDat], [MaKH]) VALUES (30, N'Khoa', N'ThuDuc', N'quan 9', N'HCM', N'KhoaBinu@gmail.com', N'0976960375', 5, 480, N'No', CAST(N'2019-11-24' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[DonHang] OFF
SET IDENTITY_INSERT [dbo].[DvVanChuyen] ON 

INSERT [dbo].[DvVanChuyen] ([MaDvVanChuyen], [TenDv], [GiaNoiThanh], [GiaNgoaiThanh]) VALUES (4, N'GRAP', 11000, 42000)
INSERT [dbo].[DvVanChuyen] ([MaDvVanChuyen], [TenDv], [GiaNoiThanh], [GiaNgoaiThanh]) VALUES (5, N'GIAO HANG NHANH', 12000, 39500)
INSERT [dbo].[DvVanChuyen] ([MaDvVanChuyen], [TenDv], [GiaNoiThanh], [GiaNgoaiThanh]) VALUES (6, N'GIAO HANG TIET KIEM', 10000, 38000)
INSERT [dbo].[DvVanChuyen] ([MaDvVanChuyen], [TenDv], [GiaNoiThanh], [GiaNgoaiThanh]) VALUES (8, N'Nhat Tin', 42000, 11000)
SET IDENTITY_INSERT [dbo].[DvVanChuyen] OFF
SET IDENTITY_INSERT [dbo].[KhachHang] ON 

INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [NamSinh], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [Taikhoan], [hinhanh]) VALUES (1, N'huy', CAST(N'1999-10-31' AS Date), N'47/11 TVH', N'quan 9', N'Ho Chi Minh', N'huy311099', N'0976960375', N'admin1', N'Images\2b16af9870514b938127701d1400a754.jpg')
INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [NamSinh], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [Taikhoan], [hinhanh]) VALUES (7, N'Nam', CAST(N'1999-10-01' AS Date), N'KTX D2', N'quan 9', N'Ho Chi Minh', N'PhuongNamsk', N'0976960375', N'Nam123', NULL)
INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [NamSinh], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [Taikhoan], [hinhanh]) VALUES (8, N'Khoa', CAST(N'1999-06-21' AS Date), N'T', N'quan 9', N'Ho Chi Minh', N'KhoaBinu@gmail.com', N'0976960375', N'huy123', NULL)
INSERT [dbo].[KhachHang] ([MaKH], [TenKH], [NamSinh], [DiaChi], [Quan], [ThanhPho], [Email], [sdt], [Taikhoan], [hinhanh]) VALUES (9, N'Khoa', CAST(N'1999-06-21' AS Date), N'ThuDuc', N'quan 9', N'Ho Chi Minh', N'Khoa21699', N'0933456789', N'disable', N'Images\883d839a5db946b59048995e79ec4460.jpg')
SET IDENTITY_INSERT [dbo].[KhachHang] OFF
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (20, N'Zenning Sofa ', 309)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (33, N'Dining white Wood leg Chair', 28)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (34, N'Chair', 60)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (35, N'Lule Carver Dining Chair', 30)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (36, N'Margot Dining Chairs,', 40)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (1032, N'Cambrey Dining Chair With Fabric', 50)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (1033, N'Shell Arm Chair ', 30)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (1034, N'Zen Dining Chair ', 40)
INSERT [dbo].[KhoHang] ([MaSp], [tenSP], [SoLuong]) VALUES (1035, N'Adolph Dining Chair With Fabric', 50)
SET IDENTITY_INSERT [dbo].[LoaiSP] ON 

INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (1, N'SOFA')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (2, N'Chair')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (3, N'Light')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (4, N'Frame')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (5, N'Picture')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (6, N'Desk')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (7, N'Wardrobe')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (8, N'Plant')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (9, N'BookShelf')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (11, N'Table')
INSERT [dbo].[LoaiSP] ([maloai], [tenloai]) VALUES (12, N'Desk')
SET IDENTITY_INSERT [dbo].[LoaiSP] OFF
SET IDENTITY_INSERT [dbo].[SanPham] ON 

INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (20, N'Zenning Sofa ', N'THIS IS A CHAIR', 1, N'1', 80, 0, N'Black', 0, N'Images\e6ab6df4ab0648c78dd9b5f3fa2c6fe3.jpg', CAST(N'2019-11-24T14:10:12.133' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (33, N'Dining white Wood leg Chair', N'Set of 2 Margot Dining Chairs, Vintage Gold velvet and Light Wood', 1, N'1', 100, 0, N'Yellow', 0, N'Images\7e257903fb9a41b096cf3c4b8dd6cda7.jpg', CAST(N'2019-11-11T13:42:29.183' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (34, N'Chair', N'Comfy, family-friendly dining chairs don’t always scream ‘sexy design’. ', 2, N'1', 100, 0, N'Yellow', 0, N'Images\c8ffd4a7975e469bbc0644fa8bfd8d48.jpg', CAST(N'2019-11-11T00:37:48.887' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (35, N'Lule Carver Dining Chair', N'our Lule chairs offer contemporary style whether you’re enjoying a meal alone or with guests.', 2, N'1', 100, 0, N'Black', 0, N'Images\bdc9f1ce2b6041fa8a26d2785cd3b177.jpg', CAST(N'2019-11-11T09:23:55.560' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (36, N'Margot Dining Chairs,', N'Margot Dining Chairs, Vintage Gold velvet and Light Wood', 1, N'1', 100, 0, N'Yellow', 0, N'Images\8497b60d294449a496c4a90d5416f64e.jpg', CAST(N'2019-11-18T13:22:34.137' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1032, N'Cambrey Dining Chair With Fabric', N'Each piece is crafted while taking care of quality, choosing only the premium solid-woods, so that you get the perfect furniture for your perfect home.', 1, N'1', 15, 0, N'Yellow', 0, N'Images\d21da966d85f4b3b98ec4bc4f2218c9e.jpg', CAST(N'2019-11-18T13:21:55.850' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1033, N'Shell Arm Chair ', N'special about this product and what makes it stand out from the crowd.', 2, N'0', 80, 0, N'Yellow', 0, N'Images\9bdf19dcdf6043c989475f5a5cfcb7b5.jpg', CAST(N'2019-11-24T12:16:37.910' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1034, N'Zen Dining Chair ', N'Material Velvet, Material Mango Wood, Finish Teak Finish', 2, N'1', 10, 0, N'Gray', 0, N'Images\fe3f6781b08b4d268ddc04e5f72be2f8.jpg', CAST(N'2019-11-11T13:45:35.073' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1035, N'Adolph Dining Chair With Fabric', N' With two rows of slots and strips running down the backrest, Adolph will make a bold statement with your dining.', 2, N'1', 30, 0, N'Gray', 0, N'Images\50411f606cb44e59bd2d92aaa3cf3761.jpg', CAST(N'2019-11-11T13:46:49.450' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1036, N'chair', N'this is a chair', 2, N'1', 223, 0, N'Blue', 0, N'Images\26d803d32a644a0896cd2f48a33f5a57.jpg', CAST(N'2019-11-24T12:25:30.650' AS DateTime))
INSERT [dbo].[SanPham] ([MaSp], [tenSP], [MoTa], [MaLoai], [TrangThai], [gia], [Giamgia], [mausac], [SoLuongDangDat], [Hinhanh], [LastModify]) VALUES (1037, N'white Steel Wood Dinning', N'11111', 2, N'1', 223, 0, N'Blue', 0, N'Images\c4a97dab86374b45a2cfccb4215ba8bd.webp', CAST(N'2019-11-24T13:03:57.257' AS DateTime))
SET IDENTITY_INSERT [dbo].[SanPham] OFF
INSERT [dbo].[TaiKhoan] ([TaiKhoan], [MatKhau], [Email], [QuyenTruyCap]) VALUES (N'admin1', N'123', N'huy311099@gmail.com', N'Ad')
INSERT [dbo].[TaiKhoan] ([TaiKhoan], [MatKhau], [Email], [QuyenTruyCap]) VALUES (N'disable', N'disable', N'disable', N'ds')
INSERT [dbo].[TaiKhoan] ([TaiKhoan], [MatKhau], [Email], [QuyenTruyCap]) VALUES (N'huy123', N'1234', N'huy3109@gmail.com', N'Ur')
INSERT [dbo].[TaiKhoan] ([TaiKhoan], [MatKhau], [Email], [QuyenTruyCap]) VALUES (N'Nam123', N'123', N'NamPhuongsk', N'Ur')
ALTER TABLE [dbo].[ChitietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChitietDonHang_DonHang] FOREIGN KEY([MaDH])
REFERENCES [dbo].[DonHang] ([MaDH])
GO
ALTER TABLE [dbo].[ChitietDonHang] CHECK CONSTRAINT [FK_ChitietDonHang_DonHang]
GO
ALTER TABLE [dbo].[ChitietDonHang]  WITH CHECK ADD  CONSTRAINT [FK_ChitietDonHang_SanPham] FOREIGN KEY([MaSP])
REFERENCES [dbo].[SanPham] ([MaSp])
GO
ALTER TABLE [dbo].[ChitietDonHang] CHECK CONSTRAINT [FK_ChitietDonHang_SanPham]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_DvVanChuyen] FOREIGN KEY([MaDvVanChuyen])
REFERENCES [dbo].[DvVanChuyen] ([MaDvVanChuyen])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_DvVanChuyen]
GO
ALTER TABLE [dbo].[DonHang]  WITH CHECK ADD  CONSTRAINT [FK_DonHang_KhachHang] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachHang] ([MaKH])
GO
ALTER TABLE [dbo].[DonHang] CHECK CONSTRAINT [FK_DonHang_KhachHang]
GO
ALTER TABLE [dbo].[KhachHang]  WITH CHECK ADD  CONSTRAINT [FK_KhachHang_TaiKhoan1] FOREIGN KEY([Taikhoan])
REFERENCES [dbo].[TaiKhoan] ([TaiKhoan])
GO
ALTER TABLE [dbo].[KhachHang] CHECK CONSTRAINT [FK_KhachHang_TaiKhoan1]
GO
ALTER TABLE [dbo].[KhoHang]  WITH CHECK ADD  CONSTRAINT [FK_KhoHang_SanPham] FOREIGN KEY([MaSp])
REFERENCES [dbo].[SanPham] ([MaSp])
GO
ALTER TABLE [dbo].[KhoHang] CHECK CONSTRAINT [FK_KhoHang_SanPham]
GO
ALTER TABLE [dbo].[SanPham]  WITH CHECK ADD  CONSTRAINT [FK_SanPham_LoaiSP] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[LoaiSP] ([maloai])
GO
ALTER TABLE [dbo].[SanPham] CHECK CONSTRAINT [FK_SanPham_LoaiSP]
GO
/****** Object:  StoredProcedure [dbo].[BoQuyenQT]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[BoQuyenQT] @username nvarchar(50)
as
begin
update TaiKhoan set QuyenTruyCap= 'Ur' where TaiKhoan = @username
end
GO
/****** Object:  StoredProcedure [dbo].[CapNhatCTTK]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CapNhatCTTK] @Ma int, @Ten nvarchar(50), @Email nvarchar(100), @Diachi nvarchar,@Quan nvarchar(50), @SDT nvarchar(20)
as
begin
	update KhachHang set
	TenKH=@Ten,
	Email=@Email,
	DiaChi=@Diachi,
	Quan=@Quan,
	sdt=@SDT
	where KhachHang.MaKH=@Ma
end
GO
/****** Object:  StoredProcedure [dbo].[CapNhatDvVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CapNhatDvVanChuyen] @MaDv int, @TenDv nvarchar(30),@GiaNoiThanh real,@GiaNgoaiThanh real
AS
BEGIN
	update DvVanChuyen set TenDv = @TenDv,GiaNoiThanh = @GiaNoiThanh,GiaNgoaiThanh = @GiaNgoaiThanh
	where MaDvVanChuyen = @MaDv
END
GO
/****** Object:  StoredProcedure [dbo].[CapNhatLoaiSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CapNhatLoaiSP] @MaLoai int, @TenLoai nvarchar(50)
as
begin
Update LoaiSP set tenloai = @TenLoai where maloai = @MaLoai
end
GO
/****** Object:  StoredProcedure [dbo].[CapNhatMK]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[CapNhatMK] @taikhoan nvarchar(30), @matkhau nvarchar(30)
as
begin
	update TaiKhoan set
	MatKhau = @matkhau
	where TaiKhoan = @taikhoan
end
GO
/****** Object:  StoredProcedure [dbo].[CapNhatSoLuong]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CapNhatSoLuong]
@Masp int,
@SoLuong int
AS
BEGIN
	Update KhoHang set SoLuong = @SoLuong where MaSP = @Masp
END
GO
/****** Object:  StoredProcedure [dbo].[CapNhatSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CapNhatSP]
@MaSP int,
@TenSP nvarchar(50),
@MoTa nvarchar(Max),
@MaLoai int,
@TrangThai int,
@Gia real,
@MauSac nvarchar(30),
@hinhanh nvarchar(200),
@Modify Datetime
AS 
BEGIN 
UPDATE SanPham
SET tenSP=@TenSP,MoTa=@MoTa,MaLoai=@MaLoai,TrangThai=@TrangThai,gia=@Gia,mausac=@MauSac,Hinhanh=@hinhanh,LastModify=@Modify
WHERE MaSp = @MaSP 
END 
GO
/****** Object:  StoredProcedure [dbo].[CapNhatTinhTrangDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CapNhatTinhTrangDonHang] @maDH int
as
BEGIN
	Update DonHang set TrangThai = 'Done' where MaDH = @maDH
END
GO
/****** Object:  StoredProcedure [dbo].[CapNhatTK]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[CapNhatTK] @taikhoan nvarchar(30), @matkhau nvarchar(30), @Email nvarchar(100),
@TenKH nvarchar(50), @NamSinh Date, @DiaChi nvarchar(50), @Quan nvarchar(30), @ThanhPho nvarchar(30),@sdt nvarchar(11),@hinhanh nvarchar(200)
as
begin
	update TaiKhoan set
	MatKhau = @matkhau,
	Email = @Email
	where TaiKhoan = @taikhoan

	update KhachHang set
	TenKH = @TenKH,
	NamSinh = @NamSinh,
	DiaChi = @DiaChi,
	Quan = @Quan,
	ThanhPho = @ThanhPho,
	sdt = @sdt,
	hinhanh = @hinhanh 
	where KhachHang.Taikhoan = @taikhoan

end
GO
/****** Object:  StoredProcedure [dbo].[CapQuyenQT]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[CapQuyenQT] @username nvarchar(50)
as
begin
update TaiKhoan set QuyenTruyCap= 'Ua' where TaiKhoan = @username
end
GO
/****** Object:  StoredProcedure [dbo].[DangNhap]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DangNhap] @TK nvarchar(30),@MK nvarchar(30),@QTC nvarchar(10) output
AS
BEGIN
	declare @MatKhau nvarchar(30) 
	set @MatKhau = 'MK'
	Select @MatKhau = TaiKhoan.MatKhau from TaiKhoan where TaiKhoan.TaiKhoan = @TK  
	if(	@MatKhau = @MK )
		select @QTC = QuyenTruyCap from TaiKhoan where TaiKhoan.TaiKhoan = @TK
	else
		select @QTC = 'NotFound' 
END
GO
/****** Object:  StoredProcedure [dbo].[DemDHNgoaiThanh]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DemDHNgoaiThanh] @Thang int, @Nam int
AS
BEGIN
	select dbo.demkhachhang (@Thang,@Nam);
END
GO
/****** Object:  StoredProcedure [dbo].[DemDHTP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DemDHTP] @Thang int, @Nam int
AS
BEGIN
	select dbo.demkhachhang (@Thang,@Nam);
END
GO
/****** Object:  StoredProcedure [dbo].[DemDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DemDonHang] @thang int, @Nam int	
AS
BEGIN
	select Count(MaDH) as SoDon 
	from DonHang
	where MONTH(DonHang.NgayDat) = @thang and YEAR(DonHang.NgayDat) = @nam
END
GO
/****** Object:  StoredProcedure [dbo].[DemNguoiDung]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DemNguoiDung] 
AS
BEGIN
	select count(Taikhoan.TaiKhoan)
	from KhachHang inner join TaiKhoan on KhachHang.Taikhoan = Taikhoan.TaiKhoan
	where Taikhoan.TaiKhoan != 'disable'
END
GO
/****** Object:  StoredProcedure [dbo].[LayChiTietDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayChiTietDonHang]
@MaDH int
AS
BEGIN
	Select* from DonHang where MaDH = @MaDH
END
GO
/****** Object:  StoredProcedure [dbo].[LayChiTietMuaHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayChiTietMuaHang]
@MaDH int
AS
BEGIN
select* from ChitietDonHang where MaDH = @maDH
END
GO
/****** Object:  StoredProcedure [dbo].[LayDonViVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[LayDonViVanChuyen]
@MaDv int
as
begin
select * from DvVanChuyen where MaDvVanChuyen = @MaDv
end
GO
/****** Object:  StoredProcedure [dbo].[LayDSDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayDSDonHang]
AS
BEGIN
select MaDH, tenKH, sdt, TongTien, NgayDat
from DonHang
where TrangThai = 'No'
END
GO
/****** Object:  StoredProcedure [dbo].[LayDSDonHangTheoNgay]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayDSDonHangTheoNgay] @thang int, @nam int
AS
BEGIN
	select MaDH, tenKH, sdt, TongTien, NgayDat
	from DonHang
	where TrangThai = 'Done' and MONTH(DonHang.NgayDat) = @thang and YEAR(DonHang.NgayDat) = @nam 
END
GO
/****** Object:  StoredProcedure [dbo].[LayDsDonViVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[LayDsDonViVanChuyen] 
as
begin
select* from DvVanChuyen
end
GO
/****** Object:  StoredProcedure [dbo].[LayDSLoaiHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[LayDSLoaiHang]
as
begin
select* from  LoaiSP
end
GO
/****** Object:  StoredProcedure [dbo].[LayDSSanPham]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[LayDSSanPham]
as
begin
select MaSp,Hinhanh,tenSP,gia,TrangThai from SanPham where SanPham.TrangThai = 1
end
GO
/****** Object:  StoredProcedure [dbo].[LayDSTaiKhoan]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayDSTaiKhoan]
AS
BEGIN
select* from ThongTinKhachHangVaTaiKhoan
END
GO
/****** Object:  StoredProcedure [dbo].[Laydstp]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Laydstp]
as
begin
select* from City
end
GO
/****** Object:  StoredProcedure [dbo].[LayGiaVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
                 
CREATE PROCEDURE  [dbo].[LayGiaVanChuyen] @ViTri nvarchar(50), @MaDV int
as
begin
if(@Vitri = 'HCM')
	select GiaNoiThanh from DvVanChuyen where MaDvVanChuyen = @MaDV
	else
	select GiaNgoaiThanh from DvVanChuyen where MaDvVanChuyen = @MaDV
END
GO
/****** Object:  StoredProcedure [dbo].[LayHATop5SanPham]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayHATop5SanPham] @thang int, @nam int
AS
BEGIN
	select top(5) SanPham.tenSP,SanPham.Hinhanh
	from(select ChitietMua.MaSP,Sum(ChitietMua.SoLuong) as TongSo
	from ChitietMua
	where MONTH(ChitietMua.NgayDat) = @thang and YEAR(ChitietMua.NgayDat) = @nam 
	group by ChitietMua.MaSP) as Chitiet join SanPham on Chitiet.MaSP = SanPham.MaSp
	order by (Chitiet.TongSo) DESC
END

select* from ChitietMua
GO
/****** Object:  StoredProcedure [dbo].[LayLoaiSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[LayLoaiSP] @MaLoai int
as
begin
select* from LoaiSP where LoaiSP.maloai = @MaLoai
end
GO
/****** Object:  StoredProcedure [dbo].[LayLoaiSPShop]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[LayLoaiSPShop] @MaLoai int
as
begin
select* from SanPham where SanPham.Maloai = @MaLoai
end
GO
/****** Object:  StoredProcedure [dbo].[LaySanPham]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[LaySanPham] @ID int
as
begin
select* from SanPham where SanPham.MaSp = @ID and SanPham.TrangThai =1
end
GO
/****** Object:  StoredProcedure [dbo].[LaySoluong]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[LaySoluong] @ID int
as
begin
select SoLuong from KhoHang where KhoHang.MaSp = @ID
end
GO
/****** Object:  StoredProcedure [dbo].[LaySoLuongSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LaySoLuongSP] @Masp int
AS
BEGIN
	select * from KhoHang where MaSp = @Masp
END
GO
/****** Object:  StoredProcedure [dbo].[LayTaiKhoan]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[LayTaiKhoan] @Username nvarchar(30)
as
begin
select* from TaiKhoan where TaiKhoan = @Username
end
GO
/****** Object:  StoredProcedure [dbo].[LayThongTin]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[LayThongTin] @ID NVARCHAR(50)
as
begin
select* from KhachHang where KhachHang.Taikhoan = @ID
end
GO
/****** Object:  StoredProcedure [dbo].[layThongTinTaiKhoan]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[layThongTinTaiKhoan]
@UserName nvarchar(30)
AS
BEGIN
	select TenKH, NamSinh,DiaChi,Quan,ThanhPho,sdt,hinhanh
	from KhachHang
	where KhachHang.Taikhoan = @UserName
END
GO
/****** Object:  StoredProcedure [dbo].[LayTop10KhachHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[LayTop10KhachHang] @Thang int, @Nam int
AS
BEGIN
	select top(10) MaKH,sum(TongTien) as Tong 
	from DonHang
	where MONTH(DonHang.NgayDat) = @Thang and Year(DonHang.NgayDat) = @Nam 
	group by MakH
	order by Tong ASC
END
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Login] @TK nvarchar(50), @MK nvarchar(50)
as
begin
select [dbo].[Signin](@TK,@MK)
end
GO
/****** Object:  StoredProcedure [dbo].[ThemChitietDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ThemChitietDonHang]
@MaSP int,
@SoLuong int,
@Gia real,
@tenSP nvarchar(50)
AS
BEGIN
Declare @MaDH int;
Select @MaDH=MAX(DonHang.MaDH) From DonHang
	Insert into ChitietDonHang(MaDH,MaSP,SoLuong,Gia,tenSP)
	values (@MaDH,@MaSP,@SoLuong,@Gia,@tenSP)
END
GO
/****** Object:  StoredProcedure [dbo].[ThemDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ThemDonHang]
@MaKH int,
@TenKH nvarchar(50),
@DiaChi nvarchar(50),
@Quan nvarchar(50),
@ThanhPho nvarchar(30),
@Email nvarchar (100),
@SDT nvarchar(11),
@MaDvVanChuyen int,
@TongTien real,
@TrangThai nvarchar(10),
@NgayDat date
	
AS
BEGIN
	Insert into DonHang(MaKH,TenKH,DiaChi,Quan,ThanhPho,Email,sdt,MaDvVanChuyen,TongTien,TrangThai,NgayDat)
	values (@MaKH, @TenKH,@DiaChi,@Quan,@ThanhPho,@Email,@SDT,@MaDvVanChuyen,@TongTien,@TrangThai,@NgayDat)
END
GO
/****** Object:  StoredProcedure [dbo].[ThemDvVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ThemDvVanChuyen] @TenDv nvarchar(30),@GiaNoiThanh real,@GiaNgoaiThanh real
AS
BEGIN
 Insert into DvVanChuyen(TenDv,GiaNoiThanh,GiaNgoaiThanh) values(@TenDv,@GiaNoiThanh,@GiaNgoaiThanh) 

END
GO
/****** Object:  StoredProcedure [dbo].[ThemLoaiSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[ThemLoaiSP] @TenLoai nvarchar(50)
as
begin
Insert into LoaiSP(tenloai) values(@TenLoai)
end
GO
/****** Object:  StoredProcedure [dbo].[ThemSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ThemSP]
@TenSP nvarchar(50),
@MoTa nvarchar(Max),
@MaLoai int,
@Gia real,
@MauSac nvarchar(30),
@hinhanh nvarchar(200),
@Modify Datetime
AS 
BEGIN 
INSERT INTO SanPham(tenSP,MoTa,MaLoai,TrangThai,gia,Giamgia,mausac,SoLuongDangDat,Hinhanh,LastModify) 
VALUES (@TenSP,@MoTa,@MaLoai,1,@Gia,0,@MauSac,0,@hinhanh,@Modify);
END 
GO
/****** Object:  StoredProcedure [dbo].[ThemTK]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ThemTK]
@TaiKhoan nvarchar(30),
@MatKhau nvarchar(30),
@Email nvarchar(100),
@QuyenTruyCap nvarchar(2)
AS 
BEGIN 
INSERT INTO TaiKhoan(TaiKhoan,MatKhau,Email,QuyenTruyCap) 
VALUES (@TaiKhoan,@MatKhau,@Email,@QuyenTruyCap) 
END 
GO
/****** Object:  StoredProcedure [dbo].[ThongtinKH]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[ThongtinKH]
as
begin
select* from thongtinNguoiDung
end
GO
/****** Object:  StoredProcedure [dbo].[tinhDoanhThu]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[tinhDoanhThu] @Thang int, @Nam int
AS
BEGIN
	Select dbo.TinhDThu(@Thang,@Nam)
END
GO
/****** Object:  StoredProcedure [dbo].[XacNhanDonHang]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[XacNhanDonHang] @MaDH int
AS
BEGIN
Update DonHang set TrangThai ='Done' where MaDH = @MaDH
END
GO
/****** Object:  StoredProcedure [dbo].[XoaDvVanChuyen]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[XoaDvVanChuyen] @MaDv int
as
BEGIN
	Delete from DvVanChuyen
	where MaDvVanChuyen = @MaDv
END
GO
/****** Object:  StoredProcedure [dbo].[XoaLoaiSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[XoaLoaiSP] @MaLoai int
as
begin
update SanPham Set TrangThai = '0' where SanPham.MaLoai = @MaLoai;
Delete from LoaiSP where maloai = @MaLoai
end
GO
/****** Object:  StoredProcedure [dbo].[XoaSP]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XoaSP]
@MaSP int
AS 
BEGIN 
Update SanPham
set TrangThai = 0
WHERE MaSp = @MaSP
END 
GO
/****** Object:  StoredProcedure [dbo].[XoaTK]    Script Date: 11/24/2019 11:27:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XoaTK]
@TaiKhoan nvarchar(30)
AS 
BEGIN 
Update  KhachHang set KhachHang.TaiKhoan = 'disable' where KhachHang.Taikhoan = @TaiKhoan
delete from TaiKhoan
where TaiKhoan = @TaiKhoan
END 

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TaiKhoan"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "KhachHang"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ThongTinKhachHangVaTaiKhoan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ThongTinKhachHangVaTaiKhoan'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TaiKhoan"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "KhachHang"
            Begin Extent = 
               Top = 175
               Left = 48
               Bottom = 338
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'thongtinNguoiDung'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'thongtinNguoiDung'
GO
