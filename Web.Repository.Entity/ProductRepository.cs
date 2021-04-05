using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class ProductRepository : IProductRepository
    {
        private readonly MotorEntities context = new MotorEntities();
        public void Add(Product model)
        {
            context.Products.Add(model);
            context.SaveChanges();
        }
        public IEnumerable<ProductModel> ListAll(int satatus, string keyWork)
        {
            object[] parameters =
            {
               new SqlParameter("@Status", satatus),
               new SqlParameter("@Name", keyWork)
            };
            return context.Database.SqlQuery<ProductModel>("Sp_Product_ListAll @Status,@Name", parameters);
        }
        public void UpdateImages(int id,string images)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@ImageMore", images)
            };
            context.Database.ExecuteSqlCommand("Sp_Product_Update_Images @ID,@ColorId,@ModelId,@Image,@Price,@Sale,@Description", parameters);
        }
        public IEnumerable<Product> GetAll()
        {
            return context.Products;
        }
        public List<ProductModel> ProductGetByCategory(string linkseo)
        {
            return context.Database.SqlQuery<ProductModel>("Sp_Product_GetByCategory @LinkSeo", new SqlParameter("@LinkSeo", linkseo)).ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var product = Find(id);
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void Edit(Product model)
        {
            var obj = Find(model.ID);
            obj.CategoryId = model.CategoryId;
            obj.Name = model.Name;
            obj.Images = model.Images;
            obj.Price = model.Price;
            obj.Sale = model.Sale;
            obj.Description = model.Description;
            obj.Status = model.Status;
            context.Entry(obj);
            context.SaveChanges();
        }
        public Product Find(int id)
        {
            return context.Products.Find(id);
        }
        public  int TotalProduct()
        {
            return context.Products.Count();
        }
    }
}
