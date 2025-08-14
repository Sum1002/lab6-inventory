CREATE TABLE "ProductWarehouses" (
    "ProductWarehouseId" INTEGER NOT NULL CONSTRAINT "PK_ProductWarehouses" PRIMARY KEY AUTOINCREMENT,
    "ProductId" INTEGER NOT NULL,
    "WarehouseId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "LastUpdated" TEXT NOT NULL,
    CONSTRAINT "FK_ProductWarehouses_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("ProductId") ON DELETE CASCADE,
    CONSTRAINT "FK_ProductWarehouses_Warehouses_WarehouseId" FOREIGN KEY ("WarehouseId") REFERENCES "Warehouses" ("WarehouseId") ON DELETE CASCADE
)

CREATE TABLE "Products" (
    "ProductId" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Description" TEXT NULL,
    "UnitPrice" decimal(18,2) NOT NULL,
    "Category" TEXT NULL,
    "SKU" TEXT NULL
)

CREATE TABLE "Shipments" (
    "ShipmentId" INTEGER NOT NULL CONSTRAINT "PK_Shipments" PRIMARY KEY AUTOINCREMENT,
    "SupplierId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    "WarehouseId" INTEGER NOT NULL,
    "Quantity" INTEGER NOT NULL,
    "ShipmentDate" TEXT NOT NULL,
    "TotalCost" decimal(18,2) NOT NULL,
    CONSTRAINT "FK_Shipments_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("ProductId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Shipments_Suppliers_SupplierId" FOREIGN KEY ("SupplierId") REFERENCES "Suppliers" ("SupplierId") ON DELETE RESTRICT,
    CONSTRAINT "FK_Shipments_Warehouses_WarehouseId" FOREIGN KEY ("WarehouseId") REFERENCES "Warehouses" ("WarehouseId") ON DELETE RESTRICT
)

CREATE TABLE "Suppliers" (
    "SupplierId" INTEGER NOT NULL CONSTRAINT "PK_Suppliers" PRIMARY KEY AUTOINCREMENT,
    "CompanyName" TEXT NOT NULL,
    "ContactEmail" TEXT NOT NULL,
    "ContactPhone" TEXT NULL,
    "Address" TEXT NULL
)

CREATE TABLE "Warehouses" (
    "WarehouseId" INTEGER NOT NULL CONSTRAINT "PK_Warehouses" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Location" TEXT NOT NULL,
    "StorageCapacity" INTEGER NOT NULL
)

CREATE TABLE sqlite_sequence(name,seq)