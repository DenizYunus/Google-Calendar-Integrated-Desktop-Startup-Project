using OctoBackend.Domain.Collections.BaseCollection;

namespace OctoBackend.Application.Models
{
    public class Response
    {
        public Response()
        {
            Success = false;
        }
        public bool Success { get; set; }

        public Message? Message { get; set; }
    }

    public class CreateResponse<TCollection> : Response where TCollection : class
    {
        public TCollection? Result { get; set; } = null;
        public CreateResponse(TCollection? result)
        {
            Result = result;
        }
        public CreateResponse()
        {

        }
    }
    public class GetOneResponse<TCollection> : Response where TCollection : class
    {
        public TCollection? Result { get; set; } = null;
        public GetOneResponse(TCollection? result)
        {
            Result = result;
        }
        public GetOneResponse()
        {

        }
    }
    public class GetManyResponse<TCollection> : Response where TCollection : class
    {
        public ICollection<TCollection>? Result { get; set; }
        public GetManyResponse(ICollection<TCollection>? result)
        {
            Result = result;
        }
        public GetManyResponse()
        {

        }
    }
}
