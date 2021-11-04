USE [OakbrookShop]
GO

/****** Object:  Table [dbo].[Menu]    Script Date: 08/10/2021 11:36:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
DROP TABLE [ResetToken];
DROP TABLE [OrderedProducts];
DROP TABLE [BasketItems];
DROP TABLE [Comments];
DROP TABLE [Devices];
DROP TABLE [Basket];
DROP TABLE [Orders];
DROP TABLE [Address];
DROP TABLE [Cards];
DROP TABLE [Products];
DROP TABLE [Captcha];
DROP TABLE [BulkOrders];
DROP TABLE [Users];


CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[ProfilePicture] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Users__3214EC073F5B6444] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[BulkOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OrderIds] [nvarchar](MAX) NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__BulkOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ResetToken](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[Valid] [nvarchar](50) NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__ResetToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Devices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserAgent] [nvarchar](MAX) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,	
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Captcha](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Question] [nvarchar](50) NOT NULL,
	[Answer] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__CaptchaId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Basket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__BasketId] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Price] [nvarchar](250) NOT NULL,
	[Image] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK__Products__3214EC073F5B6444] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[BasketItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[BasketId] [int] NOT NULL,
	FOREIGN KEY (ProductId) REFERENCES Products(Id),
	FOREIGN KEY (BasketId) REFERENCES Basket(Id),
 CONSTRAINT [PK__BasketItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Value] [nvarchar](MAX) NOT NULL,
	[Rating] [int] NOT NULL,
	[Summary] [nvarchar](MAX) NOT NULL,
	[PostedAt] [DateTime2] Not NULL,
	FOREIGN KEY (ProductId) REFERENCES Products(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__Comment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Address](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PostCode] [nvarchar](50) NOT NULL,
	[HouseNumber] [int] NOT NULL,
	[StreetAddress] [nvarchar](MAX) NOT NULL,
	[PhoneNumber] [nvarchar](MAX) NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__Address] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Cards](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Number] [nvarchar](4) NOT NULL,
	[Expiry] [nvarchar](8) NOT NULL,
	[CVV] [int] NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
 CONSTRAINT [PK__Cards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AddressId] [int] NOT NULL,
	[CardId] [int] NOT NULL,
	[Discount] [nvarchar](50) NOT NULL,
	[DiscountCode] [nvarchar](50) NOT NULL,
	[Delivery] [nvarchar](50) NOT NULL,
	[CreatedAt] [DateTime2] NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(Id),
	FOREIGN KEY (AddressId) REFERENCES Address(Id),
	FOREIGN KEY (CardId) REFERENCES Cards(Id),
 CONSTRAINT [PK__Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[OrderedProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [nvarchar](50) NOT NULL,
	FOREIGN KEY (OrderId) REFERENCES Orders(Id),
	FOREIGN KEY (ProductId) REFERENCES Products(Id),
 CONSTRAINT [PK__OrderedProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO Users (FirstName, LastName, Username, Password, Email, Role, ProfilePicture) VALUES ('Admin','Istrator','Administrator','{{Password}}', 'Administrator@example.com','Admin', '/imgs/default_profile_0.png')
INSERT INTO Users (FirstName, LastName, Username, Password, Email, Role, ProfilePicture) VALUES ('Dave','Lee','Davvey','{{Password}}', 'davey@example.com','Admin', '{{Dave_Picture}}')
INSERT INTO Captcha (Question, Answer) VALUES ('1 + 3 x 2 = ?', '7');
INSERT INTO Products (Name, Image, Price) VALUES ('20 Chicken McNuggets ShareBox', '20-Chicken-McNuggets-ShareBox.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Apple Grape Fruit Bag', 'Apple-Grape-Fruit-Bag.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Apple Pie', 'Apple-Pie.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Bacon Double Cheeseburger', 'Bacon-Double-Cheeseburger.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Bacon Mayo Chicken', 'Bacon-Mayo-Chicken.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Banana Milkshake Medium', 'Banana-Milkshake-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Big Mac', 'Big-Mac.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Black Coffee Regular', 'Black-Coffee-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Breakfast Roll with Brown Sauce', 'Breakfast-Roll-with-Brown-Sauce.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Breakfast Roll with Ketchup', 'Breakfast-Roll-with-Ketchup.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Buxton Mineral Water Still 250ml', 'Buxton-Mineral-Water-Still-250ml.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Buxton Mineral Water Still', 'Buxton-Mineral-Water-Still.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('cadbury crunchie mcflurry', 'cadbury-crunchie-mcflurry.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Cappuccino Regular', 'Cappuccino-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Caramel Iced Frapp Regular', 'Caramel-Iced-Frapp-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Carrot Bag', 'Carrot-Bag.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Cheeseburger', 'Cheeseburger.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chicken Legend with BBQ Sauce', 'Chicken-Legend-with-BBQ-Sauce.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chicken Legend with Cool Mayo', 'Chicken-Legend-with-Cool-Mayo.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chicken Legend with Hot Spicy Mayo', 'Chicken-Legend-with-Hot-Spicy-Mayo.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chicken McNuggets 6 pieces', 'Chicken-McNuggets-6-pieces.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chicken Selects 3 pieces', 'Chicken-Selects-3-pieces.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Chocolate Milkshake Medium', 'Chocolate-Milkshake-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Coca Cola Medium', 'Coca-Cola-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Coca Cola Zero Sugar Medium', 'Coca-Cola-Zero-Sugar-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Crispy Chicken Bacon Salad', 'Crispy-Chicken-Bacon-Salad.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Crispy Chicken Salad', 'Crispy-Chicken-Salad.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Diet Coke Medium', 'Diet-Coke-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Double Big Mac', 'Double-Big-Mac.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Double Cheeseburger', 'Double-Cheeseburger.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Double Espresso', 'Double-Espresso.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Double Quarter Pounder', 'Double-Quarter-Pounder.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Fanta Orange Medium', 'Fanta-Orange-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Filet o Fish', 'Filet-o-Fish.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Flat White', 'Flat-White.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Fries Medium', 'Fries-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Frozen Strawberry Lemonade Regular', 'Frozen-Strawberry-Lemonade-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Hamburger', 'Hamburger.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Hot Chocolate Regular', 'Hot-Chocolate-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Latte Regular', 'Latte-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Maltesers McFlurry', 'Maltesers-McFlurry.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Mayo Chicken', 'Mayo-Chicken.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('McChicken Sandwich', 'McChicken-Sandwich.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('McPlant', 'McPlant.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Millionaires Donut Xmas', 'Millionaires-Donut-Xmas.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Muffin With Jam', 'Muffin-With-Jam.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Oasis Medium', 'Oasis-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Oreo McFlurry', 'Oreo-McFlurry.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Organic Milk', 'Organic-Milk.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Quarter Pounder with Cheese', 'Quarter-Pounder-with-Cheese.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Robinsons Fruit Shoot', 'Robinsons-Fruit-Shoot.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Salted Caramel Latte Regular', 'Salted-Caramel-Latte-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Shaker Side Salad', 'Shaker-Side-Salad.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Smarties McFlurry', 'Smarties-McFlurry.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Spicy Nacho Cheese Wedges', 'Spicy-Nacho-Cheese-Wedges.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Sprite Zero Medium', 'Sprite-Zero-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Strawberry Milkshake Medium', 'Strawberry-Milkshake-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Tea Regular', 'Tea-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('The BBQ Chicken Bacon One Crispy', 'The-BBQ-Chicken-Bacon-One-Crispy.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('The Spicy Veggie One', 'The-Spicy-Veggie-One.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('The Sweet Chilli Chicken One Crispy', 'The-Sweet-Chilli-Chicken-One-Crispy.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Toffee Latte Regular', 'Toffee-Latte-Regular.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Triple Cheeseburger', 'Triple-Cheeseburger.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Tropicana Orange Juice', 'Tropicana-Orange-Juice.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Vanilla Milkshake Medium', 'Vanilla-Milkshake-Medium.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Vegetable Deluxe', 'Vegetable-Deluxe.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Veggie Dippers 2', 'Veggie-Dippers-2.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Veggie Dippers 4', 'Veggie-Dippers-4.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Water Deep River Rock 500ml', 'Water-Deep-River-Rock-500ml.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('Water hm Deep River Rock 250ml', 'Water-hm-Deep-River-Rock-250ml.jpg','9.99');
INSERT INTO Products (Name, Image, Price) VALUES ('White Coffee Regular', 'White-Coffee-Regular.jpg','9.99');