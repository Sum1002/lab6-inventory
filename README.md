# Inventory Management System

A comprehensive ASP.NET Core web application for managing inventory, products, warehouses, suppliers, and shipments.

## Features

- **Product Management**: Add, edit, delete, and view products with details like name, description, unit price, category, and SKU
- **Warehouse Management**: Track warehouse locations and storage capacities
- **Supplier Management**: Maintain supplier contact information and company details
- **Inventory Tracking**: Monitor stock quantities across different warehouses using a junction table
- **Shipment Management**: Record and track product deliveries with automatic inventory updates
- **Data Validation**: Comprehensive client-side and server-side validation
- **SQLite Database**: Lightweight database with Entity Framework Core

## Entity Relationship Diagram (ERD)

```
Products (1) ←→ (Many) ProductWarehouses (Many) ←→ (1) Warehouses
    ↑                                                      ↑
    |                                                      |
    |                                                      |
Shipments (Many) ←→ (1) Suppliers

Relationships:
- Products and Warehouses: Many-to-Many through ProductWarehouse junction table
- Suppliers to Shipments: One-to-Many
- Products to Shipments: One-to-Many
- Warehouses to Shipments: One-to-Many
```

## Database Schema

### Products Table

- ProductId (Primary Key)
- Name (Required, max 100 characters)
- Description (max 200 characters)
- UnitPrice (Required, range 0.01-5000.00)
- Category (max 50 characters)
- SKU (max 20 characters, unique)

### Warehouses Table

- WarehouseId (Primary Key)
- Name (Required, max 50 characters)
- Location (Required, max 100 characters)
- StorageCapacity (non-negative integer)

### Suppliers Table

- SupplierId (Primary Key)
- CompanyName (Required, max 50 characters)
- ContactEmail (Required, valid email format, unique)
- ContactPhone (max 15 characters)
- Address (max 100 characters)

### ProductWarehouse Table (Junction)

- ProductWarehouseId (Primary Key)
- ProductId (Foreign Key to Products)
- WarehouseId (Foreign Key to Warehouses)
- Quantity (non-negative integer)
- LastUpdated (DateTime, required)

### Shipments Table

- ShipmentId (Primary Key)
- SupplierId (Foreign Key to Suppliers)
- ProductId (Foreign Key to Products)
- WarehouseId (Foreign Key to Warehouses)
- Quantity (required, ≥1)
- ShipmentDate (DateTime, required)
- TotalCost (non-negative decimal, calculated as Quantity × UnitPrice)

## Technologies Used

- **.NET 8.0**: Latest stable version of .NET
- **ASP.NET Core MVC**: Web framework for building web applications
- **Entity Framework Core**: Object-relational mapping framework
- **SQLite**: Lightweight, serverless database
- **Bootstrap**: CSS framework for responsive design
- **jQuery**: JavaScript library for DOM manipulation
- **jQuery Validation**: Client-side validation library

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Run the application:
   ```bash
   dotnet run
   ```
5. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Database Setup

The application uses SQLite with Entity Framework Core. The database will be automatically created when you first run the application. The database file (`InventoryManagement.db`) will be created in the project root directory.

## Project Structure

```
lab6-inventory/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── ProductsController.cs
│   ├── WarehousesController.cs
│   ├── SuppliersController.cs
│   └── ShipmentsController.cs
├── Data/                 # Data Access Layer
│   └── InventoryContext.cs
├── Models/               # Entity Models
│   ├── Product.cs
│   ├── Warehouse.cs
│   ├── Supplier.cs
│   ├── ProductWarehouse.cs
│   ├── Shipment.cs
│   └── ErrorViewModel.cs
├── Views/                # Razor Views
│   ├── Home/
│   ├── Products/
│   ├── Warehouses/
│   ├── Suppliers/
│   ├── Shipments/
│   └── Shared/
├── wwwroot/              # Static Files
│   ├── css/
│   └── js/
├── Program.cs            # Application Entry Point
├── appsettings.json      # Configuration
└── README.md
```

## Usage

### Managing Products

1. Navigate to Products from the main menu
2. Click "Create New Product" to add a new product
3. Fill in the required fields (Name, UnitPrice)
4. Optionally add Description, Category, and SKU
5. Click Create to save the product

### Managing Warehouses

1. Navigate to Warehouses from the main menu
2. Click "Create New Warehouse" to add a new warehouse
3. Fill in the required fields (Name, Location)
4. Set the StorageCapacity
5. Click Create to save the warehouse

### Managing Suppliers

1. Navigate to Suppliers from the main menu
2. Click "Create New Supplier" to add a new supplier
3. Fill in the required fields (CompanyName, ContactEmail)
4. Optionally add ContactPhone and Address
5. Click Create to save the supplier

### Managing Shipments

1. Navigate to Shipments from the main menu
2. Click "Create New Shipment" to add a new shipment
3. Select the Supplier, Product, and Warehouse
4. Enter the Quantity and ShipmentDate
5. The TotalCost will be automatically calculated
6. Click Create to save the shipment and update inventory

## Data Validation

The application includes comprehensive validation:

- **Client-side validation**: Using jQuery Validation and data annotations
- **Server-side validation**: Using ModelState validation
- **Database constraints**: Unique constraints on SKU and email
- **Business logic validation**: Quantity must be positive, prices within range

## Sample Data

The application comes with pre-seeded sample data:

- **Products**: Laptop, Smartphone, Desk Chair
- **Warehouses**: Main Warehouse, East Coast Hub, West Coast Hub
- **Suppliers**: Tech Supplies Inc., Office Furniture Co., Global Electronics
- **Sample shipments and inventory records**

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License.

## Support

For support and questions, please open an issue in the repository.
