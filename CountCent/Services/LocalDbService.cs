
using CountCent.Model;
using SQLite;

namespace CountCent.Services
{
    // encompasses necessary methods to interact with SQLite
    public class LocalDbService
    {
        private const string DB_NAME = "local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));

            _connection.CreateTableAsync<DataPoint>();
        }

        // methods for CRUD operations

        // will return a list of customers
        public async Task<List<DataPoint>> GetDataPoints()
        {
            return await _connection.Table<DataPoint>().ToListAsync();
        }

        // returns a single DataPoint record
        public async Task<DataPoint> GetById(int id)
        {
            return await _connection.Table<DataPoint>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Create(DataPoint dataPoint)
        {
            await _connection.InsertAsync(dataPoint);
        }

        public async Task Update(DataPoint dataPoint)
        {
            await _connection.UpdateAsync(dataPoint);
        }

        public async Task Delete(DataPoint dataPoint)
        {
            await _connection.DeleteAsync(dataPoint);
        }
    }
}
