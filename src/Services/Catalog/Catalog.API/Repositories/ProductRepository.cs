﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogContext _catalogContext;

        public ProductRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Id, id);
            DeleteResult deleteResult = await _catalogContext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Category, categoryName);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(p => p.Name , name);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext.Products.Find(p => true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateProduct = await _catalogContext.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateProduct.IsAcknowledged && updateProduct.ModifiedCount > 0;
        }
    }
}
