using System.Linq.Expressions;

public interface IFormService
{
    
    IResult Add (FormModel formModel);
    IDataResult<List<FormModel>> GetAll(Expression<Func<FormModel, bool>> filter);
    IDataResult<FormModel> GetById(Expression<Func<FormModel, bool>> filter);
    IResult Remove(FormModel formModel);
    IResult ChangeStatus (FormModel formModel);
}