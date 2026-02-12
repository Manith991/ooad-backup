using System;
using OOAD_Project.Domain;
using OOAD_Project.Patterns.Repository;

namespace OOAD_Project.Patterns.Command
{
    /// <summary>
    /// COMMAND PATTERN - Add Product Command
    /// Encapsulates the logic to add a product with undo capability
    /// </summary>
    public class ProductCommand : ICommand
    {
        private readonly Product _product;
        private readonly IRepository<Product> _repository;
        private int _insertedId = -1;

        public ProductCommand(Product product, IRepository<Product> repository)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                _insertedId = _repository.Add(_product);
                _product.ProductId = _insertedId;
                Console.WriteLine($"[AddProductCommand] Added product '{_product.ProductName}' with ID {_insertedId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AddProductCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_insertedId > 0)
            {
                try
                {
                    _repository.Delete(_insertedId);
                    Console.WriteLine($"[AddProductCommand] Undid addition of product ID {_insertedId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[AddProductCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Add Product: {_product.ProductName}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Update Product Command
    /// Encapsulates the logic to update a product with undo capability
    /// </summary>
    public class UpdateProductCommand : ICommand
    {
        private readonly Product _newProduct;
        private readonly IRepository<Product> _repository;
        private Product _oldProduct;

        public UpdateProductCommand(Product newProduct, IRepository<Product> repository)
        {
            _newProduct = newProduct ?? throw new ArgumentNullException(nameof(newProduct));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                // Save current state for undo
                _oldProduct = _repository.GetById(_newProduct.ProductId);

                if (_oldProduct == null)
                {
                    throw new InvalidOperationException($"Product with ID {_newProduct.ProductId} not found");
                }

                _repository.Update(_newProduct);
                Console.WriteLine($"[UpdateProductCommand] Updated product ID {_newProduct.ProductId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateProductCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_oldProduct != null)
            {
                try
                {
                    _repository.Update(_oldProduct);
                    Console.WriteLine($"[UpdateProductCommand] Restored product ID {_oldProduct.ProductId} to previous state");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UpdateProductCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return $"Update Product: {_newProduct.ProductName}";
        }
    }

    /// <summary>
    /// COMMAND PATTERN - Delete Product Command
    /// Encapsulates the logic to delete a product with undo capability
    /// </summary>
    public class DeleteProductCommand : ICommand
    {
        private readonly int _productId;
        private readonly IRepository<Product> _repository;
        private Product _deletedProduct;

        public DeleteProductCommand(int productId, IRepository<Product> repository)
        {
            _productId = productId;
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void Execute()
        {
            try
            {
                // Save product data before deletion for undo
                _deletedProduct = _repository.GetById(_productId);

                if (_deletedProduct == null)
                {
                    throw new InvalidOperationException($"Product with ID {_productId} not found");
                }

                _repository.Delete(_productId);
                Console.WriteLine($"[DeleteProductCommand] Deleted product '{_deletedProduct.ProductName}' (ID: {_productId})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DeleteProductCommand] Error executing: {ex.Message}");
                throw;
            }
        }

        public void Undo()
        {
            if (_deletedProduct != null)
            {
                try
                {
                    _repository.Add(_deletedProduct);
                    Console.WriteLine($"[DeleteProductCommand] Restored deleted product '{_deletedProduct.ProductName}'");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DeleteProductCommand] Error undoing: {ex.Message}");
                    throw;
                }
            }
        }

        public string GetDescription()
        {
            return _deletedProduct != null
                ? $"Delete Product: {_deletedProduct.ProductName}"
                : $"Delete Product ID: {_productId}";
        }
    }
}