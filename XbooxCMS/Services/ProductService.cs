﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace XbooxLibrary.Services
{
    public class ProductService
    {
        public List<ProductListViewModel> GetAllProducts()
        {

            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<ProductListViewModel> repository = new GeneralRepository<ProductListViewModel>(context);
            GeneralRepository<Product> Productpository = new GeneralRepository<Product>(context);
            GeneralRepository<Category> Categoryrepository = new GeneralRepository<Category>(context);
            GeneralRepository<Tags> Tagrepository = new GeneralRepository<Tags>(context);
            var pList = (from p in Productpository.GetAll()
                        join c in Categoryrepository.GetAll()
                        on p.CategoryId equals c.CategoryId
                        select new ProductListViewModel()
                        {
                            ProductId = p.ProductId,
                            Name = p.Name,
                            UnitInStock = p.UnitInStock,
                            Price = p.Price,
                            Publisher = p.Publisher,
                            Author = p.Author,
                            PublishedDate = p.PublishedDate,
                            CategorName = c.Name
                        }).ToList();
            return pList;
        }

        public Product GetSingleProduct(Guid id)
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
            var temp = repository.GetAll().FirstOrDefault(x => x.ProductId == id);
            var temps = repository.GetSingle(x => x.ProductId == id);
            return temps;
        }

        public OperationResult Create(CreateDataModel input)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
            
                //tag && img
                Product entity = new Product()
                {
                   ProductId = Guid.NewGuid(),
                   Name = input.Name,
                   CategoryId = input.CategoryId,
                   ISBN = input.ISBN,
                   Author = input.Author,
                   Specification = input.Specification,
                   Intro = input.Intro,
                   Language = input.Language,
                   UnitInStock = input.UnitInStock,
                   PublishedDate = input.PublishedDate,
                   Description = input.Description

                };
                //加入Img
                repository.Create(entity);
            
                //加入tag
                AddedTag(entity, input.PostedTagIds);
                repository.SaveContext();

                result.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccessful = false;
                result.exception = ex;
            }

            return result;
        }
        //public OperationResult Create(SalesManViewModel input)
        //{
        //    var result = new OperationResult();
        //    try
        //    {
        //        BizModel context = new BizModel();
        //        BizRepository<SalesMan> repository = new BizRepository<SalesMan>(context);
        //        SalesMan entity = new SalesMan() { Name = input.Name };
        //        repository.Create(entity);
        //        context.SaveChanges();
        //        result.IsSuccessful = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsSuccessful = false;
        //        result.exception = ex;
        //    }
        //    return result;
        //}

        public void AddedTag(Product product, List<Guid> SelectedTags)
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<ProductTags> pdtagRepo = new GeneralRepository<ProductTags>(context);
            
            if (SelectedTags == null)
            {
                return;
            }

            var pTagList = pdtagRepo.GetAll().Where(x => x.ProductId == product.ProductId).Select(x => (Guid)x.TagId).ToList();
            if (pTagList.Count == 0)
            {
                //create
                foreach (var t in SelectedTags)
                {
                    ProductTags entity = new ProductTags()
                    {
                        ProductId = product.ProductId,
                        TagId = t,
                        Id = Guid.NewGuid()
                    };

                    pdtagRepo.Create(entity);
                    //context.ProductTags.Add(tags);
                }
            }
            else
            {
                //找出原本有選但現在沒選的Tag
                var newTagList1 = pTagList.Except(SelectedTags);


                //把現在沒選的Tag移除
                foreach (var t in newTagList1)
                {

                    var item = pdtagRepo.GetAll().Where(x => x.TagId == t && x.ProductId == product.ProductId).FirstOrDefault();
                    pdtagRepo.Delete(item);
                   // context.ProductTags.Remove(item);
                }

                //找出現在有選但原本沒選的Tag
                var newTagList2 = SelectedTags.Except(pTagList);

                //加入沒選過的tag
                foreach (var t in newTagList2)
                {
                    ProductTags entity = new ProductTags()
                    {
                        ProductId = product.ProductId,
                        TagId = t,
                        Id = Guid.NewGuid()
                    };
                    pdtagRepo.Create(entity);
                }
            }



        }
        public void Edit()
        {

        }

        public void Delete()
        {

        }

        




    }
}
