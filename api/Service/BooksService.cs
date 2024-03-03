using api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Service;


public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;
    private readonly AppSettings _appSettings;

    public BooksService(AppSettings appSetting)
    {
        _appSettings = appSetting;
        var mongoClient = new MongoClient(
            _appSettings.ConnectionStrings.DefaultConnection);

        var mongoDatabase = mongoClient.GetDatabase(
            _appSettings.ConnectionStrings.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            _appSettings.ConnectionStrings.BooksCollectionName);
    }

    public async Task<List<Book>> GetAsync() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);

    public void Aggregate()
    {

        var matchStage = Builders<Book>.Filter.Lte(u => u.Price, 50);
        var aggregate = _booksCollection.Aggregate().Match(matchStage);
        var results = aggregate.ToList();

        foreach (var account in results)
        {
            Console.WriteLine(account.Price);
        }
    }
    public void Group()
    {

        var matchStage = Builders<Book>.Filter.Lte(u => u.Price, 50);
        var aggregate = _booksCollection.Aggregate()
            .Match(matchStage).Group(
                x => x.Price,
                u => new
                {
                    Name = u.Key,
                    Total = u.Sum(x => 1)
                    //sum of the values within each group. Since it's using 1 as the value to sum, it effectively counts the number of documents in each group.
                });
        var results = aggregate.ToList();

        foreach (var account in results)
        {
            Console.WriteLine(account.Name + " " + account.Total);
        }
    }

    public void Sort()
    {

        var matchStage = Builders<Book>.Filter.Lte(u => u.Price, 50);
        var aggregate = _booksCollection.Aggregate()
            .Match(matchStage).Sort(Builders<Book>.Sort.Descending("BookName"));
        var results = aggregate.ToList();
    }

    public void MultipleLinQ()
    {
        var aggregate = _booksCollection.Find(x => x.Price >= 5)
                                        .SortByDescending(x => x.Price)
                                        .Skip(1)
                                        .Limit(20);
        var results = aggregate.ToList();

        foreach (var account in results)
        {
            Console.WriteLine(account.Price);
        }
    }

    public void Update()
    {

    }
}
