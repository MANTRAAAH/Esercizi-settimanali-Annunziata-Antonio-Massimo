using GestoreAlbergo.Models;
using GestoreAlbergo.Services;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

public class CameraService : ICameraService
{
    private readonly string _connectionString;

    public CameraService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Camera>> GetAllCamerasAsync()
    {
        var cameras = new List<Camera>();

        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT Id, Numero, Descrizione, Tipologia FROM Camere", connection);
            connection.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    cameras.Add(new Camera
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                        Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                        Tipologia = reader.GetString(reader.GetOrdinal("Tipologia"))
                    });
                }
            }
        }

        return cameras;
    }

    public async Task<Camera> GetCameraByIdAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("SELECT Id, Numero, Descrizione, Tipologia FROM Camere WHERE Numero = @Numero", connection);
            command.Parameters.AddWithValue("@Numero", id);
            connection.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Camera
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                        Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                        Tipologia = reader.GetString(reader.GetOrdinal("Tipologia"))
                    };
                }
            }
        }
        return null;
    }

    public async Task AddCameraAsync(Camera camera)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("INSERT INTO Camere (Numero, Descrizione, Tipologia) VALUES (@Numero, @Descrizione, @Tipologia)", connection);
            command.Parameters.AddWithValue("@Numero", camera.Numero);
            command.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
            command.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdateCameraAsync(Camera camera)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("UPDATE Camere SET Numero = @Numero, Descrizione = @Descrizione, Tipologia = @Tipologia WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", camera.Id);
            command.Parameters.AddWithValue("@Numero", camera.Numero);
            command.Parameters.AddWithValue("@Descrizione", camera.Descrizione);
            command.Parameters.AddWithValue("@Tipologia", camera.Tipologia);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteCameraAsync(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("DELETE FROM Camere WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            await command.ExecuteNonQueryAsync();
        }
    }
}
