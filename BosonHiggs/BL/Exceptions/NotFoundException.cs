namespace BosonHiggsApi.BL.Exceptions
{
    public class NotFoundException: Exception
    {
        public NotFoundException():base("Requested data not found.")
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
