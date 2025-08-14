using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Data;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly InventoryContext _context;

        public ShipmentsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Shipments
        public async Task<IActionResult> Index()
        {
            var shipments = await _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .ToListAsync();
            return View(shipments);
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Shipments/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name");
            return View();
        }

        // POST: Shipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,ProductId,WarehouseId,Quantity,ShipmentDate,TotalCost")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                // Calculate total cost based on product unit price and quantity
                var product = await _context.Products.FindAsync(shipment.ProductId);
                if (product != null)
                {
                    shipment.TotalCost = product.UnitPrice * shipment.Quantity;
                }

                _context.Add(shipment);
                await _context.SaveChangesAsync();

                // Update inventory in ProductWarehouse
                await UpdateInventory(shipment.ProductId, shipment.WarehouseId, shipment.Quantity);

                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipmentId,SupplierId,ProductId,WarehouseId,Quantity,ShipmentDate,TotalCost")] Shipment shipment)
        {
            if (id != shipment.ShipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Calculate total cost based on product unit price and quantity
                    var product = await _context.Products.FindAsync(shipment.ProductId);
                    if (product != null)
                    {
                        shipment.TotalCost = product.UnitPrice * shipment.Quantity;
                    }

                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.ShipmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "CompanyName", shipment.SupplierId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Name", shipment.ProductId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Name", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Supplier)
                .Include(s => s.Product)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.ShipmentId == id);
        }

        private async Task UpdateInventory(int productId, int warehouseId, int quantity)
        {
            var productWarehouse = await _context.ProductWarehouses
                .FirstOrDefaultAsync(pw => pw.ProductId == productId && pw.WarehouseId == warehouseId);

            if (productWarehouse != null)
            {
                // Update existing inventory
                productWarehouse.Quantity += quantity;
                productWarehouse.LastUpdated = DateTime.Now;
                _context.Update(productWarehouse);
            }
            else
            {
                // Create new inventory record
                var newInventory = new ProductWarehouse
                {
                    ProductId = productId,
                    WarehouseId = warehouseId,
                    Quantity = quantity,
                    LastUpdated = DateTime.Now
                };
                _context.ProductWarehouses.Add(newInventory);
            }

            await _context.SaveChangesAsync();
        }
    }
}
