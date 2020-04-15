namespace Aaron.Core.Managers
{
    public class CarPartManager
    {
        private readonly Database _database;

        public CarPartManager(Database database)
        {
            _database = database;
        }
    }
}