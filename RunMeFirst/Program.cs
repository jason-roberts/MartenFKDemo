using Marten;
using MyDocs;


namespace MyDocs
{
    public class A
    {
        public int Id { get; set; }
    }

    public class B
    {
        public int Id { get; set; }
    }
}

namespace RunMeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            const string cnString =
                "host = localhost; database = MartenSpike; password = YOUR PASSWORD HERE; username = postgres";
            
            using (var ds = DocumentStore.For(cnString))
            {
                ds.Advanced.Clean.CompletelyRemoveAll();

                using (var session = ds.LightweightSession())
                {
                    session.Store(new A());

                    session.SaveChanges();
                }
            }
        }
    }
}
