namespace GestoreAlbergo.Services
{
    using GestoreAlbergo.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICameraService
    {
        Task<IEnumerable<Camera>> GetAllCamerasAsync();
        Task<Camera> GetCameraByIdAsync(int id); 
        Task AddCameraAsync(Camera camera);
        Task UpdateCameraAsync(Camera camera);
        Task DeleteCameraAsync(int id); 
    }
}
