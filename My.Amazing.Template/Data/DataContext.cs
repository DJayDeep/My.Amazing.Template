using Microsoft.EntityFrameworkCore;

namespace My.Amazing.Template.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
    }
}
