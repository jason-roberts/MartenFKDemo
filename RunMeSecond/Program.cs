using System;
using Marten;
using MyDocs;

namespace MyDocs
{
    public class A
    {
        public int Id { get; set; }
        public int ForeignKeyB { get; set; }
    }

    public class B
    {
        public int Id { get; set; }
    }
}

namespace RunMeSecond
{

    class Program
    {
        static void Main(string[] args)
        {
            const string cnString =
               "host = localhost; database = MartenSpike; password = YOUR PASSWORD HERE; username = postgres";

            using (var ds = DocumentStore.For(options =>
            {
                options.Connection(cnString);

                options.Schema.For<A>().ForeignKey<B>(x => x.ForeignKeyB);
            }))
            {
               
                // insert with correct fk
                using (var session = ds.LightweightSession())
                {
                    var b = new B();
                    session.Store(b);

                    var a = new A {ForeignKeyB = b.Id};
                    session.Store(a);

                    session.SaveChanges();
                }



                // insert with non existent fk
                using (var session = ds.LightweightSession())
                {
                    var nonExistentBId = 3413969;
                    var a = new A { ForeignKeyB =  nonExistentBId };
                    session.Store(a);

                    try
                    {
                        session.SaveChanges();

                        throw new ApplicationException("SaveChanges should have thrown a foreign key constraint violation exception but did not");
                    }
                    catch (Exception)
                    {                        
                        throw;
                    }
                    



                }
            }
        }
    }
}
