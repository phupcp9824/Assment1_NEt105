using WebApi.Models;

namespace Data.Repository.IRepository
{
    public interface IRepFood
    {
        Task<List<FastFoodItem>> GetAllFood();
        Task<FastFoodItem> CreateFood(FastFoodItem fastFoodItem);
        Task<FastFoodItem> UpdateFood(FastFoodItem fastFoodItem);
        Task<FastFoodItem> GetById(int id);
        Task<FastFoodItem> DeleteFood(int id);
        Task<List<FastFoodItem>> GetByName(string name);
        Task<FastFoodItem> GetByIdCate(int id);

    }
}
