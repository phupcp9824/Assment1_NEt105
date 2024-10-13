using WebApi.Models;

namespace Data.Repository.IRepository
{
    public interface IRepCombo
    {
        Task<List<Combo>> GetAllCombo();
        Task<Combo> CreateCombo(Combo combo);
        Task<Combo> UpdateCombo(Combo combo);
        Task<Combo> GetById(int id);
        Task<Combo> DeleteCombo(int id);
        Task<List<Combo>> GetByName(string name);


    }
}
