using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;
using XbooxLibrary.Services;

namespace XbooxLibrary.Services
{
    public class ProductService
    {
        private static List<string> ImgstringList = null;
        private static List<string> GetImg()
        {
            if (ImgstringList == null)
            {
                ImgstringList = new List<string>();
            }
            return ImgstringList;
        }
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
        //trycatch
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
                PutImgs(entity);
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

        public List<CreateListViewModel.CategoryViewModel> GetCatecory()
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Category> Categoryrepository = new GeneralRepository<Category>(context);

            var cateList = Categoryrepository.GetAll().Select(item => new CreateListViewModel.CategoryViewModel
            {
                CategorName = item.Name,
                CategoryId = item.CategoryId


            }).ToList();
            return cateList;
        }


        public List<TagViewModel> GetTags()
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Tags> Tagrepository = new GeneralRepository<Tags>(context);
            // var tagList = Tagrepository.GetAll().ToList();
            var tagList = Tagrepository.GetAll().Select(item => new TagViewModel
            {
                TagId = item.TagId,
                TagName = item.TagName


            }).ToList();
            return tagList;
        }


        public List<ProductListViewModel> GetProducts()
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Product> prepository = new GeneralRepository<Product>(context);
            // var tagList = Tagrepository.GetAll().ToList();
            var pList = prepository.GetAll().Select(item => new ProductListViewModel
            {
                Name = item.Name,
                Author = item.Author,

               CategoryId = item.CategoryId,
               Intro = item.Intro,
               ISBN = item.ISBN,
               Price = item.Price,
               PublishedDate = item.PublishedDate,
               Publisher = item.Publisher,
               UnitInStock = item.UnitInStock,
              Specification = item.Specification,
              ProductId = item.ProductId,
                Description = item.Description


            }).ToList();
            return pList;
        }


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

    private void PutImgs(Product product)
    {
        var productImgs = new ProductImgs();
        XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        GeneralRepository<ProductImgs> Imgrepo = new GeneralRepository<ProductImgs>(context);
        //   var imgList = getImg().Split(',').Where(x=>x!="").ToList();


        //用list傳的
        foreach (var i in GetImg())
        {
            productImgs = new ProductImgs()
            {
                // ProductImgId = 0,
                imgLink = i,
                ProductId = product.ProductId,
            };
            Imgrepo.Create(productImgs);
        }

        ImgstringList = null;
        Imgrepo.SaveContext();
    }

    public OperationResult Edit(CreateDataModel input)
    {
        var result = new OperationResult();
        XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
        try
        {

            var productInDb = repository.GetSingle(p => p.ProductId == input.ProductId);

            productInDb.Name = input.Name;
            productInDb.CategoryId = input.CategoryId;
            productInDb.ISBN = input.ISBN;
            productInDb.Author = input.Author;
            productInDb.Specification = input.Specification;
            productInDb.Intro = input.Intro;
            productInDb.Language = input.Language;
            productInDb.UnitInStock = input.UnitInStock;
            productInDb.PublishedDate = input.PublishedDate;
            productInDb.Description = input.Description;
            PutImgs(productInDb);
            //加入Img
            repository.Create(productInDb);

            //加入tag
            AddedTag(productInDb, input.PostedTagIds);
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

    public void Delete(Guid id)
    {
        XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
        GeneralRepository<ProductImgs> Imgrepository = new GeneralRepository<ProductImgs>(context);
        GeneralRepository<ProductTags> Ptrepository = new GeneralRepository<ProductTags>(context);

        var deleteProduct = repository.GetSingle(x => x.ProductId == id);
        var deleteImg = Imgrepository.GetAll().Where(x => x.ProductId == id);
        var deleteTag = Ptrepository.GetAll().Where(x => x.ProductId == id);


        if (deleteImg != null)
        {
            foreach (var i in deleteImg)
            {
                Imgrepository.Delete(i);

            }
        }
        if (deleteTag != null)
        {
            foreach (var i in deleteTag)
            {
                Ptrepository.Delete(i);
            }
        }
        //刪除product 刪除 tag

        repository.Delete(deleteProduct);
        context.SaveChanges();
    }




}
}
