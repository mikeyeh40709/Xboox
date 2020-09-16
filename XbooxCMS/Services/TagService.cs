using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XbooxCMS.ViewModels;
using XbooxLibrary.Models.DataTable;
using XbooxLibrary.Repository;

namespace XbooxLibrary.Services
{
    public class TagService
    {

        public OperationResult Create(Tags input)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Tags> repository = new GeneralRepository<Tags>(context);
                Tags entity = new Tags()
                { TagId = Guid.NewGuid(),
                  TagName = input.TagName 
                };
                repository.Create(entity);
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

        public OperationResult Edit(Tags input)
        {
            var result = new OperationResult();
            try
            {
                XbooxLibraryDBContext context = new XbooxLibraryDBContext();
                GeneralRepository<Tags> repository = new GeneralRepository<Tags>(context);
           
                repository.Update(input);
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

        public List<TagViewModel> GetTags()
        {
            var result = new TagViewModel();
        
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Tags> repository = new GeneralRepository<Tags>(context);
            var temp = from t in repository.GetAll()
                       select new TagViewModel()
                       {
                           TagId = t.TagId,
                           TagName = t.TagName
                       };
            
            return temp.ToList();
            
        }

        public Tags GetSingleTag(Guid id)
        {
            XbooxLibraryDBContext context = new XbooxLibraryDBContext();
            GeneralRepository<Tags> repository = new GeneralRepository<Tags>(context);
            var temp = repository.GetAll().FirstOrDefault(x=>x.TagId==id);
            var temps = repository.GetFirst(x => x.TagId == id);
            return temps;
        }
    }
}
