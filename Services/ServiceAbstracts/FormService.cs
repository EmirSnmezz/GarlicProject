using System.Linq.Expressions;

public class FormService : IFormService
{
    IFormDal _formDal;

    public FormService(IFormDal formDal)
    {
        _formDal = formDal;
    }
    public IResult Add(FormModel formModel)
    {
        formModel.IsActive = true;
        _formDal.Add(formModel);
        return new SuccessResult("Form Başarıyla Gönderildi.");
    }

    public IResult ChangeStatus(FormModel formModel)
    {
        formModel.IsActive = !formModel.IsActive;
        _formDal.Update(formModel);
        return new SuccessResult("Siparişin Aktiflik Durumu Değiştirildi");
    }

    public IDataResult<List<FormModel>> GetAll(Expression<Func<FormModel, bool>> filter = null)
    {
      var result = _formDal.GetAll(filter);

        if(result is not null)
        {

            return new SuccessDataResult<List<FormModel>>(result);
        }

        return new ErrorDataResult<List<FormModel>>("Görüntülenecek ürün bulunamadı", null);
    }

    public IDataResult<FormModel> GetById(Expression<Func<FormModel, bool>> filter)
    {
        var result = _formDal.GetAll(filter).FirstOrDefault();
        if(result is not null)
        {
            return new SuccessDataResult<FormModel>(result);
        }

        return new ErrorDataResult<FormModel>("Görüntülenecek ürün bulunamadı");
    }

    public IResult Remove(FormModel formModel)
    {
        throw new NotImplementedException();
    }
}