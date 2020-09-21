using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;
using XbooxLibrary.Services;
using XbooxCMS.Helper;
using System.Diagnostics;

namespace XbooxLibrary.Services
{
    public class ProductService
    {
        public static List<string> ImgstringList = null;
        public static List<string> GetImg()
        {
            if (ImgstringList == null)
            {
                ImgstringList = new List<string>();
            }
            return ImgstringList;
        }
        public void SetNull()
        {
            ImgstringList = null;
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
            var temps = repository.GetFirst(x => x.ProductId == id);
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

                    Description = input.Description,
                  
                    Price = input.Price
                };

                PutImgs(entity);
                //加入Img
                repository.Create(entity);
                repository.SaveContext();
                //加入tag
                AddedTag(entity, input.PostedTagIds);
                //context.SaveChanges();
               // repository.SaveContext();

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
                Name = item.Name,
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



        public List<TagViewModel> GetSelectedTags(Product product)
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Product> Pdrepository = new GeneralRepository<Product>(context);
            GeneralRepository<Tags> Tagrepository = new GeneralRepository<Tags>(context);
            GeneralRepository<ProductTags> ProductTagrepository = new GeneralRepository<ProductTags>(context);
            List<TagViewModel> TagLists = new List<TagViewModel>();
            var temps = ProductTagrepository.GetAll().Where(x=>x.ProductId== product.ProductId).Select(x => x.TagId).ToList();

            foreach (var t in temps)
            {

                TagLists.Add(new TagViewModel() { TagId = (Guid)t, TagName = context.Tags.Where(x => x.TagId == t).Select(x => x.TagName).FirstOrDefault() });
               
            };
            return TagLists;
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
                Description = item.Description,
                Language = item.Language

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
                //context.ProductTags.Add(entity);
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

            pdtagRepo.SaveContext();
           // context.SaveChanges();

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

    public CreateListViewModel CreateEditList(Product getproduct)
    {
            var viewmodel = new CreateListViewModel();
            viewmodel.ProductId = getproduct.ProductId;
            viewmodel.Author = getproduct.Author;
            viewmodel.CategoryId = getproduct.CategoryId;
            viewmodel.Intro = getproduct.Intro;
            viewmodel.Name = getproduct.Name;
            viewmodel.Price = getproduct.Price;
            viewmodel.UnitInStock = getproduct.UnitInStock;
            viewmodel.Specification = getproduct.Specification;
            viewmodel.PublishedDate = getproduct.PublishedDate;
            viewmodel.Publisher = getproduct.Publisher;
            viewmodel.ProductImgId = viewmodel.ProductImgId;
            viewmodel.ISBN = getproduct.ISBN;
            viewmodel.Description = getproduct.Description;
            viewmodel.Language = getproduct.Language;
            viewmodel.Tags = GetTags();
            viewmodel.CategoryViewModels = GetCatecory();

            var tagLists = GetSelectedTags(getproduct);

            viewmodel.SelectedTags = tagLists;


            return viewmodel;

        }
    public OperationResult Edit(CreateDataModel input)
    {
        var result = new OperationResult();
        XbooxLibraryDBContext context = new XbooxLibraryDBContext();
        GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
        try
        {

            var productInDb = repository.GetFirst(p => p.ProductId == input.ProductId);

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
            productInDb.ProductImgId = input.ProductImgId;
             repository.Update(productInDb);
             repository.SaveContext();
                //加入tag
             AddedTag(productInDb, input.PostedTagIds);

              //  context.SaveChanges();
           

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

        var deleteProduct = repository.GetFirst(x => x.ProductId == id);
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



    public List<string> GetPictures(Guid id)
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
            GeneralRepository<ProductImgs> Imgrepository = new GeneralRepository<ProductImgs>(context);

            var imgList = Imgrepository.GetAll().Where(x => x.ProductId == id).Select(x=>x.imgLink).ToList();
            return imgList;
        }



    public void RemoveImg(string imgName)
    {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Product> repository = new GeneralRepository<Product>(context);
            GeneralRepository<ProductImgs> Imgrepository = new GeneralRepository<ProductImgs>(context);
           
             var charac = imgName.Split('"');
            foreach(var i in charac) { Debug.WriteLine(i); }
            var temp = charac[1];
        
            var imgToDelete = Imgrepository.GetAll().FirstOrDefault(x => x.imgLink == temp);
            if (imgToDelete != null)
            {
                Imgrepository.Delete(imgToDelete);
                Imgrepository.SaveContext();
            }
            else
            {
                
            }
          
          
     }

    }
}
